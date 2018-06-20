#include "ModbusWrapper.h"
#include "CMBWFunc.h"
#include <string>
#include <sstream>
#include <metahost.h>
#include <atlbase.h>
#include <atlcom.h>
#include <string>
int ModbusEnable(char* pstrIPAddress, unsigned int* ptrnHandle)
{
	DWORD result = 0;
	int returnValue = 0;
	unsigned int nHandle = 0;
	if (ptrnHandle == NULL) ptrnHandle = new unsigned int;
	if (pstrIPAddress == NULL)
	{
		if (ptrnHandle != NULL) *ptrnHandle = 0;
		return -1;
	}

	std::string tmp(pstrIPAddress);
	std::wstring tmpw(tmp.begin(), tmp.end());// This only works if all the characters are single byte
	std::wstring ModbusDriverName(L"IBETHIP[");
	ModbusDriverName.append(tmpw);
	ModbusDriverName.append(L"]N1_D");
	//returnValue = ModBusService::Instance().Enable(ModbusDriverName.c_str(), &result);
	nHandle = (unsigned int)result; // only good for 32 bit.
	*ptrnHandle = nHandle;
	return returnValue;

}
void ModbusDisable(int nHandle)
{
	int ErrorCode = -1;
	//bool returnValue = ModBusService::Instance().Disable(nHandle, &ErrorCode);
	return;
}
int ModbusReadData(int nHandle, int nlength, int naddr, int ndCons, unsigned char* pDst)
{
	//return ModBusService::Instance().ReadMessage(nHandle, nlength, naddr, ndCons, pDst);
	return 0;

}
int ModbusWriteData(int nHandle, int nlength, int naddr, int ndCons, unsigned char* pSrc)
{
	//return ModBusService::Instance().WriteMessage(nHandle, nlength, naddr, ndCons, pSrc);
	return 0;
}
void  ModbusGetErrorMessage(int ErrorCode, char* pDst, int nMaxLength)
{
	//return ModBusService::Instance().GetErrorMessage(ErrorCode, pDst, nMaxLength);
	return ;

}

ModbusWrapper::ModbusWrapper()
{
}


ModbusWrapper::~ModbusWrapper()
{
}
