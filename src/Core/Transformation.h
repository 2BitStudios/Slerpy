#ifndef __SLERPYCORE_TRANSFORMATION__
#define __SLERPYCORE_TRANSFORMATION__

#include "Types.h"

#ifdef _MANAGED

#define SYMBOL_PREFIX static
#define TRANSLATE_SYMBOL_NAME(name) __clrcall name

#else //_MANAGED

#ifdef _WIN32

#ifdef _SLERPYCORE_EXPORT
#define SYMBOL_PREFIX __declspec(dllexport)
#else //SLERPYCORE_EXPORT
#define SYMBOL_PREFIX __declspec(dllimport)
#endif //SLERPYCORE_EXPORT

#else //_WIN32

#define SYMBOL_PREFIX 

#endif //_WIN32

#define TRANSLATE_SYMBOL_NAME(name) __stdcall name

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

    //symbol declarations here

#ifdef _MANAGED
    };
}
#else //_MANAGED
}
#endif //_MANAGED

#undef TRANSLATE_SYMBOL_NAME

#undef SYMBOL_PREFIX

#endif //__SLERPYCORE_TRANSFORMATION__