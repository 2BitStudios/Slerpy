#include "Interpolate.h"

#include "SystemDefines.h"

#ifdef _MANAGED

#define TRANSLATE_FUNCTION_NAME(name) Interpolate::name

#else //_MANAGED

#define TRANSLATE_FUNCTION_NAME(name) Interpolate_##name

#endif //_MANAGED

#ifdef _MANAGED
namespace Slerpy
{
#else //_MANAGED

#endif //_MANAGED

float TRANSLATE_FUNCTION_NAME(Standard)(INTERPOLATE_PARAMS_STANDARD)
{
    return MATH_LERP(from, to, weight);
}

float TRANSLATE_FUNCTION_NAME(Clamped)(INTERPOLATE_PARAMS_STANDARD)
{
    return MATH_LERP(from, to, MATH_CLAMP01(weight));
}

#ifdef _MANAGED
}
#else //_MANAGED

#endif //_MANAGED

#undef TRANSLATE_SYMBOL_NAME