//************************************************************************
//
//        This work contains valuable confidential and proprietary
//    information.  Disclosure, use or reproduction without the
//    written authorization of the company is prohibited.  This
//    unpublished work by the company is protected by the laws of the
//    United States and other countries.  If publication of the work
//    should occur the following notice shall apply:
//    Copyright Baldwin Technology Company, Inc. 1998/2018 All Rights Reserved
//
//    Creation Date: 06/18/2018
//    Description: Memory shared between C# and C/C++ dlls, 
//    class C++ declaration and C functions
//
//************************************************************************

#pragma once

#include <Windows.h>
#include <mutex>

#ifdef SHAREDMEM_EXPORTS
#define SHAREDMEM_API __declspec(dllexport)
#else
#define SHAREDMEM_API __declspec(dllimport)
#endif

#define SHAREDMEM_CALLING_CONV __cdecl
// C# or Managed call and set shared Memory.
extern "C" {
	SHAREDMEM_API BOOL SHAREDMEM_CALLING_CONV SetSharedMem(int _32bitValue);
	SHAREDMEM_API BOOL SHAREDMEM_CALLING_CONV GetSharedMem(int* p32bitValue);
	SHAREDMEM_API VOID SHAREDMEM_CALLING_CONV SetModbusInfo(char* sipAddress, int iportNumber);
	SHAREDMEM_API VOID SHAREDMEM_CALLING_CONV SetWin32SerialPortForMotrolInfo( char* commPortName, int iSerialPortProtocolType, int idebugEnableTimer);
}
class SharedMem
{
public:
	static SharedMem& Instance();
	~SharedMem();
	BOOL  SetSharedMemory( int Int32Value 	);
	BOOL  GetSharedMemory(int* pInt32Value);
	void  GetLastErrorMessage(char* ErrorMessage, int MessageSize);
protected:
	HANDLE      m_hMappedFileObject = NULL;  // handle to mapped file
	LPVOID      m_lpvSharedMem = NULL;       // pointer to shared memory
	BOOL       m_IsInitialized = FALSE;

	// size of Memory to share, try 32 bytes this time. The first byte in the 
	// shared memory is the length in byte of avaiable memory.
	const int   M_SHARED_MEM_SIZE = 32;
	// thread safe reinforce
	std::mutex m_sharedMemMutex;
	SharedMem();
	BOOL Initialize();
};

