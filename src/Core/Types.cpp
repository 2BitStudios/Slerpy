#include "Types.h"

#include "SystemDefines.h"

#ifdef _MANAGED

#define DEFINE_OPERATOR_OVERLOAD(type, operatorSymbol) type type::operator operatorSymbol(type lhs, type rhs)

#else //_MANAGED

#define DEFINE_OPERATOR_OVERLOAD(type, operatorSymbol) type operator operatorSymbol(type const& lhs, type const& rhs)

#endif //_MANAGED

#ifdef _MANAGED
namespace Slerpy
{
#else //_MANAGED

#endif //_MANAGED

    Vector3D::Vector3D(float x, float y, float z)
    {
        this->X = x;
        this->Y = y;
        this->Z = z;
    }

    Vector3D Vector3D::Lerp(Vector3D from, Vector3D to, float weight)
    {
        return Vector3D(
            MATH_LERP(from.X, to.X, weight), 
            MATH_LERP(from.Y, to.Y, weight), 
            MATH_LERP(from.Z, to.Z, weight));
    }

    DEFINE_OPERATOR_OVERLOAD(Vector3D, +)
    {
        return Vector3D(
            lhs.X + rhs.X,
            lhs.Y + rhs.Y,
            lhs.Z + rhs.Z);
    }

    DEFINE_OPERATOR_OVERLOAD(Vector3D, -)
    {
        return Vector3D(
            lhs.X - rhs.X,
            lhs.Y - rhs.Y,
            lhs.Z - rhs.Z);
    }

    Quaternion::Quaternion(float x, float y, float z, float w)
    {
        this->X = x;
        this->Y = y;
        this->Z = z;
        this->W = w;
    }

    Quaternion Quaternion::Lerp(Quaternion from, Quaternion to, float weight)
    {
        Quaternion returnValue = Quaternion(
            MATH_LERP(from.X, to.X, weight),
            MATH_LERP(from.Y, to.Y, weight),
            MATH_LERP(from.Z, to.Z, weight),
            MATH_LERP(from.W, to.W, weight));

        returnValue.Normalize();

        return returnValue;
    }

    Quaternion Quaternion::FromEuler(float x, float y, float z)
    {
        x = MATH_DEG2RAD(x);
        y = MATH_DEG2RAD(y);
        z = MATH_DEG2RAD(z);

        float sinX = MATH_SIN(0.5f * x);
        float sinY = MATH_SIN(0.5f * y);
        float sinZ = MATH_SIN(0.5f * z);

        float cosX = MATH_COS(0.5f * x);
        float cosY = MATH_COS(0.5f * y);
        float cosZ = MATH_COS(0.5f * z);

        return Quaternion(
            sinX * cosY * cosZ - cosX * sinY * sinZ,
            cosX * sinY * cosZ + sinX * cosY * sinZ,
            cosX * cosY * sinZ - sinX * sinY * cosZ,
            cosX * cosY * cosZ + sinX * sinY * sinZ);
    }

    void Quaternion::Normalize()
    {
        float const length = MATH_SQRT(
            this->X * this->X +
            this->Y * this->Y +
            this->Z * this->Z +
            this->W * this->W);

        float const lengthScale = 1.0f / length;

        this->X *= lengthScale;
        this->Y *= lengthScale;
        this->Z *= lengthScale;
        this->W *= lengthScale;
    }

    DEFINE_OPERATOR_OVERLOAD(Quaternion, *)
    {
        return Quaternion(
            lhs.Y * rhs.Z - lhs.Z * rhs.Y + lhs.W * rhs.X + lhs.X * rhs.W,
            lhs.Z * rhs.X - lhs.X * rhs.Z + lhs.W * rhs.Y + lhs.Y * rhs.W,
            lhs.X * rhs.Y - lhs.Y * rhs.X + lhs.W * rhs.Z + lhs.Z * rhs.W,
            lhs.W * rhs.W - (lhs.X * rhs.X + lhs.Y * rhs.Y + lhs.Z * rhs.Z));
    }

    Transform::Transform(Vector3D position, Quaternion rotation, Vector3D scale)
    {
        this->Position = position;
        this->Rotation = rotation;
        this->Scale = scale;
    }

    Transform Transform::Lerp(Transform from, Transform to, float weight)
    {
        return Transform(
            Vector3D::Lerp(from.Position, to.Position, weight), 
            Quaternion::Lerp(from.Rotation, to.Rotation, weight), 
            Vector3D::Lerp(from.Scale, to.Scale, weight));
    }

    Weighting::Weighting(float rate, float strength)
    {
        this->Rate = rate;
        this->Strength = strength;
    }

    AxisWeightings::AxisWeightings(Weighting x, Weighting y, Weighting z)
    {
        this->X = x;
        this->Y = y;
        this->Z = z;
    }

#ifdef _MANAGED
}
#else //_MANAGED

#endif //_MANAGED

#undef DEFINE_OPERATOR_OVERLOAD