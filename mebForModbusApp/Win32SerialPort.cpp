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
#include "stdafx.h"
#include "Win32SerialPort.h"

Win32SerialPort::Win32SerialPort()
	: m_PortHandle(INVALID_HANDLE_VALUE), m_bForAssert(FALSE)
{

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
		SecureZeroMemory(&dcb, sizeof(dcb));  /* clear the new struct  */
		dcb.DCBlength = sizeof(dcb);
		if (GetCommState(m_PortHandle, &dcb))
		{
			dcb.BaudRate = BaudRate;
			dcb.ByteSize = ByteSize;
			dcb.StopBits = StopBits;
			dcb.Parity = Parity;
			_ASSERTE(dcb.fParity == 1);
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
				timeouts.ReadIntervalTimeout = 50;
				timeouts.ReadTotalTimeoutConstant = 50;
				timeouts.ReadTotalTimeoutMultiplier = 10;
				timeouts.WriteTotalTimeoutConstant = 50;
				timeouts.WriteTotalTimeoutMultiplier = 10;
			}
			else
			{
				timeouts.ReadIntervalTimeout = 0;
				timeouts.ReadTotalTimeoutMultiplier = 0;
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

DWORD Win32SerialPort::Write(LPVOID Buffer, DWORD BufferSize)
{
	DWORD Res(0);
	if (m_PortHandle != INVALID_HANDLE_VALUE)
	{
		WriteFile(m_PortHandle, Buffer, BufferSize, &Res, NULL);
	}
	return Res;
}

BOOL Win32SerialPort::Get_CD_State()
{
	if (m_PortHandle != INVALID_HANDLE_VALUE)
	{
		DWORD ModemStat;
		if (GetCommModemStatus(m_PortHandle, &ModemStat))
		{
			return (ModemStat & MS_RLSD_ON) > 0; //Not sure
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

BOOL Win32SerialPort::Get_CTS_State()
{
	if (m_PortHandle != INVALID_HANDLE_VALUE)
	{
		DWORD ModemStat;
		if (GetCommModemStatus(m_PortHandle, &ModemStat))
		{
			return (ModemStat & MS_CTS_ON) > 0;
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

BOOL Win32SerialPort::Get_DSR_State()
{
	if (m_PortHandle != INVALID_HANDLE_VALUE)
	{
		DWORD ModemStat;
		if (GetCommModemStatus(m_PortHandle, &ModemStat))
		{
			return (ModemStat & MS_DSR_ON) > 0;
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

BOOL Win32SerialPort::Get_RI_State()
{
	if (m_PortHandle != INVALID_HANDLE_VALUE)
	{
		DWORD ModemStat;
		if (GetCommModemStatus(m_PortHandle, &ModemStat))
		{
			return (ModemStat & MS_RING_ON) > 0;
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
