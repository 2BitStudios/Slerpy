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

#define TRANSLATE_FUNCTION_NAME(name) __stdcall Weight_##name

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
        Inverted = 2,
        Exaggerated = 3,
    };

#ifdef _MANAGED
}
#endif //_MANAGED

#ifdef _MANAGED
namespace Slerpy
{
    public
#endif //_MANAGED

    enum class TimeWrapType
    {
        Clamp = 0,
        PingPong = 1,
        Repeat = 2
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

    DECLARATION_PREFIX float TRANSLATE_FUNCTION_NAME(FromTime)(TimeWrapType type, float timeCurrent, float timeMax);

#define WEIGHT_PARAMS_STANDARD float weight

    DECLARATION_PREFIX float TRANSLATE_FUNCTION_NAME(WithType)(WeightType type, WEIGHT_PARAMS_STANDARD);

    DECLARATION_PREFIX float TRANSLATE_FUNCTION_NAME(Linear)(WEIGHT_PARAMS_STANDARD);
    DECLARATION_PREFIX float TRANSLATE_FUNCTION_NAME(Heavy)(WEIGHT_PARAMS_STANDARD);
    DECLARATION_PREFIX float TRANSLATE_FUNCTION_NAME(Inverted)(WEIGHT_PARAMS_STANDARD);
    DECLARATION_PREFIX float TRANSLATE_FUNCTION_NAME(Exaggerated)(WEIGHT_PARAMS_STANDARD);

#ifdef _MANAGED
    };
}
#else //_MANAGED
}
#endif //_MANAGED

#undef TRANSLATE_FUNCTION_NAME
#undef DECLARATION_PREFIX

#endif //__SLERPYCORE_WEIGHT__