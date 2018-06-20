#include "ManagedWrapper.h"

#include <msclr/marshal.h>
using namespace msclr::interop;

#include <strsafe.h>

using namespace System;
using namespace System::Runtime::InteropServices;
using namespace  PhoenixContact::DDI;


ManagedWrapper::ManagedWrapper()
{
	IBS_G4_Drv ^ obj = gcnew IBS_G4_Drv( "ManageWrapper");
}
