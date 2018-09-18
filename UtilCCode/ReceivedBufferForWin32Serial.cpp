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
//    Description: MotrolDecodeForWin32Serial implementation for Win32SerialPort comms. received buffer
//
//************************************************************************
#include "ReceivedBufferForWin32Serial.h"
#include "MACROS.H"
#include <string.h>
#include <Windows.h>

ReceivedBufferForWin32Serial::ReceivedBufferForWin32Serial()
{
	m_iSize = SPI_RX_BUF_SIZE - 1;
	m_pReceiveBuffer = new char[SPI_RX_BUF_SIZE];
	memset(m_pReceiveBuffer, '\0', SPI_RX_BUF_SIZE);
}

ReceivedBufferForWin32Serial::ReceivedBufferForWin32Serial(char * pNewBuffer, int iNewBufferSize) : ReceivedBufferForWin32Serial()
{
	if (pNewBuffer != NULL && (&(pNewBuffer[iNewBufferSize - 1]) != NULL))
	{
		m_iSize = MIN(iNewBufferSize, (SPI_RX_BUF_SIZE - 1));
		CopyMemory((LPVOID)m_pReceiveBuffer, pNewBuffer, m_iSize);

	}
}

ReceivedBufferForWin32Serial::ReceivedBufferForWin32Serial(const ReceivedBufferForWin32Serial & lhs) : ReceivedBufferForWin32Serial()
{
	// From this link: https://docs.microsoft.com/en-us/windows/desktop/api/winbase/nf-winbase-isbadreadptr
	//	if (!IsBadReadPtr((LPVOID)lhs.m_pReceiveBuffer, SPI_RX_BUF_SIZE))
	if (lhs.m_pReceiveBuffer != NULL && (&(lhs.m_pReceiveBuffer[SPI_RX_BUF_SIZE - 1]) != NULL))
	{
		CopyMemory((LPVOID)m_pReceiveBuffer, lhs.m_pReceiveBuffer, (SPI_RX_BUF_SIZE - 1));
		m_iSize = lhs.m_iSize;
	}
}

ReceivedBufferForWin32Serial & ReceivedBufferForWin32Serial::operator=(const ReceivedBufferForWin32Serial & lhs)
{
	// TODO: insert return statement here
	char* pcharaName = new char[SPI_RX_BUF_SIZE];
	memset(pcharaName, '\0', SPI_RX_BUF_SIZE);
	if (lhs.m_pReceiveBuffer != NULL && (&(lhs.m_pReceiveBuffer[lhs.m_iSize - 1]) != NULL))
	{
		CopyMemory((LPVOID)pcharaName, lhs.m_pReceiveBuffer, lhs.m_iSize);
		m_iSize = lhs.m_iSize;
	}

	delete[] m_pReceiveBuffer;
	m_pReceiveBuffer = pcharaName;
	return *this;
}

unsigned char ReceivedBufferForWin32Serial::chksum8(char* pbuffer, int ilength)
{
	unsigned char sum;       // nothing gained in using smaller types!
	for (sum = 0; ilength != 0; ilength--)  sum += *(pbuffer++);   // parenthesis not required!
	return sum;
}

ReceivedBufferForWin32Serial::~ReceivedBufferForWin32Serial()
{
	if (m_pReceiveBuffer != nullptr)
	{
		delete[] m_pReceiveBuffer;
		m_pReceiveBuffer = nullptr;
	}
}


