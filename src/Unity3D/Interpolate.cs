using UnityEngine;

namespace Slerpy.Unity3D
{
    public static class Interpolate
    {
        public static Color Color(InterpolateType interpolateType, Color from, Color to, float weight)
        {
            return new Color(
                Slerpy.Interpolate.WithType(interpolateType, from.r, to.r, weight),
                Slerpy.Interpolate.WithType(interpolateType, from.g, to.g, weight),
                Slerpy.Interpolate.WithType(interpolateType, from.b, to.b, weight),
                Slerpy.Interpolate.WithType(interpolateType, from.a, to.a, weight));
        }

        public static Color Color(Color from, Color to, float weight)
        {
            return Interpolate.Color(InterpolateType.Standard, from, to, weight);
        }

        public static Vector2 Vector2(InterpolateType interpolateType, Vector2 from, Vector2 to, float weight)
        {
            return new Vector2(
                Slerpy.Interpolate.WithType(interpolateType, from.x, to.x, weight),
                Slerpy.Interpolate.WithType(interpolateType, from.y, to.y, weight));
        }

        public static Vector2 Vector2(Vector2 from, Vector2 to, float weight)
        {
            return Interpolate.Vector2(InterpolateType.Standard, from, to, weight);
        }

        public static Vector3 Vector3(InterpolateType interpolateType, Vector3 from, Vector3 to, float weight)
        {
            return new Vector3(
                Slerpy.Interpolate.WithType(interpolateType, from.x, to.x, weight),
                Slerpy.Interpolate.WithType(interpolateType, from.y, to.y, weight),
                Slerpy.Interpolate.WithType(interpolateType, from.z, to.z, weight));
        }

        public static Vector3 Vector3(Vector3 from, Vector3 to, float weight)
        {
            return Interpolate.Vector3(InterpolateType.Standard, from, to, weight);
        }

        public static Vector4 Vector4(InterpolateType interpolateType, Vector4 from, Vector4 to, float weight)
        {
            return new Vector4(
                Slerpy.Interpolate.WithType(interpolateType, from.x, to.x, weight),
                Slerpy.Interpolate.WithType(interpolateType, from.y, to.y, weight),
                Slerpy.Interpolate.WithType(interpolateType, from.z, to.z, weight),
                Slerpy.Interpolate.WithType(interpolateType, from.w, to.w, weight));
        }

        public static Vector4 Vector4(Vector4 from, Vector4 to, float weight)
        {
            return Interpolate.Vector4(InterpolateType.Standard, from, to, weight);
        }

        public static Quaternion Quaternion(InterpolateType interpolateType, Quaternion from, Quaternion to, float weight)
        {
            // Allows negative weights to work with quaternion value wrapping
            if (weight < 0.0f)
            {
                weight = -weight;

                to *= UnityEngine.Quaternion.Inverse(from);
                to = from * UnityEngine.Quaternion.Inverse(to);
            }

            // Allow for seamless wrapping
            if (UnityEngine.Quaternion.Dot(from, to) < 0.0f)
            {
                to = new Quaternion(-to.x, -to.y, -to.z, -to.w);
            }

            Quaternion result = new Quaternion(
                Slerpy.Interpolate.WithType(interpolateType, from.x, to.x, weight),
                Slerpy.Interpolate.WithType(interpolateType, from.y, to.y, weight),
                Slerpy.Interpolate.WithType(interpolateType, from.z, to.z, weight),
                Slerpy.Interpolate.WithType(interpolateType, from.w, to.w, weight));

            // Normalize the result
            float magnitudeScale = 1.0f / UnityEngine.Quaternion.Dot(result, result);

            result.x *= magnitudeScale;
            result.y *= magnitudeScale;
            result.z *= magnitudeScale;
            result.w *= magnitudeScale;

            return result;
        }

        public static Quaternion Quaternion(Quaternion from, Quaternion to, float weight)
        {
            return Interpolate.Quaternion(InterpolateType.Standard, from, to, weight);
        }
    }
}
