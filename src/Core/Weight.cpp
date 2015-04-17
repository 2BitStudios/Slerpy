#include "Weight.h"

#include "SystemDefines.h"

#ifdef _MANAGED

#define TRANSLATE_FUNCTION_NAME(name) Weight::name

#else //_MANAGED

#define TRANSLATE_FUNCTION_NAME(name) Weight_##name

#endif //_MANAGED

#ifdef _MANAGED
namespace Slerpy
{
#else //_MANAGED

#endif //_MANAGED

    float TRANSLATE_FUNCTION_NAME(FromTime)(TimeWrapType type, float timeCurrent, float timeMax)
    {
        switch (type)
        {
        case TimeWrapType::PingPong:
            {
                float weight = MATH_FMOD(timeCurrent, timeMax * 2.0f) / timeMax;

                return weight >= 1.0f ? 2.0f - weight : weight;
            }
        case TimeWrapType::Repeat:
            {
                float weight = timeCurrent / timeMax;

                return MATH_FMOD(weight, 1.0f);
            }
        case TimeWrapType::Cycle :
            {
                float weight = MATH_FMOD(timeCurrent, timeMax * 4.0f) / timeMax;

                if (weight >= 2.0f)
                {
                    return weight >= 3.0f ? weight - 4.0f: 2.0f - weight;
                }
                else
                {
                    return weight >= 1.0f ? 2.0f - weight : weight;
                }
            }
        case TimeWrapType::Clamp:
        default:
            {
                float weight = timeCurrent / timeMax;

                return MATH_CLAMP01(weight);
            }
        }
    }

    float TRANSLATE_FUNCTION_NAME(WithType)(WeightType type, WEIGHT_PARAMS_STANDARD)
    {
        switch (type)
        {
        case WeightType::Heavy:
            return TRANSLATE_FUNCTION_NAME(Heavy)(weight);
        case WeightType::Inverted:
            return TRANSLATE_FUNCTION_NAME(Inverted)(weight);
        case WeightType::Exaggerated:
            return TRANSLATE_FUNCTION_NAME(Exaggerated)(weight);
        case WeightType::StickyLow:
            return TRANSLATE_FUNCTION_NAME(StickyLow)(weight);
        case WeightType::StickyHigh:
            return TRANSLATE_FUNCTION_NAME(StickyHigh)(weight);
        case WeightType::Linear:
        default:
            return TRANSLATE_FUNCTION_NAME(Linear)(weight);
        }
    }

    float TRANSLATE_FUNCTION_NAME(Linear)(WEIGHT_PARAMS_STANDARD)
    {
        return weight;
    }

    float TRANSLATE_FUNCTION_NAME(Heavy)(WEIGHT_PARAMS_STANDARD)
    {
        return weight * MATH_ABS(weight);
    }

    float TRANSLATE_FUNCTION_NAME(Inverted)(WEIGHT_PARAMS_STANDARD)
    {
        return -weight;
    }

    float TRANSLATE_FUNCTION_NAME(Exaggerated)(WEIGHT_PARAMS_STANDARD)
    {
        static float const HIGHPOINT = 0.9f;
        static float const HIGHWEIGHT = HIGHPOINT / (HIGHPOINT - (1.0f - HIGHPOINT));

        float const weightAbs = MATH_ABS(weight);

        if (weightAbs < HIGHPOINT)
        {
            weight = (weight / HIGHPOINT) * HIGHWEIGHT;
        }
        else if (weightAbs <= 1.0f)
        {
            weight *= MATH_LERP(HIGHWEIGHT, 1.0f, (weightAbs - HIGHPOINT) / (1.0f - HIGHPOINT)) / weightAbs;
        }

        return weight;
    }

    float TRANSLATE_FUNCTION_NAME(StickyLow)(WEIGHT_PARAMS_STANDARD)
    {
        static float const THRESHOLD = 0.2f;

        float const weightAbs = MATH_ABS(weight);

        if (weightAbs < THRESHOLD)
        {
            weight = 0.0f;
        }
        else if (weightAbs <= 1.0f)
        {
            weight = ((weightAbs - THRESHOLD) * MATH_SIGN(weight)) / (1.0f - THRESHOLD);
        }

        return weight;
    }

    float TRANSLATE_FUNCTION_NAME(StickyHigh)(WEIGHT_PARAMS_STANDARD)
    {
        static float const THRESHOLD = 0.8f;

        float const weightAbs = MATH_ABS(weight);

        if (weightAbs < THRESHOLD)
        {
            weight = weight / THRESHOLD;
        }
        else
        {
            weight = 1.0f * MATH_SIGN(weight);
        }

        return weight;
    }

#ifdef _MANAGED
}
#else //_MANAGED

#endif //_MANAGED

#undef TRANSLATE_SYMBOL_NAME