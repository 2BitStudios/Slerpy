#include "Weight.h"

#include "SystemDefines.h"

#ifdef _MANAGED

#define TRANSLATE_SYMBOL_NAME(name) Weight::name

#else //_MANAGED

#define TRANSLATE_SYMBOL_NAME(name) name

#endif //_MANAGED

#ifdef _MANAGED
namespace Slerpy
{
#else //_MANAGED

#endif //_MANAGED

    float TRANSLATE_SYMBOL_NAME(WithWeightType)(WeightType type, WEIGHT_PARAMS_STANDARD)
    {
        switch (type)
        {
        case WeightType::Heavy:
            return TRANSLATE_SYMBOL_NAME(Heavy)(weight);
        case WeightType::Inverted:
            return TRANSLATE_SYMBOL_NAME(Inverted)(weight);
        case WeightType::Exaggerated:
            return TRANSLATE_SYMBOL_NAME(Exaggerated)(weight);
        default:
            return TRANSLATE_SYMBOL_NAME(Linear)(weight);
        }
    }

    float TRANSLATE_SYMBOL_NAME(Linear)(WEIGHT_PARAMS_STANDARD)
    {
        return weight;
    }

    float TRANSLATE_SYMBOL_NAME(Heavy)(WEIGHT_PARAMS_STANDARD)
    {
        return weight * MATH_ABS(weight);
    }

    float TRANSLATE_SYMBOL_NAME(Inverted)(WEIGHT_PARAMS_STANDARD)
    {
        return 1.0f - weight;
    }

    float TRANSLATE_SYMBOL_NAME(Exaggerated)(WEIGHT_PARAMS_STANDARD)
    {
        static float const HIGHPOINT = 0.9f;
        static float const HIGHWEIGHT = 1.125f;

        if (weight < HIGHPOINT)
        {
            weight = (weight / HIGHPOINT) * HIGHWEIGHT;
        }
        else if (weight <= 1.0f)
        {
            weight += (1.0f - weight) * HIGHWEIGHT;
        }

        return weight;
    }

#ifdef _MANAGED
}
#else //_MANAGED

#endif //_MANAGED

#undef TRANSLATE_SYMBOL_NAME