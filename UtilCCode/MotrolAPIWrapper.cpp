//************************************************************************
//
//    This work contains valuable confidential and proprietary
//    information.  Disclosure, use or reproduction without the
//    written authorization of the company is prohibited.  This
//    unpublished work by the company is protected by the laws of the
//    United States and other countries.  If publication of the work
//    should occur the following notice shall apply:
//    Copyright Baldwin Technology Company, Inc. 1998/2018 All Rights Reserved
//
//    Creation Date: 07/30/2018
//    Description: MotrolAPIWrapper implementation for Win32SerialPort comms. and new thread
//    to run the process of accessing unique data set then process and put it 
//    to its blocking queue(the queue is blocked if it is empty). 
//
//************************************************************************

#include "MotrolAPIWrapper.h"
#include "Win32SerialPortWrapper.h"
#include "MACROS.H"
#include "VMELibrary.h"
#include "Logging.h"
#include <Windows.h>
#include <TIMER.H>
#include <mutex>
#ifndef _WIN32_WINNT
	# define _WIN32_WINNT 0x0600
#endif // !_WIN32_WINNT
std::mutex g_mutex;

int MotrolAPIEnable(const char* strCommPort)
{
	int returnValue = -1;
	if (strCommPort == NULL || (&(strCommPort[3]) == NULL)) return -1;
	std::string CommPortName("\\\\.\\");
	CommPortName.append(strCommPort);
	if (MotrolAPIWrapper::Instance().Enable(CommPortName))
	{
		returnValue = 0;
	}
	return returnValue;

}
void MotrolAPIDisable(const char* strCommPort)
{
	std::string CommPortName("\\\\.\\");
	CommPortName.append(strCommPort);
	MotrolAPIWrapper::Instance().Disable(CommPortName);
	return;
}
bool MapDataBuffers(PVOID ptxBuffer, int itxBufferLength, PVOID prxBuffer, int irxBufferLength)
{
	// rxBuffer does not need to be set all the time.
	//if (!IsBadReadPtr(prxBuffer, irxBufferLength))
	if (prxBuffer != NULL && (&(((char*)prxBuffer)[irxBufferLength - 1])) != NULL )
	{
		MotrolAPIWrapper::Instance().MapRxBuffer(prxBuffer, irxBufferLength);
	}
	if (((char*)ptxBuffer)[0] = 0x01) // only handle soh command buffer
	{
		return (MotrolAPIWrapper::Instance().AddMsgToSendSet((char*)ptxBuffer, itxBufferLength) > 0 );
	}
	return false;
}

DWORD GetLastErrorMessage(char* pacMessage, int ilength)
{
	// Retrieve the system error message for the last-error code
	LPVOID lpMsgBuf;
	DWORD dw = ::GetLastError();
	int iSize = FormatMessage(
		FORMAT_MESSAGE_ALLOCATE_BUFFER |
		FORMAT_MESSAGE_FROM_SYSTEM |
		FORMAT_MESSAGE_IGNORE_INSERTS,
		NULL,
		dw,
		MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT),
		(LPTSTR)&lpMsgBuf,
		0, NULL);
	if(iSize > 0)
	{
		iSize = MIN(iSize, (ilength - 1));
		memset(pacMessage, '\0', iSize);
		// copy the error message and exit the process
		CopyMemory(pacMessage, lpMsgBuf, iSize);
	}
	LogMessage(pacMessage);
	LocalFree(lpMsgBuf);
	return dw;

}
int GetCommPortName(char* pstrCommPortName, int iLength)
{
	if (pstrCommPortName == nullptr || &(pstrCommPortName[iLength - 1]) == nullptr) return -1;
	memset(pstrCommPortName, '\0', iLength);
	return MotrolAPIWrapper::Instance().m_strCommPortName.copy(pstrCommPortName, iLength - 1);
}
int GetSW_PROTOCOLS()
{
	return MotrolAPIWrapper::Instance().m_eSW_PROTOCOLS;
}

//---------------------------------------------------------------------------
// Function:  TimerRoutine
// Description: Thread timer callback function. 
// Parameters: 
//    lpParameter - 'this' pointer passed from Timequeue function.
// Return: 
//    Returns 0 (OK) or -1 .
//---------------------------------------------------------------------------
HANDLE gDoneEvent;
#define MILLI_SECOND_TO_NANO100(x)  (x * 1000 * 10)

// This method will be invoked every 1.5 sec to add 
// Keep alive message to the serial port which will be sent out
// by other thread.
VOID CALLBACK TimerRoutine(
	PTP_CALLBACK_INSTANCE Instance,
	PVOID                 Parameter,
	PTP_TIMER             Timer
)
{
	// Instance, and Timer not used in this TimerRoutine.
	UNREFERENCED_PARAMETER(Instance);
	UNREFERENCED_PARAMETER(Timer);
	if (Parameter == NULL)
	{
		ODS("TimerRoutine lpParameter is NULL\n");
		LogMessage("TimerRoutine lpParameter is NULL\n");
	}
	else
	{
		MotrolAPIWrapper* pmotrolAPIWrapper = static_cast<MotrolAPIWrapper*>(Parameter);
		//if (!IsBadReadPtr(pmotrolAPIWrapper, sizeof(MotrolAPIWrapper)))
		if(pmotrolAPIWrapper != nullptr)
		{
			pmotrolAPIWrapper->AddKeepAliveMsgToSendSet();
		}
	}


}

// -------------------------------------------------------------------------- -
// Function:  ProcessMsgsFromMebToWin32SerialPort
// Description: Thread callback function.  Dispatches to class defined procedure.
// Parameters: 
//    lpParameter - 'this' pointer passed from QueueUserWorkItem function.
// Return: 
//    Returns 0 (OK) or -1 .
//---------------------------------------------------------------------------
DWORD WINAPI ProcessMsgsFromMebToWin32SerialPort(LPVOID lpParameter)
{
	MotrolAPIWrapper* pmotrolAPIWrapper = static_cast<MotrolAPIWrapper*>(lpParameter);
	if (pmotrolAPIWrapper == nullptr) return -1;
	pmotrolAPIWrapper->MotrolProcessTask();
	ODS("ProcessMsgsFromMebToWin32SerialPort Exit\n");
	return 0;
}

MotrolAPIWrapper & MotrolAPIWrapper::Instance()
{
	static MotrolAPIWrapper instance;
	return instance;
}

MotrolAPIWrapper::MotrolAPIWrapper()
{
	m_pWin32SerialPortObject = new Win32SerialPort();
	m_bTerminate = false;
	m_ptpTimer = NULL;
	m_bdisposed = false;
	m_bSerialPortThreadStarted = false;
	m_strCommPortName = "COM2";
}


MotrolAPIWrapper::~MotrolAPIWrapper()
{
	Disable(m_strCommPortName);
}

BOOL MotrolAPIWrapper::Disable(std::string strCommPortName)
{
	if (m_bdisposed) return false;
	m_bTerminate = true;
	if (m_ptpTimer)
	{
		CleanUpTimer();
	}
	if (m_pWin32SerialPortObject != nullptr)
	{
		delete m_pWin32SerialPortObject;
		m_pWin32SerialPortObject = nullptr;
	}
	m_aBufferToSend.clear();
	m_aMotrolDecodeForWin32Serials.clear();
	m_bdisposed = true;
	return true;
}

BOOL MotrolAPIWrapper::Enable(std::string strCommPortName)
{
	//start the timer thread for Motrol's staying alive' BG
	if (m_pWin32SerialPortObject->OpenWithCommPort(strCommPortName))
	{
		if (m_ptpTimer == NULL && m_bEnableTimer)
		{
			// timer set for testing cycle of one-half second for now.
			if(!SetupTimer(100,1500))	throw "Error: Could not create MotrolAPIWrapper::threadpool timer.";
		}
		// Use a thread from thread pool
		return QueueUserWorkItem(ProcessMsgsFromMebToWin32SerialPort, this, WT_EXECUTEDEFAULT);
	}
	else
	{
		return FALSE;
	}
}

int MotrolAPIWrapper::MotrolProcessTask()
{
	ReceivedBufferForWin32Serial itemFromQueue;
	m_bSerialPortThreadStarted = true;
	DWORD dwReturnedValue(0);
	while (!m_bTerminate)
	{
		if (m_pWin32SerialPortObject != nullptr)
		{
			itemFromQueue = m_aBufferToSend.pop();
			//ODS(itemFromQueue.GetBuffer());
			dwReturnedValue = m_pWin32SerialPortObject->Write(itemFromQueue.GetBuffer(), itemFromQueue.Size());
			LogMessage(itemFromQueue.GetBuffer());
		}
		else
		{
			// serial port issue
			m_bTerminate = true;
			dwReturnedValue = -1;
			CleanUpTimer();
		}

	}
	OutputDebugString("MotrolAPIWrapper::StartMotrolProcess Exit\n");
	LogMessage("**MotrolAPIWrapper::StartMotrolProcess Exit\n");
	m_bSerialPortThreadStarted = false;
	return dwReturnedValue;
}

int MotrolAPIWrapper::AddKeepAliveMsgToSendSet()
{
	// testing ...ONLY
	int iReturnValue = 0;
	//int iValue = m_iCountForKeepAlive++ % 10;
	//if (iValue == 0)
	//{
	//	iReturnValue = AddMsgToSendSet(m_baKeepAliveBuffer, 7);
	//}
	//if (iValue == 1)
	//{
	//	//do nothing because keep alive just send.
	//	iReturnValue = 1;
	//}
	//else
	//{
	iReturnValue = (!m_bSwap) ? AddMsgToSendSet(m_baTestMoveForwardBuffer, MOTROL_REG_CONTROL_MSG_MAX_LENGH) : AddMsgToSendSet(m_baTestMoveBackwardBuffer, MOTROL_REG_CONTROL_MSG_MAX_LENGH);
	m_bSwap = !m_bSwap;
	//}
	return iReturnValue;
}

int MotrolAPIWrapper::MapRxBuffer(PVOID pMappedRxBuffer, int iMappedRxBufferLength)
{
	return m_pWin32SerialPortObject->SetRxBuffer(pMappedRxBuffer, iMappedRxBufferLength);
}

int MotrolAPIWrapper::AddMsgToSendSet(char* pMsg, int iMsgLength)
{
	if (pMsg == NULL || (&(pMsg[iMsgLength - 1]) == NULL) )return -1;
	int iReturnLength = 0;
	if (m_bSerialPortThreadStarted)
	{
		g_mutex.lock();
		iReturnLength = MIN(iMsgLength, SPI_RX_BUF_SIZE);
		CopyMemory((LPVOID)m_baTmpBuffer, (LPVOID)pMsg, iReturnLength);
		// Make sure to terminate with null
		//m_baTmpBuffer[iReturnLength] = '\0';
		g_mutex.unlock();
		ReceivedBufferForWin32Serial item(m_baTmpBuffer, iReturnLength);
		m_aBufferToSend.push(item);
		return iReturnLength;
	}
	else
	{
		return 0;
	}

}
BOOL MotrolAPIWrapper::CleanUpTimer()
{
	// Prevent callbacks after timer is closed
	SetThreadpoolTimer(m_ptpTimer, NULL, 0, 0);
	WaitForThreadpoolTimerCallbacks(m_ptpTimer, true);
	CloseThreadpoolTimer(m_ptpTimer);
	m_ptpTimer = NULL;
	LogMessage("**CleanUpTimer Before Exit\n");
	return true;
}
BOOL MotrolAPIWrapper::SetupTimer(DWORD msWaitToStartTimer, DWORD msInterval)
{
	FILETIME dueTime;
	*reinterpret_cast<PLONGLONG>(&dueTime) = -static_cast<LONGLONG>(MILLI_SECOND_TO_NANO100(msWaitToStartTimer));
	m_ptpTimer = CreateThreadpoolTimer(TimerRoutine, this, NULL);
	if (!m_ptpTimer)
	{
		LogMessage("**SetupTimer failed n");
		return false;
	}
	SetThreadpoolTimer(m_ptpTimer, &dueTime, msInterval, 0);
	return true;
}


