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
//    Description: MotrolDecodeForWin32Serial definition for Win32SerialPort comms. received buffer
//
//************************************************************************
#pragma once
#include "Const.h"

class MotrolDecodeForWin32Serial
{
public:
	MotrolDecodeForWin32Serial();
	MotrolDecodeForWin32Serial(char* pNewBuffer, int iNewBufferSize);
	MotrolDecodeForWin32Serial(const MotrolDecodeForWin32Serial& lhs);
	MotrolDecodeForWin32Serial& operator=(const MotrolDecodeForWin32Serial& that);
	virtual ~MotrolDecodeForWin32Serial();


private:
	char* m_pReceiveBuffer = nullptr;
};

