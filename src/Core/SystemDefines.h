#ifndef __SLERPYCORE_SYSTEMDEFINES__
#define __SLERPYCORE_SYSTEMDEFINES__

#ifdef _MANAGED

#define MATH_PI ((float)System::Math::PI)
#define MATH_SIN(value) ((float)System::Math::Sin(value))
#define MATH_COS(value) ((float)System::Math::Cos(value))
#define MATH_SQRT(value) ((float)System::Math::Sqrt(value))
#define MATH_ABS(value) ((float)System::Math::Abs(value))

#else //_MANAGED

#include <stdlib.h>
#include <math.h>

#define MATH_PI ((float)M_PI)
#define MATH_SIN(value) ((float)sin(value))
#define MATH_COS(value) ((float)cos(value))
#define MATH_SQRT(value) ((float)sqrt(value))
#define MATH_ABS(value) ((float)abs(value))

#endif //_MANAGED

#define MATH_LERP(value1, value2, weight) (value1 * (1.0f - weight) + value2 * weight)
#define MATH_CLAMP(value, min, max) (value < min ? min : (value > max) ? max : value)
#define MATH_CLAMP01(value) MATH_CLAMP(value, 0.0f, 1.0f)

#define MATH_DEG2RAD(value) (value * ((MATH_PI * 2.0f) / 360.0f))
#define MATH_RAD2DEG(value) (value * (360.0f / (MATH_PI * 2.0f)))

#endif //__SLERPYCORE_SYSTEMDEFINES__