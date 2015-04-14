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

    Transform TRANSLATE_FUNCTION_NAME(OffsetShake)(TRANSFORMATION_OFFSET_PARAMS_STANDARD, float xRate, float yRate, float zRate)
    {
        Transform returnValue = TRANSFORM_DEFAULT;

        float const shakeCoef = MATH_PI * time;

        returnValue.Position = returnValue.Position + Vector3D(
            MATH_SIN(xRate * shakeCoef),
            MATH_SIN(yRate * shakeCoef),
            MATH_SIN(zRate * shakeCoef));

        return returnValue;
    }

#ifdef _MANAGED
}
#else //_MANAGED

#endif //_MANAGED

#undef TRANSLATE_FUNCTION_NAME