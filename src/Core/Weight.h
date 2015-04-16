#ifndef __SLERPYCORE_WEIGHT__
#define __SLERPYCORE_WEIGHT__

#ifdef _MANAGED

#define DECLARATION_PREFIX static
#define TRANSLATE_FUNCTION_NAME(name) __clrcall name

#else //_MANAGED

#ifdef _WIN32

#ifdef _SLERPYCORE_EXPORT
#define DECLARATION_PREFIX __declspec(dllexport)
#else //SLERPYCORE_EXPORT
#define DECLARATION_PREFIX __declspec(dllimport)
#endif //SLERPYCORE_EXPORT

#else //_WIN32

#define DECLARATION_PREFIX 

#endif //_WIN32

#define TRANSLATE_FUNCTION_NAME(name) __stdcall name

#endif //_MANAGED

#ifdef _MANAGED
namespace Slerpy
{
    public
#endif //_MANAGED

    enum class WeightType
    {
        Linear = 0,
        Heavy = 1,
        Invert = 2
    };

#ifdef _MANAGED
}
#endif //_MANAGED

#ifdef _MANAGED
namespace Slerpy
{
    public ref class Weight
    {
    private :
        Weight() {};

    public:
#else //_MANAGED
extern "C"
{
#endif //_MANAGED

#define WEIGHT_PARAMS_STANDARD float weight

    DECLARATION_PREFIX float TRANSLATE_FUNCTION_NAME(WithWeightType)(WeightType type, WEIGHT_PARAMS_STANDARD);

    DECLARATION_PREFIX float TRANSLATE_FUNCTION_NAME(Linear)(WEIGHT_PARAMS_STANDARD);
    DECLARATION_PREFIX float TRANSLATE_FUNCTION_NAME(Heavy)(WEIGHT_PARAMS_STANDARD);
    DECLARATION_PREFIX float TRANSLATE_FUNCTION_NAME(Invert)(WEIGHT_PARAMS_STANDARD);

#ifdef _MANAGED
    };
}
#else //_MANAGED
}
#endif //_MANAGED

#undef TRANSLATE_FUNCTION_NAME
#undef DECLARATION_PREFIX

#endif //__SLERPYCORE_WEIGHT__