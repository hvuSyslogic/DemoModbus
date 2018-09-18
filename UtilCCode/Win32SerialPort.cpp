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
//    Creation Date: 07/23/2018
//    Description: Win32SerialPort to work with Serial Comm. Port 
//    this class abstract the read and write IO values to Serial Port
//
//************************************************************************
#include <string>
#include "stdafx.h"
#include "Win32SerialPort.h"
#include "Logging.h"

Win32SerialPort::Win32SerialPort()
	: m_PortHandle(INVALID_HANDLE_VALUE), m_bForAssert(FALSE), m_pMappedRxBuffer(nullptr), m_iMappedRxBufferLength(0)
{
	memset(m_abBuffer, '\0', g_iabBufferLength);
}

Win32SerialPort::~Win32SerialPort()
{
	Close();
}

BOOL Win32SerialPort::OpenWithDefault()
{
	// from Legacy code
	//Setup.Baud = BAUD_19K2;
	//Setup.DataLen = EIGHT_DATA_BITS;
	//Setup.StopLen = ONE_STOP_BIT;
	//Setup.ParityEnable = PARITY;
	//Setup.Parity = ODD_PARITY;
	return Open("\\\\.\\COM2", CBR_19200, 8, ODDPARITY, ONESTOPBIT);
}
BOOL Win32SerialPort::ResetTimeOutAndPurgeCommPort(LPCOMMTIMEOUTS pcommTimeOut, DWORD dwEventsMask)
{
	COMSTAT comStat;
	DWORD errorFlags;
	if (pcommTimeOut != nullptr)
	{
		m_bForAssert = SetCommTimeouts(m_PortHandle, pcommTimeOut);
		_ASSERTE(m_bForAssert);
	}
	m_bForAssert = ClearCommError(m_PortHandle, &errorFlags, &comStat);
	_ASSERTE(m_bForAssert);
	m_bForAssert = PurgeComm(m_PortHandle, PURGE_TXCLEAR | PURGE_RXCLEAR);
	_ASSERTE(m_bForAssert);
	m_bForAssert = SetCommMask(m_PortHandle, dwEventsMask);// | EV_RING | EV_RLSD);
	return TRUE;
}
BOOL Win32SerialPort::OpenWithCommPort( std::string strCommPortName)
{
	// from Legacy code
	//Setup.Baud = BAUD_19K2;
	//Setup.DataLen = EIGHT_DATA_BITS;
	//Setup.StopLen = ONE_STOP_BIT;
	//Setup.ParityEnable = PARITY;
	//Setup.Parity = ODD_PARITY;
	return Open(strCommPortName.c_str(), CBR_19200, 8, ODDPARITY, ONESTOPBIT);
}
BOOL Win32SerialPort::Open(LPCTSTR PortName, DWORD BaudRate, BYTE ByteSize, BYTE Parity, BYTE StopBits, DWORD DesiredAccess)
{
	bool bSetTimeOut = false;
	Close();
	// 
	m_PortHandle = CreateFile(PortName, DesiredAccess,/* GENERIC_READ | GENERIC_WRITE*/
								0, /* no share  */
								NULL, /* no security */
								OPEN_EXISTING,
								0,/* no threads */
								NULL /* no templates */);
	if (m_PortHandle != INVALID_HANDLE_VALUE)
	{
		DCB dcb;/*Device-Control Block*/
		SecureZeroMemory(&dcb,  sizeof(dcb));  /* clear the new struct  */
		dcb.DCBlength = sizeof(dcb);
		if (GetCommState(m_PortHandle, &dcb))
		{
			dcb.BaudRate = BaudRate;
			dcb.ByteSize = ByteSize;
			dcb.StopBits = StopBits;
			dcb.Parity = Parity;
			//_ASSERTE(dcb.fParity == 1);
			dcb.fDsrSensitivity = 0;
			dcb.fOutxCtsFlow = 0;
			dcb.fOutxDsrFlow = 0;
			dcb.fInX = 0;
			dcb.fOutX = 0;
			dcb.fDtrControl = DTR_CONTROL_DISABLE; //DTR and RTS 0
			dcb.fRtsControl = RTS_CONTROL_DISABLE;

			m_bForAssert = SetCommState(m_PortHandle, &dcb);
			_ASSERTE(m_bForAssert);
			COMMTIMEOUTS timeouts;
			SecureZeroMemory(&timeouts, sizeof(timeouts));  /* clear the new struct  */
			if (bSetTimeOut)
			{
				timeouts.ReadIntervalTimeout = 50; //maximum time IN ms allowed to elapse before the arrival of the next byte
				timeouts.ReadTotalTimeoutConstant = 50; //For each read operation, this value is added to the product of the ReadTotalTimeoutMultiplier member and the requested number of bytes
				timeouts.ReadTotalTimeoutMultiplier = 10;//this value is multiplied by the requested number of bytes to be read
				timeouts.WriteTotalTimeoutConstant = 50;
				timeouts.WriteTotalTimeoutMultiplier = 10;
			}
			else
			{
				timeouts.ReadIntervalTimeout = 0;// NOT USED
				timeouts.ReadTotalTimeoutMultiplier = 0;// NOT USED
				timeouts.ReadTotalTimeoutConstant = 0;/*CAN BE 0 */
				timeouts.WriteTotalTimeoutConstant = 0;/*CAN BE 0 */
				timeouts.WriteTotalTimeoutMultiplier = 0;
			}
			m_bForAssert = SetCommTimeouts(m_PortHandle, &timeouts);
			_ASSERTE(m_bForAssert);
			m_bForAssert = SetCommMask(m_PortHandle, EV_CTS | EV_DSR);// | EV_RING | EV_RLSD);
			_ASSERTE(m_bForAssert);
			m_bForAssert = PurgeComm(m_PortHandle, PURGE_TXCLEAR | PURGE_RXCLEAR);
			_ASSERTE(m_bForAssert);
			return TRUE;
		}
		else
		{
			// Set error message here.
			// Use GetLastError() to know the reason
			return FALSE;
		}
	}
	else
	{
		return FALSE; // Use GetLastError() to know the reason
	}
}

void Win32SerialPort::Close()
{
	if (m_PortHandle != INVALID_HANDLE_VALUE)
	{
		CloseHandle(m_PortHandle);
		m_PortHandle = INVALID_HANDLE_VALUE;
	}
}

BOOL Win32SerialPort::IsOpen()
{
	return (m_PortHandle != INVALID_HANDLE_VALUE);
}

DWORD Win32SerialPort::Read(LPVOID Buffer, DWORD BufferSize)
{
	DWORD Res(0);
	if (m_PortHandle != INVALID_HANDLE_VALUE)
	{
		ReadFile(m_PortHandle, Buffer, BufferSize, &Res, NULL);
	}
	return Res;
}

DWORD Win32SerialPort::Write(std::string bufferAsString)
{
	if (bufferAsString.empty())
	{
		LogMessage("**string from PC is empty.\n");
		return 0;
	}
	if (bufferAsString.length() > g_iabBufferLength)
	{
		LogMessage("**string from PC is oversized.\n");
		return 0;
	}

	CopyMemory(m_abBuffer, bufferAsString.c_str(), g_iabBufferLength);
	OutputDebugString((char*)m_abBuffer);
	LogMessage(bufferAsString.c_str());
	//if (m_abBuffer[1] == 0xA3)// keep alive , need to do read the reply
	//{
	//	int returnValue = Write((LPVOID)m_abBuffer, g_iabBufferLength);
	//	if (returnValue == g_iabBufferLength)
	//	{
	//		COMMTIMEOUTS timeouts;
	//		SecureZeroMemory(&timeouts, sizeof(timeouts));  /* clear the new struct  */
	//		timeouts.ReadIntervalTimeout = 100; //maximum time IN ms allowed to elapse before the arrival of the next byte
	//		timeouts.ReadTotalTimeoutConstant = 50; //For each read operation, this value is added to the product of the ReadTotalTimeoutMultiplier member and the requested number of bytes
	//		timeouts.ReadTotalTimeoutMultiplier = 10;//this value is multiplied by the requested number of bytes to be read
	//		timeouts.WriteTotalTimeoutConstant = 50;
	//		timeouts.WriteTotalTimeoutMultiplier = 10;
	//		// Clear Comm Port Rx & Tx buffer, wait for EV_RXCHAR
	//		ResetTimeOutAndPurgeCommPort(&timeouts, (EV_RXCHAR));
	//		char returnArray[8] = { 0 };
	//		Sleep(50);// Make sure the device has time to reply
	//		returnValue = Read((LPVOID)returnArray, g_iabBufferLength);
	//		LogMessage(returnArray);
	//		SecureZeroMemory(&timeouts, sizeof(timeouts));  /* clear the new struct  */
	//		ResetTimeOutAndPurgeCommPort(&timeouts, (EV_CTS | EV_DSR));
	//		return returnValue;
	//	}
	//}
	//else
	//{
	return	Write((LPVOID)m_abBuffer, g_iabBufferLength);
	//}
}
DWORD Win32SerialPort::Write(LPVOID Buffer, DWORD BufferSize)
{
	DWORD Res(0);
	//if (IsBadReadPtr(Buffer, BufferSize)) return -1;
	if (Buffer == nullptr || &( ((char*)Buffer)[BufferSize - 1] ) == nullptr) return -1;
	//the following is for debugging if needed.
	//char msg[80];
	//int idx = sprintf(msg, "FakeSerialWrite: BufferSize %d, hexBuffer:",  BufferSize);
	//int length = nLen < 8 ? nLen : 8;
	//for (int i = 0; i < length; i++)
	//	idx += sprintf(&msg[idx], " %02x", ((char*)Buffer)[i]);
	if (m_PortHandle != INVALID_HANDLE_VALUE)
	{
		//OutputDebugString((char*)msg);
		WriteFile(m_PortHandle, Buffer, BufferSize, &Res, NULL);
	}
	return Res;
}

// return -1 if bad parameters.
// return 0 if override memory data.
// return length otherwise
int Win32SerialPort::SetRxBuffer(PVOID pMappedRxBuffer, int iMappedRxBufferLength)
{
	if (pMappedRxBuffer == nullptr || &(((char*)pMappedRxBuffer)[iMappedRxBufferLength - 1]) == nullptr) return -1;
	if (m_pMappedRxBuffer == nullptr)
	{
		m_pMappedRxBuffer = pMappedRxBuffer;
		m_iMappedRxBufferLength = iMappedRxBufferLength;
		return m_iMappedRxBufferLength;
	}
	else // memory is overriden
	{
		m_pMappedRxBuffer = pMappedRxBuffer;
		m_iMappedRxBufferLength = iMappedRxBufferLength;
		return 0;
	}

}
BOOL Win32SerialPort::Get_CD_State()
{
	return this->Get_ModemSignalActive_State(MS_RLSD_ON);
}

BOOL Win32SerialPort::Get_CTS_State()
{
	return this->Get_ModemSignalActive_State(MS_CTS_ON);
}

BOOL Win32SerialPort::Get_DSR_State()
{
	return this->Get_ModemSignalActive_State(MS_DSR_ON);
}

BOOL Win32SerialPort::Get_RI_State()
{
	return this->Get_ModemSignalActive_State(MS_RING_ON);
}

void Win32SerialPort::Set_DTR_State(BOOL state)
{
	if (m_PortHandle != INVALID_HANDLE_VALUE)
	{
		EscapeCommFunction(m_PortHandle, (state ? SETDTR : CLRDTR));
	}
}

void Win32SerialPort::Set_RTS_State(BOOL state)
{
	if (m_PortHandle != INVALID_HANDLE_VALUE)
	{
		EscapeCommFunction(m_PortHandle, (state ? SETRTS : CLRRTS));
	}
}
BOOL Win32SerialPort::Get_ModemSignalActive_State(DWORD signalToGet)
{
	if (m_PortHandle != INVALID_HANDLE_VALUE)
	{
		DWORD ModemStat;
		if (GetCommModemStatus(m_PortHandle, &ModemStat))
		{
			return (ModemStat & signalToGet) > 0;
		}
		else
		{
			return FALSE;
		}
	}
	else
	{
		return FALSE;
	}
}


