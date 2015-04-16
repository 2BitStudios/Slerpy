#include "Interpolate.h"

#include "SystemDefines.h"

#ifdef _MANAGED

#define TRANSLATE_SYMBOL_NAME(name) Interpolate::name

#else //_MANAGED

#define TRANSLATE_SYMBOL_NAME(name) name

#endif //_MANAGED

#ifdef _MANAGED
namespace Slerpy
{
#else //_MANAGED

#endif //_MANAGED

    float TRANSLATE_SYMBOL_NAME(WithInterpolateType)(InterpolateType type, INTERPOLATE_PARAMS_STANDARD)
    {
        switch (type)
        {
        case InterpolateType::PingPong:
            return TRANSLATE_SYMBOL_NAME(PingPong)(timeCurrent, timeMax);
        case InterpolateType::Repeat:
            return TRANSLATE_SYMBOL_NAME(Repeat)(timeCurrent, timeMax);
        default:
            return TRANSLATE_SYMBOL_NAME(Clamp)(timeCurrent, timeMax);
        }
    }

    float TRANSLATE_SYMBOL_NAME(Clamp)(INTERPOLATE_PARAMS_STANDARD)
    {
        float weight = timeCurrent / timeMax;

        return MATH_CLAMP01(weight);
    }

    float TRANSLATE_SYMBOL_NAME(PingPong)(INTERPOLATE_PARAMS_STANDARD)
    {
        float weight = MATH_FMOD(timeCurrent, timeMax * 2.0f) / timeMax;

        return weight >= 1.0f ? 2.0f - weight : weight;
    }

    float TRANSLATE_SYMBOL_NAME(Repeat)(INTERPOLATE_PARAMS_STANDARD)
    {
        float weight = timeCurrent / timeMax;

        return MATH_FMOD(weight, 1.0f);
    }

#ifdef _MANAGED
}
#else //_MANAGED

#endif //_MANAGED

#undef TRANSLATE_SYMBOL_NAME