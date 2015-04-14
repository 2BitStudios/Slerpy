namespace Slerpy.Unity3D
{
    public static class Extensions
    {
        public static Slerpy.Vector3D ToSlerpy(this UnityEngine.Vector3 origin)
        {
            return new Slerpy.Vector3D(origin.x, origin.y, origin.z);
        }

        public static UnityEngine.Vector3 ToUnity3D(this Slerpy.Vector3D origin)
        {
            return new UnityEngine.Vector3(origin.X, origin.Y, origin.Z);
        }

        public static Slerpy.Quaternion ToSlerpy(this UnityEngine.Quaternion origin)
        {
            return new Slerpy.Quaternion(origin.x, origin.y, origin.z, origin.w);
        }

        public static UnityEngine.Quaternion ToUnity3D(this Slerpy.Quaternion origin)
        {
            return new UnityEngine.Quaternion(origin.X, origin.Y, origin.Z, origin.W);
        }

        public static Slerpy.Transform ToSlerpy(this UnityEngine.Transform origin)
        {
            return new Slerpy.Transform(
                origin.position.ToSlerpy(),
                origin.rotation.ToSlerpy(),
                origin.localScale.ToSlerpy());
        }

        public static void ToUnity3D(this Slerpy.Transform origin, UnityEngine.Transform target)
        {
            target.position = origin.Position.ToUnity3D();
            target.rotation = origin.Rotation.ToUnity3D();
            target.localScale = origin.Scale.ToUnity3D();
        }
    }
}
