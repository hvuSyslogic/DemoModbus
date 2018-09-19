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
//    Description: ReceivedBufferForWin32Serial definition for Win32SerialPort comms. received buffer
//
//************************************************************************
#pragma once
#include "Const.h"

class ReceivedBufferForWin32Serial
{
public:
	ReceivedBufferForWin32Serial();
	ReceivedBufferForWin32Serial(char* pNewBuffer, int iNewBufferSize);
	ReceivedBufferForWin32Serial(const ReceivedBufferForWin32Serial& lhs);
	ReceivedBufferForWin32Serial& operator=(const ReceivedBufferForWin32Serial& that);
	unsigned char chksum8(char* pbuffer, int ilength);
	virtual bool GetASCIIBuffer(char* pASCIIBuffer, int ilength);
	virtual ~ReceivedBufferForWin32Serial();
	virtual char* GetBuffer() { return m_pReceiveBuffer; }
	virtual int Size() { return m_iSize; }

private:
	char* m_pReceiveBuffer = nullptr;
	int m_iSize = 0;
};
inline bool operator< (const ReceivedBufferForWin32Serial& lhs, const ReceivedBufferForWin32Serial& rhs) { return 0; };