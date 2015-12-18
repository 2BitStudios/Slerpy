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

#define TRANSLATE_FUNCTION_NAME(name) __stdcall Interpolate_##name

#endif //_MANAGED

#ifdef _MANAGED
namespace Slerpy
{
    public
#endif //_MANAGED

    enum class InterpolateType
    {
        Standard = 0,
        Clamped = 1
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

#define INTERPOLATE_PARAMS_STANDARD float from, float to, float weight

    DECLARATION_PREFIX float TRANSLATE_FUNCTION_NAME(LinearWithType)(InterpolateType type, INTERPOLATE_PARAMS_STANDARD);
    DECLARATION_PREFIX float TRANSLATE_FUNCTION_NAME(SphericalWithType)(InterpolateType type, INTERPOLATE_PARAMS_STANDARD, float angle);

    DECLARATION_PREFIX float TRANSLATE_FUNCTION_NAME(LinearStandard)(INTERPOLATE_PARAMS_STANDARD);
    DECLARATION_PREFIX float TRANSLATE_FUNCTION_NAME(LinearClamped)(INTERPOLATE_PARAMS_STANDARD);

    DECLARATION_PREFIX float TRANSLATE_FUNCTION_NAME(SphericalStandard)(INTERPOLATE_PARAMS_STANDARD, float angle);
    DECLARATION_PREFIX float TRANSLATE_FUNCTION_NAME(SphericalClamped)(INTERPOLATE_PARAMS_STANDARD, float angle);

#ifdef _MANAGED
};
    }
#else //_MANAGED
}
#endif //_MANAGED

#undef TRANSLATE_FUNCTION_NAME
#undef DECLARATION_PREFIX

#endif //__SLERPYCORE_INTERPOLATE__