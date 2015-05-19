using UnityEngine;

namespace Slerpy.Unity3D
{
    public static class Extensions
    {
        public static Color Interpolate(Color from, Color to, float weight, InterpolateType interpolateType = InterpolateType.Standard)
        {
            return new Color(
                Slerpy.Interpolate.WithType(interpolateType, from.r, to.r, weight),
                Slerpy.Interpolate.WithType(interpolateType, from.g, to.g, weight),
                Slerpy.Interpolate.WithType(interpolateType, from.b, to.b, weight),
                Slerpy.Interpolate.WithType(interpolateType, from.a, to.a, weight));
        }

        public static Vector2 Interpolate(Vector2 from, Vector2 to, float weight, InterpolateType interpolateType = InterpolateType.Standard)
        {
            return new Vector2(
                Slerpy.Interpolate.WithType(interpolateType, from.x, to.x, weight),
                Slerpy.Interpolate.WithType(interpolateType, from.y, to.y, weight));
        }

        public static Vector3 Interpolate(Vector3 from, Vector3 to, float weight, InterpolateType interpolateType = InterpolateType.Standard)
        {
            return new Vector3(
                Slerpy.Interpolate.WithType(interpolateType, from.x, to.x, weight),
                Slerpy.Interpolate.WithType(interpolateType, from.y, to.y, weight),
                Slerpy.Interpolate.WithType(interpolateType, from.z, to.z, weight));
        }

        public static Vector4 Interpolate(Vector4 from, Vector4 to, float weight, InterpolateType interpolateType = InterpolateType.Standard)
        {
            return new Vector4(
                Slerpy.Interpolate.WithType(interpolateType, from.x, to.x, weight),
                Slerpy.Interpolate.WithType(interpolateType, from.y, to.y, weight),
                Slerpy.Interpolate.WithType(interpolateType, from.z, to.z, weight),
                Slerpy.Interpolate.WithType(interpolateType, from.w, to.w, weight));
        }

        public static Quaternion Interpolate(Quaternion from, Quaternion to, float weight, InterpolateType interpolateType = InterpolateType.Standard)
        {
            // Allows negative weights to work with quaternion value wrapping
            if (weight < 0.0f)
            {
                weight = -weight;

                to *= Quaternion.Inverse(from);
                to = from * Quaternion.Inverse(to);
            }

            // Allow for seamless wrapping
            if (Quaternion.Dot(from, to) < 0.0f)
            {
                to = new Quaternion(-to.x, -to.y, -to.z, -to.w);
            }

            Quaternion result = new Quaternion(
                Slerpy.Interpolate.WithType(interpolateType, from.x, to.x, weight),
                Slerpy.Interpolate.WithType(interpolateType, from.y, to.y, weight),
                Slerpy.Interpolate.WithType(interpolateType, from.z, to.z, weight),
                Slerpy.Interpolate.WithType(interpolateType, from.w, to.w, weight));

            // Normalize the result
            float magnitudeScale = 1.0f / Quaternion.Dot(result, result);

            result.x *= magnitudeScale;
            result.y *= magnitudeScale;
            result.z *= magnitudeScale;
            result.w *= magnitudeScale;

            return result;
        }
    }
}
