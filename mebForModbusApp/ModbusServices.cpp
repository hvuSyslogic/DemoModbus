#include "stdafx.h"
#include "ModbusServices.h"
#include <sstream>

ModBusServices::ModBusServices()
{
	m_hr = 0;
	m_hr = Initialize();
	wprintf(L"In ModBusServices ctor with connection: %s -- w/hr 0x%08lx\n", m_strConnectionDTI, m_hr);
}


ModBusServices::~ModBusServices()
{
}

ModBusServices & ModBusServices::Instance()
{
	static ModBusServices instance;
	return instance;
}

bool ModBusServices::Disable(int Handle, int * ErrorCode)
{
	int handle = 0;
	DWORD dwResult;
	std::wostringstream os;
	os << Handle;
	if (SUCCEEDED(m_hr))
	{
		m_hr = m_spRuntimeHost->ExecuteInDefaultAppDomain(
			m_strAssemblyName.c_str(),
			m_strTypeName.c_str(),
			L"Disable",
			os.str().c_str(),
			&dwResult);

		return dwResult == 0 ? true : false;
	}
	wprintf(L"Failed to call Disable w/hr 0x%08lx\n", m_hr);

	return false;
}

void ModBusServices::GetErrorMessage(int ErrorCode, char * ErrorMessage, int MessageSize)
{
	int handle = 0;
	std::string errorMsg = "DefaultMessage";
	//DWORD dwResult;
	if (ErrorMessage != NULL && MessageSize > (int)0)
		strcpy_s(ErrorMessage, min(errorMsg.length(), MessageSize), errorMsg.c_str());
}


int ModBusServices::Enable(const WCHAR * args, LPDWORD pdwResult)
{

	if (pdwResult == NULL) pdwResult = new DWORD;
	if (args == NULL)
	{
		*pdwResult = 0;
		return -1;
	}
	if (SUCCEEDED(m_hr))
	{

		m_hr = m_spRuntimeHost->ExecuteInDefaultAppDomain(
			m_strAssemblyName.c_str(),
			m_strTypeName.c_str(),
			L"Enable",
			m_strConnectionDTI.c_str(),
			pdwResult);
		m_nHandle = (int)*pdwResult;
	}
	wprintf(L"In ModBusService Enable w/handle 0x%08lx\n", m_nHandle);

	return m_hr;
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
	wprintf(L"In ModBusService ReadMessage Handle: %d -Length: %d -addr: %d -dCons : %d w/hr 0x%08lx\n", Handle, length, addr, dCons, m_hr);
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
	wprintf(L"In ModBusService WriteMessage Handle: %d -Length: %d -addr: %d -dCons : %d w/hr 0x%08lx\n", Handle, length, addr, dCons, m_hr);
	return (int)dwResult;
}

int ModBusServices::Initialize()
{
	CComPtr<ICLRMetaHost> spHost;
	m_hr = CLRCreateInstance(CLSID_CLRMetaHost, IID_PPV_ARGS(&spHost));
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
	m_hr = m_spRuntimeInfo->IsStarted(&bStarted, &dwStartupFlags);
	if (m_hr != S_OK) // sometimes 0x80004001  not implemented  
	{
		m_spRuntimeInfo = nullptr;
		m_hr = spHost->GetRuntime(L"v4.0.30319", IID_PPV_ARGS(&m_spRuntimeInfo));
		bStarted = false;
	}

	IfFailRet(m_spRuntimeInfo->GetInterface(CLSID_CLRRuntimeHost, IID_PPV_ARGS(&m_spRuntimeHost)));
	if (!bStarted)
	{
		m_hr = m_spRuntimeHost->Start();
	}
	return m_hr;
}

