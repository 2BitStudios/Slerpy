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

    float TRANSLATE_FUNCTION_NAME(LinearWithType)(InterpolateType type, INTERPOLATE_PARAMS_STANDARD)
    {
        switch (type)
        {
        case InterpolateType::Clamped:
            return TRANSLATE_FUNCTION_NAME(LinearClamped)(from, to, weight);
        case InterpolateType::Standard:
        default:
            return TRANSLATE_FUNCTION_NAME(LinearStandard)(from, to, weight);
        }
    }

    float TRANSLATE_FUNCTION_NAME(SphericalWithType)(InterpolateType type, INTERPOLATE_PARAMS_STANDARD, float angle)
    {
        switch (type)
        {
        case InterpolateType::Clamped:
            return TRANSLATE_FUNCTION_NAME(SphericalClamped)(from, to, weight, angle);
        case InterpolateType::Standard:
        default:
            return TRANSLATE_FUNCTION_NAME(SphericalStandard)(from, to, weight, angle);
        }
    }

    float TRANSLATE_FUNCTION_NAME(LinearStandard)(INTERPOLATE_PARAMS_STANDARD)
    {
        return MATH_LERP(from, to, weight);
    }

    float TRANSLATE_FUNCTION_NAME(LinearClamped)(INTERPOLATE_PARAMS_STANDARD)
    {
        return TRANSLATE_FUNCTION_NAME(LinearStandard)(from, to, MATH_CLAMP(weight, -1.0f, 1.0f));
    }

    float TRANSLATE_FUNCTION_NAME(SphericalStandard)(INTERPOLATE_PARAMS_STANDARD, float angle)
    {
        float const sinAngle = MATH_SIN(angle);

        return MATH_SIN((1.0f - weight) * angle) / sinAngle * from + MATH_SIN(weight * angle) / sinAngle * to;
    }

    float TRANSLATE_FUNCTION_NAME(SphericalClamped)(INTERPOLATE_PARAMS_STANDARD, float angle)
    {
        return TRANSLATE_FUNCTION_NAME(SphericalStandard)(from, to, MATH_CLAMP(weight, -1.0f, 1.0f), angle);
    }

#ifdef _MANAGED
}
#else //_MANAGED

#endif //_MANAGED

#undef TRANSLATE_FUNCTION_NAME