#pragma once
#include "stdafx.h"
#include <metahost.h>
#include <atlbase.h>
#include <atlcom.h>
#include <string>
class ModBusServices
{
public:
	ModBusServices();
	~ModBusServices();
	int __declspec(dllexport) CALLBACK Enable(
		const WCHAR *args,
		int* Handle,
		LPDWORD pdwResult
	);
	bool __declspec(dllexport) CALLBACK Disable(int Handle, int* ErrorCode);
	void __declspec(dllexport) CALLBACK GetErrorMessage(int ErrorCode, char* ErrorMessage, int MessageSize);
	int __declspec(dllexport) CALLBACK ReadMessage(int Handle, int length, int addr, int dCons, byte* msgBuf);
	int __declspec(dllexport) CALLBACK WriteMessage(int Handle, int length, int addr, int dCons, byte* msgBuf);
protected:
	bool m_bStarted;
	DWORD m_dwStartupFlags;
	int m_hResult;
	std::wstring m_strAssemblyName = (L"IBSG4_Driver_FX46.dll");
	std::wstring m_strTypeName = (L"PhoenixContact.DDI.FlatAPIForDDI");
	std::wstring m_strConnectionDTI = (L"IBETHIP[192.168.0.1]N1_D");
	CComPtr<ICLRRuntimeHost> m_spRuntimeHost = nullptr;
	CComPtr<ICLRRuntimeInfo> m_spRuntimeInfo = nullptr;

	int Initialize();

};

