#ifndef __SLERPYCORE_TRANSFORMATION__
#define __SLERPYCORE_TRANSFORMATION__

#include "Types.h"

#ifdef _MANAGED

#define DECLARATION_PREFIX static
#define TRANSLATE_FUNCTION_NAME(name) __clrcall name

#else //_MANAGED

#ifdef _WIN32

#ifdef _SLERPYCORE_EXPORT
#define DECLARATION_PREFIX __declspec(dllexport)
#else //SLERPYCORE_EXPORT
#define DECLARATION_PREFIX __declspec(dllimport)
#endif //SLERPYCORE_EXPORT

#else //_WIN32

#define DECLARATION_PREFIX 

#endif //_WIN32

#define TRANSLATE_FUNCTION_NAME(name) __stdcall name

#endif //_MANAGED

#ifdef _MANAGED
namespace Slerpy
{
    public ref class Transformation
    {
    public:
#else //_MANAGED
extern "C"
{
#endif //_MANAGED

#define TRANSFORMATION_PARAMS_STANDARD Transform from, Transform to, float weight

    DECLARATION_PREFIX Transform TRANSLATE_FUNCTION_NAME(LerpShake)(TRANSFORMATION_PARAMS_STANDARD, float xRate, float yRate, float zRate, float strength);

#ifdef _MANAGED
    };
}
#else //_MANAGED
}
#endif //_MANAGED

#undef TRANSLATE_FUNCTION_NAME
#undef DECLARATION_PREFIX

#endif //__SLERPYCORE_TRANSFORMATION__