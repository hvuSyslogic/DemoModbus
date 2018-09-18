// mebForModbusApp.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <metahost.h>
#include <atlbase.h>
#include <atlcom.h>
#include <string>
#include "ModbusServices.h"
#include "Win32SerialPort.h"

// From this link https://blogs.msdn.microsoft.com/calvin_hsia/2015/02/27/call-c-code-from-your-legacy-c-code/
extern  "C" int __declspec(dllexport) CALLBACK CallClrMethod(
	const WCHAR *AssemblyName,
	const WCHAR *TypeName,
	const WCHAR *MethodName,
	const WCHAR *args,
	LPDWORD pdwResult
)
{
	int m_hr = S_OK, handler = 0;
	CComPtr<ICLRMetaHost> spHost;
	m_hr = CLRCreateInstance(CLSID_CLRMetaHost, IID_PPV_ARGS(&spHost));
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
	m_hr = spRuntimeInfo->IsStarted(&bStarted, &dwStartupFlags);
	if (m_hr != S_OK) // sometimes 0x80004001  not implemented  
	{
		spRuntimeInfo = nullptr;
		m_hr = spHost->GetRuntime(L"v4.0.30319", IID_PPV_ARGS(&spRuntimeInfo));

		bStarted = false;
	}

	CComPtr<ICLRRuntimeHost> spRuntimeHost;
	IfFailRet(spRuntimeInfo->GetInterface(CLSID_CLRRuntimeHost, IID_PPV_ARGS(&spRuntimeHost)));
	if (!bStarted)
	{
		m_hr = spRuntimeHost->Start();
	}
	m_hr = spRuntimeHost->ExecuteInDefaultAppDomain(
		AssemblyName,
		TypeName,
		MethodName,
		args,
		pdwResult);
	m_hr = spRuntimeHost->ExecuteInDefaultAppDomain(
		AssemblyName,
		TypeName,
		L"Enable",
		L"Ethernet",
		pdwResult);
	handler = (int)*pdwResult;
	Sleep(500);
	m_hr = spRuntimeHost->ExecuteInDefaultAppDomain(
		AssemblyName,
		TypeName,
		L"Disable",
		std::to_wstring(handler).c_str(),
		pdwResult);
	return m_hr;
}


int main()
{
	std::wstring strConnectionDTIMain = (L"IBETHIP[192.168.0.1]N1_D");
	int Handle = 0;
	Win32SerialPort* test = new Win32SerialPort();
	//test->Open()
	// For testing Phoenix driver
	//ModBusServices localModBusServices = ModBusServices::Instance();
	//DWORD dwResult;
	//byte data[8] =  { 0,16,0,0,0,0,0,0 };
	//byte ReadData[8] =  { 0,0,0,0,0,0,0,0 };
	////HRESULT hr = CallClrMethod(
	////	L"IBSG4_Driver_FX46.dll",  // name of DLL (can be fullpath)
	////	L"PhoenixContact.DDI.FlatAPIForDDI",  // name of managed type
	////	L"ManagedMethodCalledFromExtension", // name of static method
	////	L"some args",
	////	&dwResult);
	//ModBusServices::Instance().Enable(strConnectionDTIMain.c_str(), &dwResult);
	//Handle = (int)dwResult;
	//Sleep(50);
	//ModBusServices::Instance().WriteMessage(Handle, 8, 2, 1, data);
	//Sleep(10);
	//ModBusServices::Instance().ReadMessage(Handle, 8, 2, 1, ReadData);
	//Sleep(10);
	//ModBusServices::Instance().Disable(Handle, (int*)&dwResult);
	// for testing serial port.
	return 0;
}


