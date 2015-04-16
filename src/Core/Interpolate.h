#ifndef __SLERPYCORE_INTERPOLATE__
#define __SLERPYCORE_INTERPOLATE__

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

    enum class InterpolateType
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
    public ref class Interpolate
    {
    private:
        Interpolate() {};

    public:
#else //_MANAGED
extern "C"
{
#endif //_MANAGED

#define INTERPOLATE_PARAMS_STANDARD float timeCurrent, float timeMax

    DECLARATION_PREFIX float TRANSLATE_FUNCTION_NAME(WithInterpolateType)(InterpolateType type, INTERPOLATE_PARAMS_STANDARD);

    DECLARATION_PREFIX float TRANSLATE_FUNCTION_NAME(Clamp)(INTERPOLATE_PARAMS_STANDARD);
    DECLARATION_PREFIX float TRANSLATE_FUNCTION_NAME(PingPong)(INTERPOLATE_PARAMS_STANDARD);
    DECLARATION_PREFIX float TRANSLATE_FUNCTION_NAME(Repeat)(INTERPOLATE_PARAMS_STANDARD);

#ifdef _MANAGED
    };
}
#else //_MANAGED
}
#endif //_MANAGED

#undef TRANSLATE_FUNCTION_NAME
#undef DECLARATION_PREFIX

#endif //__SLERPYCORE_INTERPOLATE__