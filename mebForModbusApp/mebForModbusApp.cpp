// mebForModbusApp.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <metahost.h>
#include <atlbase.h>
#include <atlcom.h>
#include <string>


// From this link https://blogs.msdn.microsoft.com/calvin_hsia/2015/02/27/call-c-code-from-your-legacy-c-code/
extern  "C" int __declspec(dllexport) CALLBACK CallClrMethod(
	const WCHAR *AssemblyName,
	const WCHAR *TypeName,
	const WCHAR *MethodName,
	const WCHAR *args,
	LPDWORD pdwResult
)
{
	int hr = S_OK, handler = 0;
	CComPtr<ICLRMetaHost> spHost;
	hr = CLRCreateInstance(CLSID_CLRMetaHost, IID_PPV_ARGS(&spHost));
	CComPtr<ICLRRuntimeInfo> spRuntimeInfo;
	CComPtr<IEnumUnknown> pRunTimes;
	IfFailRet(spHost->EnumerateInstalledRuntimes(&pRunTimes));
	CComPtr<IUnknown> pUnkRuntime;
	while (S_OK == pRunTimes->Next(1, &pUnkRuntime, 0))
	{
		CComQIPtr<ICLRRuntimeInfo> pp(pUnkRuntime);
		if (pUnkRuntime != nullptr)
		{
			spRuntimeInfo = pp;
			break;
		}
	}
	IfNullFail(spRuntimeInfo);

	BOOL bStarted;
	DWORD dwStartupFlags;
	hr = spRuntimeInfo->IsStarted(&bStarted, &dwStartupFlags);
	if (hr != S_OK) // sometimes 0x80004001  not implemented  
	{
		spRuntimeInfo = nullptr;
		hr = spHost->GetRuntime(L"v4.0.30319", IID_PPV_ARGS(&spRuntimeInfo));

		bStarted = false;
	}

	CComPtr<ICLRRuntimeHost> spRuntimeHost;
	IfFailRet(spRuntimeInfo->GetInterface(CLSID_CLRRuntimeHost, IID_PPV_ARGS(&spRuntimeHost)));
	if (!bStarted)
	{
		hr = spRuntimeHost->Start();
	}
	hr = spRuntimeHost->ExecuteInDefaultAppDomain(
		AssemblyName,
		TypeName,
		MethodName,
		args,
		pdwResult);
	hr = spRuntimeHost->ExecuteInDefaultAppDomain(
		AssemblyName,
		TypeName,
		L"Enable",
		L"Ethernet",
		pdwResult);
	handler = (int)*pdwResult;
	Sleep(500);
	hr = spRuntimeHost->ExecuteInDefaultAppDomain(
		AssemblyName,
		TypeName,
		L"Disable",
		std::to_wstring(handler).c_str(),
		pdwResult);
	return hr;
}


int main()
{
	DWORD dwResult;
	HRESULT hr = CallClrMethod(
		L"IBSG4_Driver_FX46.dll",  // name of DLL (can be fullpath)
		L"PhoenixContact.DDI.FlatAPIForDDI",  // name of managed type
		L"ManagedMethodCalledFromExtension", // name of static method
		L"some args",
		&dwResult);
	return 0;
}


