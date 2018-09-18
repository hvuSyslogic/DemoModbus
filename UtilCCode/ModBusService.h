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
//    Description: Phoenix Contact  IL ETH BK DI8 DO4 2TX-PAC driver wrapper  C++ declaration
//
//************************************************************************
#ifndef MODBUSERVICE_H__
#define MODBUSERVICE_H__
#pragma once
#include "stdafx.h"
#include <metahost.h>
#include <atlbase.h>
#include <atlcom.h>
#include <string>

// for COM INTEROP 
#define IfFailRet(expr)                { m_ihr = (expr); if(FAILED(m_ihr)) return (m_ihr); }
#define IfNullFail(expr)                { if (!expr) return (E_FAIL); }

class ModBusService
{
public:
	// Global Access wrapper
	static ModBusService& Instance();
	~ModBusService();
	int  Enable(
		const WCHAR *args,
		LPDWORD pdwResult
	);
	bool  Disable(int Handle, int* ErrorCode);
	void  GetErrorMessage(int ErrorCode, char* ErrorMessage, int MessageSize);
	int  ReadMessage(int Handle, int length, int addr, int dCons, byte* msgBuf);
	int  WriteMessage(int Handle, int length, int addr, int dCons, byte* msgBuf);
	int  EasyModbusOp(const WCHAR * args);
	int EasyModbusSetupMappingIO(const WCHAR * args);
	void SetModbusInfo(const char* sIPAddress, int iPortNumber);
	BOOL InitMemoryEntryPoint(void);
	inline std::string GetIPAddress() { return m_strIPAddress; }
	inline int GetModbusPortNumber() { return m_iModbusPortNumber; }
protected:
	bool m_bStarted;
	DWORD m_dwStartupFlags;
	int m_iHandle, m_ihr, m_iModbusPortNumber;
	//hard code for testing
	std::wstring m_strAssemblyName = (L"ModbusAutotronAPI.dll");
	std::wstring m_strTypeName = (L"Qti.Autotron.ModbusAutotronAPI.FlatAPIForDDI");
	std::string m_strIPAddress = ("");
	
	// end hardcode.
	CComPtr<ICLRRuntimeHost> m_spRuntimeHost = nullptr;
	CComPtr<ICLRRuntimeInfo> m_spRuntimeInfo = nullptr;
	ModBusService();
	int Initialize();

};
#endif


