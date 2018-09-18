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
#include "MotrolDecodeForWin32Serial.h"
#include "MACROS.H"
#include <string.h>
#include <Windows.h>

MotrolDecodeForWin32Serial::MotrolDecodeForWin32Serial()
{
	m_pReceiveBuffer = new char[SPI_RX_BUF_SIZE];
	memset(m_pReceiveBuffer, '\0', SPI_RX_BUF_SIZE);
}

MotrolDecodeForWin32Serial::MotrolDecodeForWin32Serial(char * pNewBuffer, int iNewBufferSize)
{
	m_pReceiveBuffer = new char[SPI_RX_BUF_SIZE];
	memset(m_pReceiveBuffer, '\0', SPI_RX_BUF_SIZE);
	if (pNewBuffer != NULL && (&(pNewBuffer[iNewBufferSize - 1]) != NULL))
	{
		CopyMemory((LPVOID)m_pReceiveBuffer, pNewBuffer, MIN(iNewBufferSize, (SPI_RX_BUF_SIZE - 1)));
	}
}

MotrolDecodeForWin32Serial::MotrolDecodeForWin32Serial(const MotrolDecodeForWin32Serial & lhs)
{
	m_pReceiveBuffer = new char[SPI_RX_BUF_SIZE];
	memset(m_pReceiveBuffer, '\0', SPI_RX_BUF_SIZE);
	if (lhs.m_pReceiveBuffer != NULL && (&(lhs.m_pReceiveBuffer[SPI_RX_BUF_SIZE - 1]) != NULL))
	{
		CopyMemory((LPVOID)m_pReceiveBuffer, lhs.m_pReceiveBuffer, (SPI_RX_BUF_SIZE - 1));
	}
}

MotrolDecodeForWin32Serial & MotrolDecodeForWin32Serial::operator=(const MotrolDecodeForWin32Serial & lhs)
{
	// TODO: insert return statement here
	char* pcharaName = new char[SPI_RX_BUF_SIZE];
	memset(pcharaName, '\0', SPI_RX_BUF_SIZE);
	if (lhs.m_pReceiveBuffer != NULL && (&(lhs.m_pReceiveBuffer[SPI_RX_BUF_SIZE - 1]) != NULL))
	{
		CopyMemory((LPVOID)pcharaName, lhs.m_pReceiveBuffer, (SPI_RX_BUF_SIZE - 1));
	}

	delete[] m_pReceiveBuffer;
	m_pReceiveBuffer = pcharaName;
	return *this;
}
MotrolDecodeForWin32Serial::~MotrolDecodeForWin32Serial()
{
	if (m_pReceiveBuffer != nullptr)
	{
		delete[] m_pReceiveBuffer;
		m_pReceiveBuffer = nullptr;
	}
}