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

    Transform TRANSLATE_FUNCTION_NAME(OffsetShake)(TRANSFORMATION_OFFSET_PARAMS_STANDARD, AxisWeightings axisWeightings)
    {
        float const shakeCoef = MATH_PI * time;

        return Transform(
            Vector3D(
                axisWeightings.X.Strength * MATH_SIN(axisWeightings.X.Rate * shakeCoef),
                axisWeightings.Y.Strength * MATH_SIN(axisWeightings.Y.Rate * shakeCoef),
                axisWeightings.Z.Strength * MATH_SIN(axisWeightings.Z.Rate * shakeCoef)),
            QUATERNION_DEFAULT,
            VECTOR3D_DEFAULT);
    }

    Transform TRANSLATE_FUNCTION_NAME(OffsetTwist)(TRANSFORMATION_OFFSET_PARAMS_STANDARD, AxisWeightings axisWeightings)
    {
        float const twistCoef = MATH_PI * time;

        return Transform(
            VECTOR3D_DEFAULT,
            Quaternion::FromEuler(
                axisWeightings.X.Strength * MATH_SIN(axisWeightings.X.Rate * twistCoef),
                axisWeightings.Y.Strength * MATH_SIN(axisWeightings.Y.Rate * twistCoef),
                axisWeightings.Z.Strength * MATH_SIN(axisWeightings.Z.Rate * twistCoef)),
            VECTOR3D_DEFAULT);
    }

    Transform TRANSLATE_FUNCTION_NAME(OffsetThrob)(TRANSFORMATION_OFFSET_PARAMS_STANDARD, AxisWeightings axisWeightings)
    {
        float const throbCoef = MATH_PI * time;

        return Transform(
            VECTOR3D_DEFAULT,
            QUATERNION_DEFAULT,
            Vector3D(
                axisWeightings.X.Strength * MATH_SIN(axisWeightings.X.Rate * throbCoef),
                axisWeightings.Y.Strength * MATH_SIN(axisWeightings.Y.Rate * throbCoef),
                axisWeightings.Z.Strength * MATH_SIN(axisWeightings.Z.Rate * throbCoef)));
    }

#ifdef _MANAGED
}
#else //_MANAGED

#endif //_MANAGED

#undef TRANSLATE_FUNCTION_NAME