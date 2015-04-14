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

    Transform TRANSLATE_FUNCTION_NAME(OffsetShake)(TRANSFORMATION_OFFSET_PARAMS_STANDARD, float xRate, float yRate, float zRate, float xStrength, float yStrength, float zStrength)
    {
        float const shakeCoef = MATH_PI * time;

        return Transform(
            Vector3D(
                xStrength * MATH_SIN(xRate * shakeCoef),
                yStrength * MATH_SIN(yRate * shakeCoef),
                zStrength * MATH_SIN(zRate * shakeCoef)),
            QUATERNION_DEFAULT,
            VECTOR3D_DEFAULT);
    }

    Transform TRANSLATE_FUNCTION_NAME(OffsetTwist)(TRANSFORMATION_OFFSET_PARAMS_STANDARD, float xRate, float yRate, float zRate, float xStrength, float yStrength, float zStrength)
    {
        float const twistCoef = MATH_PI * time;

        return Transform(
            VECTOR3D_DEFAULT,
            Quaternion::FromEuler(
                xStrength * MATH_SIN(xRate * twistCoef),
                yStrength * MATH_SIN(yRate * twistCoef),
                zStrength * MATH_SIN(zRate * twistCoef)),
            VECTOR3D_DEFAULT);
    }

    Transform TRANSLATE_FUNCTION_NAME(OffsetThrob)(TRANSFORMATION_OFFSET_PARAMS_STANDARD, float xRate, float yRate, float zRate, float xStrength, float yStrength, float zStrength)
    {
        float const throbCoef = MATH_PI * time;

        return Transform(
            VECTOR3D_DEFAULT,
            QUATERNION_DEFAULT,
            Vector3D(
                xStrength * MATH_SIN(xRate * throbCoef),
                yStrength * MATH_SIN(yRate * throbCoef),
                zStrength * MATH_SIN(zRate * throbCoef)));
    }

#ifdef _MANAGED
}
#else //_MANAGED

#endif //_MANAGED

#undef TRANSLATE_FUNCTION_NAME