#include "Transformation.h"

#ifdef _MANAGED

#define TRANSLATE_SYMBOL_NAME(name) Transformation::name

#else //_MANAGED

#define TRANSLATE_SYMBOL_NAME(name) name

#endif //_MANAGED

#ifdef _MANAGED
namespace Slerpy
{
#else //_MANAGED

#endif //_MANAGED

    //symbol definitions here

#ifdef _MANAGED
}
#else //_MANAGED

#endif //_MANAGED

#undef TRANSLATE_SYMBOL_NAME