#ifndef __SLERPYCORE_MATH__
#define __SLERPYCORE_MATH__

#ifdef _MANAGED

#define MATH_PI ((float)System::Math::PI)

#define MATH_SIN(value) ((float)System::Math::Sin(value))
#define MATH_ASIN(value) ((float)System::Math::Asin(value))

#define MATH_COS(value) ((float)System::Math::Cos(value))
#define MATH_ACOS(value) ((float)System::Math::Acos(value))

#define MATH_SQRT(value) ((float)System::Math::Sqrt(value))
#define MATH_ABS(value) ((float)System::Math::Abs(value))
#define MATH_FLOOR(value) ((float)System::Math::Floor(value))

#define MATH_LOG(value) ((float)System::Math::Log(value))
#define MATH_LOG2(value) ((float)System::Math::Log(value, 2.0f))
#define MATH_LOG10(value) ((float)System::Math::Log10(value))

#else //_MANAGED

#include <stdlib.h>
#include <math.h>

#define MATH_PI ((float)M_PI)

#define MATH_SIN(value) ((float)sin(value))
#define MATH_ASIN(value) ((float)asin(value))

#define MATH_COS(value) ((float)cos(value))
#define MATH_ACOS(value) ((float)acos(value))

#define MATH_SQRT(value) ((float)sqrt(value))
#define MATH_ABS(value) ((float)abs(value))
#define MATH_FLOOR(value) ((float)floor(value))

#define MATH_LOG(value) ((float)log(value))
#define MATH_LOG2(value) ((float)log2(value))
#define MATH_LOG10(value) ((float)log10(value))

#endif //_MANAGED

#define MATH_LERP(from, to, weight) Math_Lerp(from, to, weight)

#define MATH_CLAMP(value, min, max) Math_Clamp(value, min, max)
#define MATH_CLAMP01(value) MATH_CLAMP(value, 0.0f, 1.0f)

#define MATH_DEG2RAD(value) Math_Deg2Rad(value)
#define MATH_RAD2DEG(value) Math_Rad2Deg(value)

#define MATH_SIGN(value) Math_Sign(value)

#define MATH_FMOD(dividend, divisor) Math_Fmod(dividend, divisor)

inline float Math_Lerp(float from, float to, float weight)
{
    return (from + (to - from) * weight);
}

inline float Math_Clamp(float value, float min, float max)
{
    return (value < min ? min : (value > max) ? max : value);
}

inline float Math_Deg2Rad(float value)
{
    return (value * ((MATH_PI * 2.0f) / 360.0f));
}

inline float Math_Rad2Deg(float value)
{
    return (value * (360.0f / (MATH_PI * 2.0f)));
}

inline float Math_Sign(float value)
{
    return ((float)(value > 0.0f ? 1.0f : (value < 0.0f ? -1.0f : 0.0f)));
}

inline float Math_Fmod(float dividend, float divisor)
{
    return ((MATH_ABS(dividend) - (MATH_ABS(divisor) * (MATH_FLOOR(MATH_ABS(dividend) / MATH_ABS(divisor))))) * MATH_SIGN(dividend));
}

#endif //__SLERPYCORE_MATH__