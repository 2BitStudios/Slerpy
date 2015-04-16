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
        case WeightType::Invert:
            return TRANSLATE_SYMBOL_NAME(Invert)(weight);
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
        return weight * weight;
    }

    float TRANSLATE_SYMBOL_NAME(Invert)(WEIGHT_PARAMS_STANDARD)
    {
        return 1.0f - weight;
    }

#ifdef _MANAGED
}
#else //_MANAGED

#endif //_MANAGED

#undef TRANSLATE_SYMBOL_NAME