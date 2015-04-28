#ifndef __SLERPYCORE_SYSTEMDEFINES__
#define __SLERPYCORE_SYSTEMDEFINES__

#ifdef _MANAGED

#define MATH_PI ((float)System::Math::PI)
#define MATH_SIN(value) ((float)System::Math::Sin(value))
#define MATH_COS(value) ((float)System::Math::Cos(value))
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
#define MATH_COS(value) ((float)cos(value))
#define MATH_SQRT(value) ((float)sqrt(value))
#define MATH_ABS(value) ((float)abs(value))
#define MATH_FLOOR(value) ((float)floor(value))
#define MATH_LOG(value) ((float)log(value))
#define MATH_LOG2(value) ((float)log2(value))
#define MATH_LOG10(value) ((float)log10(value))

#endif //_MANAGED

#define MATH_LERP(from, to, weight) (from + (to - from) * weight)
#define MATH_CLAMP(value, min, max) (value < min ? min : (value > max) ? max : value)
#define MATH_CLAMP01(value) MATH_CLAMP(value, 0.0f, 1.0f)

#define MATH_DEG2RAD(value) (value * ((MATH_PI * 2.0f) / 360.0f))
#define MATH_RAD2DEG(value) (value * (360.0f / (MATH_PI * 2.0f)))

#define MATH_SIGN(value) ((float)(value > 0.0f ? 1.0f : (value < 0.0f ? -1.0f : 0.0f)))

#define MATH_FMOD(dividend, divisor) (MATH_ABS(dividend) - (MATH_ABS(divisor) * (MATH_FLOOR(MATH_ABS(dividend) / MATH_ABS(divisor))))) * MATH_SIGN(dividend)

#endif //__SLERPYCORE_SYSTEMDEFINES__