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
//    Creation Date: 08/05/2018
//    Description: wrapper C functions for implementation for MotrolAPIWrapper comms. 
//
//************************************************************************
#pragma once
#ifndef WIN32SERIALPORTWRAPPER_H
#define WIN32SERIALPORTWRAPPER_H
#ifdef  __cplusplus
extern "C" {
	// Enable Win32SerialPort with Comm Port Number
	// returning 0 for succeed otherwise can use that for ModbusGetErrorMessage
	int MotrolAPIEnable(const char* strCommPort);
	// Disable Modbus driver in  handler 
	void MotrolAPIDisable(const char* strCommPort);
	bool MapDataBuffers(PVOID ptxBuffer, int itxBufferLength, PVOID prxBuffer, int irxBufferLength);
	DWORD GetLastErrorMessage(char* pacMessage, int ilength);
	int GetCommPortName(char* pstrCommPortName, int iLength);
	int GetSW_PROTOCOLS();
}
#endif

#endif