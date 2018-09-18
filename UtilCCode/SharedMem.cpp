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
//    Description: Memory shared between C# and C/C++ dlls, 
//    class C++ definition
//
//************************************************************************
#include "stdafx.h"
#include "SharedMem.h"
#include "ModBusService.h"
#include "MotrolAPIWrapper.h"
VOID SetModbusInfo(char* sipAddress, int iportNumber)
{
	ModBusService::Instance().SetModbusInfo(sipAddress, iportNumber);
}
VOID SetWin32SerialPortForMotrolInfo(char* commPortName, int iSerialPortProtocolType, int idebugEnableTimer)
{
	MotrolAPIWrapper::Instance().m_strCommPortName = commPortName;
	MotrolAPIWrapper::Instance().m_eSW_PROTOCOLS = iSerialPortProtocolType;
	MotrolAPIWrapper::Instance().m_bEnableTimer = idebugEnableTimer != 0;
}

//https://stackoverflow.com/questions/9591579/clr-hosting-call-a-function-with-an-arbitrary-method-signature/9593846
BOOL SetSharedMem(int _32bitValue)
{
	return SharedMem::Instance().SetSharedMemory(_32bitValue);
}

  BOOL GetSharedMem(int* p32bitValue)
{
	if (p32bitValue == NULL) return FALSE;
	return SharedMem::Instance().GetSharedMemory(p32bitValue);
}
  SharedMem::SharedMem()
  {
	  m_IsInitialized = Initialize();
	  wprintf(L"In SharedMem ctor w/hr 0x%08lx\n", m_IsInitialized);
  }


  SharedMem::~SharedMem()
  {
	  if (m_lpvSharedMem != NULL)
	  {
		  UnmapViewOfFile(m_lpvSharedMem);
		  m_lpvSharedMem = NULL;
	  }
	  if (m_hMappedFileObject != NULL)
	  {
		  CloseHandle(m_hMappedFileObject);
		  m_hMappedFileObject = NULL;
	  }

  }

  SharedMem& SharedMem::Instance()
  {
	  static SharedMem instance;
	  return instance;
  }
  BOOL SharedMem::Initialize()
  {
	  // Create a named file mapping object
	  m_hMappedFileObject = CreateFileMapping(
		  INVALID_HANDLE_VALUE,
		  NULL,
		  PAGE_READWRITE,
		  0,
		  M_SHARED_MEM_SIZE,
		  TEXT("shmemfile") // Name of shared mem file
	  );

	  if (m_hMappedFileObject == NULL)
	  {
		  return FALSE;
	  }

	  BOOL bFirstInit = (ERROR_ALREADY_EXISTS != GetLastError());

	  // Get a ptr to the shared memory
	  m_lpvSharedMem = MapViewOfFile(m_hMappedFileObject, FILE_MAP_WRITE, 0, 0, 0);

	  if (m_lpvSharedMem == NULL)
	  {
		  CloseHandle(m_hMappedFileObject);
		  return FALSE;
	  }

	  if (bFirstInit) // First time the shared memory is accessed?
	  {
		  ZeroMemory(m_lpvSharedMem, M_SHARED_MEM_SIZE);
	  }

	  return TRUE;
  }
  BOOL  SharedMem::SetSharedMemory(int Int32Value)
  {
	  std::lock_guard<std::mutex> guard(m_sharedMemMutex);

	  if (m_IsInitialized && (m_lpvSharedMem != NULL) )
	  {
		  int* pSharedMem = (int*)m_lpvSharedMem;
		  *pSharedMem = Int32Value;
	  }

	  return m_IsInitialized;
  }
  BOOL  SharedMem::GetSharedMemory(int* pInt32Value)
  {
	  if (pInt32Value == NULL) return FALSE;
	  std::lock_guard<std::mutex> guard(m_sharedMemMutex);
	  //BOOL bOK = CreateSharedMem();

	  if (m_IsInitialized && (m_lpvSharedMem != NULL))
	  {
		  int* pSharedMem = (int*)m_lpvSharedMem;
		  *pInt32Value = *pSharedMem;
	  }

	  return m_IsInitialized;
  }
  void  SharedMem::GetLastErrorMessage(char* ErrorMessage, int MessageSize)
  {
	  return;
  }


