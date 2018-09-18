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
#pragma once
#include <Windows.h>
#include "MOTROCOM.H"
const int g_iabBufferLength = MOTROL_MSG_OVERHEAD + MAX_MOTROL_BLOCK + 2;

class Win32SerialPort
{
public:
	void Set_RTS_State(BOOL state);
	void Set_DTR_State(BOOL state);
	BOOL Get_RI_State();
	BOOL Get_DSR_State();
	BOOL Get_CTS_State();
	BOOL Get_CD_State();
	int SetRxBuffer(PVOID pMappedRxBuffer, int iMappedRxBufferLength);
	virtual DWORD Write(LPVOID Buffer, DWORD BufferSize);
	virtual DWORD Write(std::string bufferAsString);
	virtual DWORD Read(LPVOID Buffer, DWORD BufferSize);
	virtual BOOL IsOpen();
	virtual void Close();
	virtual BOOL OpenWithDefault();
	virtual BOOL OpenWithCommPort(std::string strCommPortName);
	virtual BOOL ResetTimeOutAndPurgeCommPort(LPCOMMTIMEOUTS pcommTimeOut, DWORD dwEventsMask);
	Win32SerialPort();
	virtual ~Win32SerialPort();
protected:
	BOOL Open(LPCTSTR PortName, DWORD BaudRate, BYTE ByteSize, BYTE Parity, BYTE StopBits, DWORD DesiredAccess = GENERIC_READ | GENERIC_WRITE);
	BOOL Get_ModemSignalActive_State(DWORD signalToGet);
	HANDLE m_PortHandle;
	BOOL m_bForAssert;
	PVOID m_pMappedRxBuffer;
	int m_iMappedRxBufferLength;
	BYTE m_abBuffer[g_iabBufferLength];
};

