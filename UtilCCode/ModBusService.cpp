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
//    Description: Phoenix Contact  IL ETH BK DI8 DO4 2TX-PAC driver wrapper implementation
//	    for both C functions & C++ declaration & EasyModbus dll client
//      from   <package id="EasyModbusTCP" version="5.0.0" targetFramework="net40" />
//
//************************************************************************
#include "ModbusService.h"
#include "MODBUSSERVICEWRAPPER.H"
#include "SharedMem.h"
#include <string>
#include <sstream>
unsigned char* g_pabyteSharedModbusIOMemory;
int ModbusEnable( const char* pstrIPAddress, BOOL useModbus, unsigned int* ptrnHandle)
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
	std::wstring ModbusDriverName(L"");
	if (useModbus)
	{
		ModbusDriverName.append(tmpw);
		ModbusDriverName.append(L"|502");

	}
	else
	{
		ModbusDriverName.append(L"IBETHIP[");
		ModbusDriverName.append(tmpw);
		ModbusDriverName.append(L"]N1_D");
	}
	returnValue = ModBusService::Instance().Enable(ModbusDriverName.c_str(), &result);
	nHandle = (unsigned int)result; // only good for 32 bit driver.
	*ptrnHandle = nHandle;
	return returnValue;
	
}
void ModbusDisable(int nHandle)
{
	int ErrorCode = -1;
	bool returnValue = ModBusService::Instance().Disable(nHandle, &ErrorCode);
	return;
}
int IOInterbusReadData(int nHandle, int nlength, int naddr, int ndCons, unsigned char* pDst)
{
	return ModBusService::Instance().ReadMessage(nHandle, nlength, naddr, ndCons, pDst);

}
int IOInterbusWriteData(int nHandle, int nlength, int naddr, int ndCons, unsigned char* pSrc)
{
	return ModBusService::Instance().WriteMessage(nHandle, nlength, naddr, ndCons, pSrc);
}
/// This method will set up the number of IOs and send out First Write to Digital Output
int InitialiseIOWithManagedModbusAPI()
{
	// strArg format: 
	// Args is constructed as follow:
	// "Y|XXXX|DATA".
	// where as: Y: 3 IS FC3, 4 is FC4, 6 is FC6... and  23 is FC23
	// XXXXX Is modbus register value or Starting Address of IO
	// Data is unsigned long (64BIT -Little Endiant format) for write if applicable.
	std::wstring strArg(L"6");
	strArg.append(L"|do-1");// or 8001 should work FIRST DIGITAL OUT PUT
	strArg.append(L"|7");
	// setup the IO, which will affect the shared memory read from.
	// the share memory first byte is the available Length, the second byte is modbus status
	// the following bytes is the value for ordered IO as setup here.
	std::wstring strArgsForIO(L"di|do|do");
	int returnedValue = ModBusService::Instance().EasyModbusSetupMappingIO(strArgsForIO.c_str());
	// then send out Write 7 (binary 0111 ) to First Digital Output-Base on Legacy code.
	return ModBusService::Instance().EasyModbusOp(strArg.c_str());
}
int PerformModbusOperation(int modbusFunctionCode, int modbusStartingAddress, int quantity)
{
	std::wostringstream osFC, osAddress, osQuantity;
	osFC << modbusFunctionCode;
	osAddress << modbusStartingAddress;
	osQuantity << quantity;
	std::wstring args(osFC.str());
	args.append(L"|");
	args.append(osAddress.str());
	args.append(L"|");
	args.append(osQuantity.str());
	return ModBusService::Instance().EasyModbusOp(args.c_str());


}
void  ModbusGetErrorMessage(int ErrorCode, char* pDst, int nMaxLength)
{
	return ModBusService::Instance().GetErrorMessage(ErrorCode, pDst, nMaxLength);

}

int GetModbusIPAddress(char* pstrIPAddress, int iLength)
{
	if (pstrIPAddress == nullptr || &(pstrIPAddress[iLength - 1]) == nullptr) return -1;
	memset(pstrIPAddress, '\0', iLength);
	ModBusService::Instance().GetIPAddress().copy(pstrIPAddress, iLength -1);
	return iLength;
}
BOOL InitModbusIOMemoryEntryPoint(void)
{
	return ModBusService::Instance().InitMemoryEntryPoint();
}

ModBusService::ModBusService()
{
	m_ihr = 0;
	m_iModbusPortNumber = 502;
	Initialize();
	wprintf(L"In ModBusService ctor w/hr 0x%08lx\n", m_ihr);
}


ModBusService::~ModBusService()
{
}

ModBusService & ModBusService::Instance()
{
	static ModBusService instance;
	return instance;
}

BOOL ModBusService::InitMemoryEntryPoint(void)
{
	DWORD dwResult;
	HRESULT hr = m_spRuntimeHost->ExecuteInDefaultAppDomain(
		m_strAssemblyName.c_str(),
		m_strTypeName.c_str(),
		L"EntryPoint",
		L"1",
		&dwResult);
	wprintf(L"In ModBusService EntryPoint Handle:  w/hr 0x%08lx\n", m_ihr);
	if (hr == S_OK && dwResult == 1)
	{
		int nSharedMemValue = 0;
		BOOL bGotValue = GetSharedMem(&nSharedMemValue);
		if (bGotValue)
		{
			g_pabyteSharedModbusIOMemory = (unsigned char*)nSharedMemValue;
			return true;
		}
		else 	return false;
	}
	else
	{
		return false;
	}
}

bool ModBusService::Disable(int Handle, int * ErrorCode)
{
	int handle = 0;
	DWORD dwResult;
	std::wostringstream os;
	os << Handle;
	if (SUCCEEDED( m_ihr))
	{
			m_ihr = m_spRuntimeHost->ExecuteInDefaultAppDomain(
			m_strAssemblyName.c_str(),
			m_strTypeName.c_str(),
			L"Disable",
			os.str().c_str(),
			&dwResult);

		return dwResult == 0 ? true : false;
	}
	wprintf(L"Failed to call Disable w/hr 0x%08lx\n", m_ihr);

	return false;
}

void ModBusService::SetModbusInfo(const char* sIPAddress, int iPortNumber)
{
	m_strIPAddress.empty();
	std::string tmp(sIPAddress);
	m_strIPAddress = tmp;
}


void ModBusService::GetErrorMessage(int ErrorCode, char * ErrorMessage, int MessageSize)
{
	int handle = 0;
	std::string errorMsg = "DefaultMessage";
	if (m_spRuntimeHost.IsEqualObject(nullptr))
		return ;
	if ( !errorMsg.empty() )
	{

		DWORD dwResult;
		HRESULT hr = m_spRuntimeHost->ExecuteInDefaultAppDomain(
			m_strAssemblyName.c_str(),
			m_strTypeName.c_str(),
			L"EntryPoint",
			L"1",
			&dwResult);
		wprintf(L"In ModBusService EntryPoint Handle:  w/hr 0x%08lx\n", m_ihr);
		if (hr == S_OK && dwResult == 1)
		{
			int nSharedMemValue = 0;
			BOOL bGotValue = GetSharedMem(&nSharedMemValue);
			if (bGotValue)
			{
				g_pabyteSharedModbusIOMemory = (unsigned char*)nSharedMemValue;
			}
		}
		return;
	}
	else
	{
		if (ErrorMessage != NULL && MessageSize > (int)0)
			strcpy_s(ErrorMessage, min(errorMsg.length(), MessageSize), errorMsg.c_str());
	}
}


int ModBusService::Enable(const WCHAR * args, LPDWORD pdwResult)
{
	
	if (pdwResult == NULL) pdwResult = new DWORD;
	if (args == NULL)
	{
		*pdwResult = 0;
		return -1;
	}
	if (SUCCEEDED(m_ihr))
	{

		m_ihr = m_spRuntimeHost->ExecuteInDefaultAppDomain(
			m_strAssemblyName.c_str(),
			m_strTypeName.c_str(),
			L"Enable",
			args,
			pdwResult);
		m_iHandle = (int)*pdwResult;
	}
	wprintf(L"In ModBusService Enable w/handle 0x%08lx\n", m_iHandle);

	return m_ihr;
}



int ModBusService::ReadMessage(int Handle, int length, int addr, int dCons, byte * msgBuf)
{
	if (m_spRuntimeHost.IsEqualObject(nullptr))
		return 0;
	DWORD dwResult;
	HRESULT hr = m_spRuntimeHost->ExecuteInDefaultAppDomain(
		m_strAssemblyName.c_str(),
		m_strTypeName.c_str(),
		L"ManagedPhoenixDN2DDIDriverWrapperOp",
		L"1",
		&dwResult);
	wprintf(L"In ModBusService ReadMessage Handle: %d -Length: %d -addr: %d -dCons : %d w/hr 0x%08lx\n", Handle, length, addr, dCons, m_ihr);
	return (int)dwResult;

}

int ModBusService::WriteMessage(int Handle, int length, int addr, int dCons, byte * msgBuf)
{
	if (m_spRuntimeHost.IsEqualObject(nullptr))
		return 0;
	DWORD dwResult;
	HRESULT hr = m_spRuntimeHost->ExecuteInDefaultAppDomain(
		m_strAssemblyName.c_str(),
		m_strTypeName.c_str(),
		L"ManagedPhoenixDN2DDIDriverWrapperOp",
		L"2",
		&dwResult);
	wprintf(L"In ModBusService WriteMessage Handle: %d -Length: %d -addr: %d -dCons : %d w/hr 0x%08lx\n", Handle, length, addr, dCons, m_ihr);
	return (int)dwResult;
}
int  ModBusService::EasyModbusOp(const WCHAR * args)
{
	if (m_spRuntimeHost.IsEqualObject(nullptr))
		return 0;
	DWORD dwResult;
	HRESULT hr = m_spRuntimeHost->ExecuteInDefaultAppDomain(
		m_strAssemblyName.c_str(),
		m_strTypeName.c_str(),
		L"ManagedModbusServiceWrapperOp",
		args,
		&dwResult);
	wprintf(L"In ModBusService EasyModbusOp:%s\n", args);
	return (int)dwResult;
}
int  ModBusService::EasyModbusSetupMappingIO(const WCHAR * args)
{
	if (m_spRuntimeHost.IsEqualObject(nullptr))
		return 0;
	DWORD dwResult;
	HRESULT hr = m_spRuntimeHost->ExecuteInDefaultAppDomain(
		m_strAssemblyName.c_str(),
		m_strTypeName.c_str(),
		L"ManagedModbusServiceWrapperMappingIO",
		args,
		&dwResult);
	wprintf(L"In ModBusService EasyModbusSetupMappingIO:%s\n", args);
	return (int)dwResult;
}
int ModBusService::Initialize()
{
	CComPtr<ICLRMetaHost> spHost;
	m_ihr = CLRCreateInstance(CLSID_CLRMetaHost, IID_PPV_ARGS(&spHost));
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
	m_ihr = m_spRuntimeInfo->IsStarted(&bStarted, &dwStartupFlags);
	if (m_ihr != S_OK) // sometimes 0x80004001  not implemented  
	{
		m_spRuntimeInfo = nullptr;
		m_ihr = spHost->GetRuntime(L"v4.0.30319", IID_PPV_ARGS(&m_spRuntimeInfo));
		bStarted = false;
	}

	IfFailRet(m_spRuntimeInfo->GetInterface(CLSID_CLRRuntimeHost, IID_PPV_ARGS(&m_spRuntimeHost)));
	if (!bStarted)
	{
		m_ihr = m_spRuntimeHost->Start();
	}
	return m_ihr;
}
