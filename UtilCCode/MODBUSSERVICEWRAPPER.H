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
//    Creation Date: 06/18/2018
//    Description: Phoenix Contact  IL ETH BK DI8 DO4 2TX-PAC driver wrapper C functions
//
//************************************************************************
#pragma once
#ifndef MODBUS_SERVICEWRAPPER_H
#define MODBUS_SERVICEWRAPPER_H
// testing data for shared memory
extern unsigned char* g_pabyteSharedModbusIOMemory;

#ifdef  __cplusplus
extern "C" {
#endif
	// Enable Modbus driver in with IP address , out handler to use later
	// returning 0 for succeed otherwise can use that for ModbusGetErrorMessage
	int ModbusEnable(const char* pstrIPAddress, BOOL useModBus, unsigned int* ptrnHandle);
	// Disable Modbus driver in  handler 
	void ModbusDisable(int nHander);
	// read Input Modbus Map 
	// returning 0 for succeed otherwise can use that for ModbusGetErrorMessage
	int IOInterbusReadData(int nHandle, int nlength, int naddr, int ndCons, unsigned char* pDst);
	// write output Modbus Map 
	int IOInterbusWriteData(int nHandle, int nlength, int naddr, int ndCons, unsigned char* pDst);
	// returning 0 for succeed otherwise can use that for ModbusGetErrorMessage
	int InitialiseIOWithManagedModbusAPI();
	// returning 0 for succeed otherwise can use that for ModbusGetErrorMessage
	int PerformModbusOperation(int modbusFunctionCode, int modbusStartingAddress, int quantity);
	// GetErrorMessage
	void  ModbusGetErrorMessage(int ErrorCode, char* pDst, int nMaxLength);
	int GetModbusIPAddress( char* pstrIPAddress, int iLength);
	BOOL InitModbusIOMemoryEntryPoint( void );

#ifdef  __cplusplus
}
#endif

#endif // !MODBUS_SERVICEWRAPPER_H

