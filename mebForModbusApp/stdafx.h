// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently, but
// are changed infrequently
//

#pragma once

#include "targetver.h"

#include <stdio.h>
#include <tchar.h>


// TODO: reference additional headers your program requires here
#pragma comment(lib, "mscoree.lib")

#define IfFailRet(expr)                { hr = (expr); if(FAILED(hr)) return (hr); }
#define IfNullFail(expr)                { if (!expr) return (E_FAIL); }
