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
    
    float TRANSLATE_FUNCTION_NAME(FromValueInRange)(WrapType type, float currentValue, float minValue, float maxValue)
    {
        float const scaledCurrentValue = currentValue - minValue;
        float const scaledMaxValue = maxValue - minValue;
        
        switch (type)
        {
        case WrapType::PingPong:
            {
                float const weight = MATH_ABS(MATH_FMOD(scaledCurrentValue, scaledMaxValue * 2.0f) / scaledMaxValue);

                return weight >= 1.0f ? 2.0f - weight : weight;
            }
        case WrapType::Repeat:
            {
                float weight = scaledCurrentValue / scaledMaxValue;

                weight = MATH_FMOD(weight, 1.0f);
                
                if (weight < 0.0f)
                {
                    weight = 1.0f + weight;
                }
                
                return weight;
            }
        case WrapType::Cycle:
            {
                float const weight = MATH_FMOD(scaledCurrentValue, scaledMaxValue * 4.0f) / scaledMaxValue;
                float const weightAbs = MATH_ABS(weight);

                if (weightAbs >= 2.0f)
                {
                    return (weightAbs >= 3.0f ? weightAbs - 4.0f : 2.0f - weightAbs) * MATH_SIGN(weight);
                }
                else
                {
                    return (weightAbs >= 1.0f ? 2.0f - weightAbs : weightAbs) * MATH_SIGN(weight);
                }
            }
        case WrapType::MirrorClamp:
            {
                float weight = scaledCurrentValue / scaledMaxValue;

                weight = MATH_CLAMP(weight, 0.0f, 2.0f);

                if (weight > 1.0f)
                {
                    weight = 2.0f - weight;
                }

                return weight;
            }
        case WrapType::Clamp:
        default:
            {
                float const weight = scaledCurrentValue / scaledMaxValue;

                return MATH_CLAMP01(weight);
            }
        }
    }

    float TRANSLATE_FUNCTION_NAME(FromTime)(WrapType type, float currentTime, float maxTime)
    {
        return TRANSLATE_FUNCTION_NAME(FromValueInRange)(type, currentTime, 0.0f, maxTime);
    }

    float TRANSLATE_FUNCTION_NAME(FromAngle)(float currentAngleDegrees, float wrapAngleDegrees)
    {
        float const scaledWrapAngle = MATH_FMOD(wrapAngleDegrees, 360.0f);

        return TRANSLATE_FUNCTION_NAME(FromValueInRange)(WrapType::PingPong, currentAngleDegrees, scaledWrapAngle - 180.0f, scaledWrapAngle);
    }

    float TRANSLATE_FUNCTION_NAME(WithType)(WeightType type, WEIGHT_PARAMS_STANDARD)
    {
        switch (type)
        {
        case WeightType::Eager:
            return TRANSLATE_FUNCTION_NAME(Eager)(weight);
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
        case WeightType::Snap:
            return TRANSLATE_FUNCTION_NAME(Snap)(weight);
        case WeightType::Elastic:
            return TRANSLATE_FUNCTION_NAME(Elastic)(weight);
        case WeightType::Bounce:
            return TRANSLATE_FUNCTION_NAME(Bounce)(weight);
        case WeightType::Linear:
        default:
            return TRANSLATE_FUNCTION_NAME(Linear)(weight);
        }
    }

    float TRANSLATE_FUNCTION_NAME(Linear)(WEIGHT_PARAMS_STANDARD)
    {
        return weight;
    }

    float TRANSLATE_FUNCTION_NAME(Eager)(WEIGHT_PARAMS_STANDARD)
    {
        float const weightAbs = MATH_ABS(weight);
        
        return (MATH_LOG2(1.0f + weightAbs * 3.0f) * 0.5f) * MATH_SIGN(weight);
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

    float TRANSLATE_FUNCTION_NAME(Snap)(WEIGHT_PARAMS_STANDARD)
    {
        float const weightAbs = MATH_ABS(weight);

        return ((int)(weightAbs + 0.5f)) * MATH_SIGN(weight);
    }

    float TRANSLATE_FUNCTION_NAME(Elastic)(WEIGHT_PARAMS_STANDARD)
    {
        static float const THRESHOLD = 0.6f;
        static float const THRESHOLD_HEAD = 1.0f - THRESHOLD;

        static int const BOUNCES = 2;
        static float const BOUNCE_STRENGTH = 0.5f;

        float const weightAbs = MATH_ABS(weight);

        if (weightAbs < THRESHOLD)
        {
            weight = weight / THRESHOLD;
        }
        else if (weightAbs < 1.0f)
        {
            float const overflow = weightAbs - THRESHOLD;

            float const overflowStrength = (1.0f - overflow / THRESHOLD_HEAD);

            weight = 
                (1.0f
                    + THRESHOLD_HEAD / BOUNCES * (overflowStrength) * BOUNCE_STRENGTH
                    * MATH_SIN((overflow / THRESHOLD_HEAD) * (BOUNCES * MATH_PI)))
                * MATH_SIGN(weight);
        }

        return weight;
    }

    float TRANSLATE_FUNCTION_NAME(Bounce)(WEIGHT_PARAMS_STANDARD)
    {
        static float const THRESHOLD = 0.4f;
        static float const THRESHOLD_HEAD = 1.0f - THRESHOLD;

        static int const BOUNCES = 3;
        static float const BOUNCE_STRENGTH = 0.5f;

        float const weightAbs = MATH_ABS(weight);

        if (weightAbs < THRESHOLD)
        {
	        weight = weight / THRESHOLD;
        }
        else if (weightAbs < 1.0f)
        {
            float const overflow = weightAbs - THRESHOLD;

            float const bounceProgress = overflow / THRESHOLD_HEAD;

            float previousBounceInterval = 0.0f;

            for (int bounceNumber = 1; bounceNumber <= BOUNCES; ++bounceNumber)
            {
                float const bounceInterval = MATH_LOG2(1.0f + 1.0f / BOUNCES * bounceNumber * 3.0f) * 0.5f;

                if (bounceProgress <= bounceInterval)
                {
                    weight =
                        (1.0f
                            + THRESHOLD_HEAD / bounceNumber * BOUNCE_STRENGTH
                            * -MATH_SIN((bounceProgress - previousBounceInterval) / (bounceInterval - previousBounceInterval) * MATH_PI))
                        * MATH_SIGN(weight);

                    break;
                }

                previousBounceInterval = bounceInterval;
            }
        }

        return weight;
    }

#ifdef _MANAGED
}
#else //_MANAGED

#endif //_MANAGED

#undef TRANSLATE_FUNCTION_NAME