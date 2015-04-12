#ifndef __SLERPYCORE_TYPES__
#define __SLERPYCORE_TYPES__

#ifdef _MANAGED
namespace Slerpy
{
    public value struct Vector3D
    {
    public:
        property float X;
        property float Y;
        property float Z;

        Vector3D(float x, float y, float z);
    };

    public value struct Quaternion
    {
    public:
        property float X;
        property float Y;
        property float Z;
        property float W;

        Quaternion(float x, float y, float z, float w);
    };

    public value struct Transform
    {
    public:
        property Vector3D Position;
        property Quaternion Rotation;
        property Vector3D Scale;

        Transform(Vector3D position, Quaternion rotation, Vector3D scale);
    };
}
#else //_MANAGED
extern "C"
{
    struct Vector3D
    {
    public:
        float X;
        float Y;
        float Z;
    };

    struct Quaternion
    {
    public:
        float X;
        float Y;
        float Z;
        float W;
    };

    struct Transform
    {
    public:
        Vector3D Position;
        Quaternion Rotation;
        Vector3D Scale;
    };
}
#endif //_MANAGED

#endif //__SLERPYCORE_TYPES__