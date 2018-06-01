#include "stdafx.h"
#include "ModbusServices.h"
#include <sstream>

ModBusServices::ModBusServices()
{
	m_hResult = Initialize();
}


ModBusServices::~ModBusServices()
{
}

bool ModBusServices::Disable(int Handle, int * ErrorCode)
{
	int hr = 0, handle = 0;
	DWORD dwResult;
	std::wostringstream os;
	os << Handle;
	if (m_hResult == 0)
	{
		//IfFailRet(m_spRuntimeInfo->GetInterface(CLSID_CLRRuntimeHost, IID_PPV_ARGS(&m_spRuntimeHost)));
		//if (!m_bStarted)
		//{
		//	hr = m_spRuntimeHost->Start();
		//}
		hr = m_spRuntimeHost->ExecuteInDefaultAppDomain(
			m_strAssemblyName.c_str(),
			m_strTypeName.c_str(),
			L"Disable",
			os.str().c_str(),
			&dwResult);
		
		return dwResult == 0 ? true : false;
	}
	return false;
}

void ModBusServices::GetErrorMessage(int ErrorCode, char * ErrorMessage, int MessageSize)
{
	int hr = 0, handle = 0;
	std::string errorMsg = "DefaultMessage";
	//DWORD dwResult;
	if( ErrorMessage != NULL && MessageSize > (int)0 )
		strcpy_s(ErrorMessage, min(errorMsg.length(), MessageSize), errorMsg.c_str());
}


int ModBusServices::Enable( const WCHAR * args, int * Handle, LPDWORD pdwResult)
{
	int hr = 0, handle = 0;
	if(args == NULL)

	if (m_hResult == 0)
	{
		IfFailRet(m_spRuntimeInfo->GetInterface(CLSID_CLRRuntimeHost, IID_PPV_ARGS(&m_spRuntimeHost)));
		if (!m_bStarted)
		{
			hr = m_spRuntimeHost->Start();
		}
		hr = m_spRuntimeHost->ExecuteInDefaultAppDomain(
			m_strAssemblyName.c_str(),
			m_strTypeName.c_str(),
			L"Enable",
			m_strConnectionDTI.c_str(),
			pdwResult);
		handle = (int)*pdwResult;
	}

	return handle;
}



int ModBusServices::ReadMessage(int Handle, int length, int addr, int dCons, byte * msgBuf)
{
	if (m_spRuntimeHost.IsEqualObject(nullptr))
		return 0;
	DWORD dwResult;
	HRESULT hr = m_spRuntimeHost->ExecuteInDefaultAppDomain(
		m_strAssemblyName.c_str(),
		m_strTypeName.c_str(),
		L"ManagedMethodCalledFromExtension",
		L"1",
		&dwResult);
	return (int)dwResult;

}

int ModBusServices::WriteMessage(int Handle, int length, int addr, int dCons, byte * msgBuf)
{
	if (m_spRuntimeHost.IsEqualObject(nullptr))
		return 0;
	DWORD dwResult;
	HRESULT hr = m_spRuntimeHost->ExecuteInDefaultAppDomain(
		m_strAssemblyName.c_str(),
		m_strTypeName.c_str(),
		L"ManagedMethodCalledFromExtension",
		L"2",
		&dwResult);
	return (int)dwResult;
}

int ModBusServices::Initialize()
{
	CComPtr<ICLRMetaHost> spHost;
	int hr = CLRCreateInstance(CLSID_CLRMetaHost, IID_PPV_ARGS(&spHost));
	CComPtr<IEnumUnknown> pRunTimes;
	IfFailRet(spHost->EnumerateInstalledRuntimes(&pRunTimes));
	CComPtr<IUnknown> pUnkRuntime;
	while (S_OK == pRunTimes->Next(1, &pUnkRuntime, 0))
	{
		CComQIPtr<ICLRRuntimeInfo> pp(pUnkRuntime);
		if (pUnkRuntime != nullptr)
		{
			m_spRuntimeInfo = pp;
			break;
		}
	}
	IfNullFail(m_spRuntimeInfo);

	BOOL bStarted;
	DWORD dwStartupFlags;
	hr = m_spRuntimeInfo->IsStarted(&bStarted, &dwStartupFlags);
	if (hr != S_OK) // sometimes 0x80004001  not implemented  
	{
		m_spRuntimeInfo = nullptr;
		hr = spHost->GetRuntime(L"v4.0.30319", IID_PPV_ARGS(&m_spRuntimeInfo));
		bStarted = false;
	}

	IfFailRet(m_spRuntimeInfo->GetInterface(CLSID_CLRRuntimeHost, IID_PPV_ARGS(&m_spRuntimeHost)));
	if (!bStarted)
	{
		hr = m_spRuntimeHost->Start();
	}
	return hr;
}
