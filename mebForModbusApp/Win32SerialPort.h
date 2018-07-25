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
#ifndef WIN32SERIALPORT_H__
#define WIN32SERIALPORT_H__
#pragma once
#include "stdafx.h"

class Win32SerialPort
{
public:
	void Set_RTS_State(BOOL state);
	void Set_DTR_State(BOOL state);
	BOOL Get_RI_State();
	BOOL Get_DSR_State();
	BOOL Get_CTS_State();
	BOOL Get_CD_State();
	virtual DWORD Write(LPVOID Buffer, DWORD BufferSize);
	virtual DWORD Read(LPVOID Buffer, DWORD BufferSize);
	virtual BOOL IsOpen();
	virtual void Close();
	// Use PortName usually "COM1:" ... "COM4:" note that the name must end by ":"
	virtual BOOL OpenWithDefault();
	Win32SerialPort();
	virtual ~Win32SerialPort();
protected:
	BOOL Open(LPCTSTR PortName, DWORD BaudRate, BYTE ByteSize, BYTE Parity, BYTE StopBits, DWORD DesiredAccess = GENERIC_READ | GENERIC_WRITE);
	HANDLE m_PortHandle;
	BOOL m_bForAssert;
};
#endif

