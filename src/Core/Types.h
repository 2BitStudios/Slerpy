#ifndef __SLERPYCORE_TYPES__
#define __SLERPYCORE_TYPES__

#ifdef _MANAGED

#define TRANSLATE_TYPE_NAME(name) public value struct name
#define TRANSLATE_PROPERTY_NAME(type, name) property type name

#else //_MANAGED

#define TRANSLATE_TYPE_NAME(name) struct name
#define TRANSLATE_PROPERTY_NAME(type, name) type name

#endif //_MANAGED

#ifdef _MANAGED
namespace Slerpy
{
#else //_MANAGED
extern "C"
{
#endif //_MANAGED

#define VECTOR3D_DEFAULT Vector3D(0.0f, 0.0f, 0.0f)

    TRANSLATE_TYPE_NAME(Vector3D)
    {
    public:
        TRANSLATE_PROPERTY_NAME(float, X);
        TRANSLATE_PROPERTY_NAME(float, Y);
        TRANSLATE_PROPERTY_NAME(float, Z);

#ifndef _MANAGED
        Vector3D() {};
#endif //_MANAGED

        Vector3D(float x, float y, float z);

        static Vector3D Lerp(Vector3D from, Vector3D to, float weight);

#ifdef _MANAGED
        static Vector3D operator+(Vector3D lhs, Vector3D rhs);
        static Vector3D operator-(Vector3D lhs, Vector3D rhs);
#endif //_MANAGED
    };

#ifndef _MANAGED
    Vector3D operator+(Vector3D const& lhs, Vector3D const& rhs);
    Vector3D operator-(Vector3D const& lhs, Vector3D const& rhs);
#endif //_MANAGED

#define QUATERNION_DEFAULT Quaternion(0.0f, 0.0f, 0.0f, 1.0f)

    TRANSLATE_TYPE_NAME(Quaternion)
    {
    public:
        TRANSLATE_PROPERTY_NAME(float, X);
        TRANSLATE_PROPERTY_NAME(float, Y);
        TRANSLATE_PROPERTY_NAME(float, Z);
        TRANSLATE_PROPERTY_NAME(float, W);

#ifndef _MANAGED
        Quaternion() {};
#endif //_MANAGED

        Quaternion(float x, float y, float z, float w);

        static Quaternion Lerp(Quaternion from, Quaternion to, float weight);
        static Quaternion FromEuler(float x, float y, float z);

        void Normalize();

#ifdef _MANAGED
        static Quaternion operator*(Quaternion lhs, Quaternion rhs);
#endif //_MANAGED
    };

#ifndef _MANAGED
    Quaternion operator*(Quaternion const& lhs, Quaternion const& rhs);
#endif //_MANAGED

#define TRANSFORM_DEFAULT Transform(VECTOR3D_DEFAULT, QUATERNION_DEFAULT, VECTOR3D_DEFAULT)

    TRANSLATE_TYPE_NAME(Transform)
    {
    public:
        TRANSLATE_PROPERTY_NAME(Vector3D, Position);
        TRANSLATE_PROPERTY_NAME(Quaternion, Rotation);
        TRANSLATE_PROPERTY_NAME(Vector3D, Scale);

#ifndef _MANAGED
        Transform() {};
#endif //_MANAGED

        Transform(Vector3D position, Quaternion rotation, Vector3D scale);

        static Transform Lerp(Transform from, Transform to, float weight);
    };

#ifdef _MANAGED
}
#else //_MANAGED
}
#endif //_MANAGED

#undef TRANSLATE_PROPERTY_NAME
#undef TRANSLATE_TYPE_NAME

#endif //__SLERPYCORE_TYPES__