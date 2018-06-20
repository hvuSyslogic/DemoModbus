#pragma once
#include "stdafx.h"
#include <metahost.h>
#include <atlbase.h>
#include <atlcom.h>
#include <string>
class ModBusServices
{
public:
	~ModBusServices();
	static ModBusServices& Instance();
	int  CALLBACK Enable(
		const WCHAR *pstrConnectionName,
		LPDWORD pdwResult
	);
	bool Disable(int Handle, int* ErrorCode);
	void   GetErrorMessage(int ErrorCode, char* ErrorMessage, int MessageSize);
	int  ReadMessage(int Handle, int length, int addr, int dCons, byte* msgBuf);
	int  WriteMessage(int Handle, int length, int addr, int dCons, byte* msgBuf);
protected:
	bool m_bStarted;
	DWORD m_dwStartupFlags;
	int m_nHandle, m_hr;
	std::wstring m_strAssemblyName = (L"IBSG4_Driver_FX46.dll");
	std::wstring m_strTypeName = (L"PhoenixContact.DDI.FlatAPIForDDI");
	std::wstring m_strConnectionDTI = (L"IBETHIP[192.168.0.1]N1_D");
	CComPtr<ICLRRuntimeHost> m_spRuntimeHost = nullptr;
	CComPtr<ICLRRuntimeInfo> m_spRuntimeInfo = nullptr;
	ModBusServices();
	int Initialize();

};

