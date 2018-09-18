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
//    Description: MotrolAPIWrapper definition for Win32SerialPort comms. and new thread
//    to run the process of accessing unique data set and process it and put it 
//    to its block queue, the queue is blocked if it is empty. 
//
//************************************************************************
#pragma once
#include<set>
#include<string>
#include <windows.h>
#include "Const.h"
#include "TDequeConcurrent.h"
#include "ReceivedBufferForWin32Serial.h"
#include "Win32SerialPort.h"
#include "VMELibrary.h"
#define MOTROL_REG_CONTROL_MSG_MAX_LENGH		8 // Refer to PCL PD AM 0060 –03C.PDF doc Page 5, add 1 for null terminate.
class MotrolAPIWrapper
{
public:
	// Global Access wrapper
	static MotrolAPIWrapper& Instance();
	MotrolAPIWrapper();
	~MotrolAPIWrapper();
	int MotrolProcessTask();
	BOOL Enable( std::string strCommPortName);
	BOOL Disable( std::string strCommPortName );
	int AddMsgToSendSet(char* pMsg, int iMsgLength);
	int AddKeepAliveMsgToSendSet();
	int MapRxBuffer( PVOID pMappedRxBuffer, int iMappedRxBufferLength);
	// For testing, so make it public at this time.
	std::string m_strCommPortName;
	int m_eSW_PROTOCOLS;
	bool m_bEnableTimer = false;
protected:
	TDequeConcurrent<ReceivedBufferForWin32Serial> m_aBufferToSend;
	TDequeConcurrent<ReceivedBufferForWin32Serial> m_aMotrolDecodeForWin32Serials;
private:
	BOOL CleanUpTimer();
	BOOL SetupTimer( DWORD msToStart, DWORD msInterval );
	char m_baTmpBuffer[SPI_RX_BUF_SIZE] = {0};// 16 byte is max. to help debugging for now.
	// Refer to "OEM MOTOR DRIVE	COMMS INTERFACE SPECIFICATION" SA2000_OEM_Motor_Drive_Protocol_1-0.DOC
	// msg info:													SOH, MOVE-OR-DIAGNOSTIC, ...,			LAST BYTE IS CRC.
	char m_baKeepAliveBuffer[MOTROL_REG_CONTROL_MSG_MAX_LENGH] = { 0x01,(char)0xA3,0x10,0x20,0x1A,0x02,(char)0xCF,'\0'};
	char m_baTestMoveForwardBuffer[MOTROL_REG_CONTROL_MSG_MAX_LENGH] = { (char)0x01,(char)0xA5,(char)0x0A,0x02,(char)0x82,(char)0x00,(char)0x05, 0x38}; //{ 0x01,'2','1','8','0','5','\0' };
	char m_baTestMoveBackwardBuffer[MOTROL_REG_CONTROL_MSG_MAX_LENGH] = { (char)0x01,(char)0xA5,(char)0x0B,0x02,(char)0x82,(char)0x00,(char)0x05, 0x39 }; // { 0x01,'2','1','C','0','5','\0' };
	Win32SerialPort* m_pWin32SerialPortObject = nullptr;
	PTP_TIMER m_ptpTimer = NULL;
	bool m_bTerminate;
	// Flag: Has Dispose already been called?
	bool m_bdisposed = false;
	bool m_bSerialPortThreadStarted;
	bool m_bSwap = false;// for testing
	int m_iCountForKeepAlive = 0;
};

