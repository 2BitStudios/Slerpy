#include "Transformation.h"

#include "SystemDefines.h"

#ifdef _MANAGED

#define TRANSLATE_FUNCTION_NAME(name) Transformation::name

#else //_MANAGED

#define TRANSLATE_FUNCTION_NAME(name) name

#endif //_MANAGED

#ifdef _MANAGED
namespace Slerpy
{
#else //_MANAGED

#endif //_MANAGED

    Transform TRANSLATE_FUNCTION_NAME(LerpShake)(TRANSFORMATION_PARAMS_STANDARD, float xRate, float yRate, float zRate, float strength)
    {
        Transform returnValue = Transform::Lerp(from, to, weight);

        float const shakeCoef = MATH_PI * weight;

        returnValue.Position = returnValue.Position + Vector3D(
            strength * MATH_SIN(xRate * shakeCoef),
            strength * MATH_SIN(yRate * shakeCoef),
            strength * MATH_SIN(zRate * shakeCoef));

        return returnValue;
    }

#ifdef _MANAGED
}
#else //_MANAGED

#endif //_MANAGED

#undef TRANSLATE_FUNCTION_NAME