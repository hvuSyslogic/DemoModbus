#pragma once
#ifndef CMBWFunc_H
#define CMBWFunc_H

#ifdef  __cplusplus
extern "C" {
#endif
	int WrModbusEnable(char* pstrIPAddress, unsigned int* ptrnHandle);
	void WrModbusDisable(int nHander);
	int WrModbusReadData(int nHandle, int nlength, int naddr, int ndCons, unsigned char* pDst);
	int WrModbusWriteData(int nHandle, int nlength, int naddr, int ndCons, unsigned char* pSrc);
	void  WrModbusGetErrorMessage(int ErrorCode, char* pDst, int nMaxLength);

#ifdef  __cplusplus
}
#endif

#endif // !CMBWFunc_H