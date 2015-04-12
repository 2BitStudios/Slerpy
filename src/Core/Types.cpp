#include "Types.h"

#ifdef _MANAGED
namespace Slerpy
{
    Vector3D::Vector3D(float x, float y, float z)
    {
        this->X = x;
        this->Y = y;
        this->Z = z;
    }

    Quaternion::Quaternion(float x, float y, float z, float w)
    {
        this->X = x;
        this->Y = y;
        this->Z = z;
        this->W = w;
    }

    Transform::Transform(Vector3D position, Quaternion rotation, Vector3D scale)
    {
        this->Position = position;
        this->Rotation = rotation;
        this->Scale = scale;
    }
}
#endif //_MANAGED