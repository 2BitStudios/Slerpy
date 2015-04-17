#ifndef __SLERPYCORE_LERP__
#define __SLERPYCORE_LERP__

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

#define TRANSLATE_FUNCTION_NAME(name) __stdcall Lerp_##name

#endif //_MANAGED

#ifdef _MANAGED
namespace Slerpy
{
    public ref class Lerp
    {
    private:
        Lerp() {};

    public:
#else //_MANAGED
extern "C"
{
#endif //_MANAGED

#define LERP_PARAMS_STANDARD float from, float to, float weight

    DECLARATION_PREFIX float TRANSLATE_FUNCTION_NAME(Standard)(LERP_PARAMS_STANDARD);
    DECLARATION_PREFIX float TRANSLATE_FUNCTION_NAME(Clamped)(LERP_PARAMS_STANDARD);

#ifdef _MANAGED
};
    }
#else //_MANAGED
}
#endif //_MANAGED

#undef TRANSLATE_FUNCTION_NAME
#undef DECLARATION_PREFIX

#endif //__SLERPYCORE_LERP__