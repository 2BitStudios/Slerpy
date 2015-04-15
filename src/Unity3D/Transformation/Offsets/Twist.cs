using UnityEngine;

namespace Slerpy.Unity3D
{
    public sealed class Twist : Offset
    {
        protected override Slerpy.Transform CalculateOffset(float time)
        {
            return Slerpy.Transformation.OffsetTwist(time, this.AxisWeightings.AsStruct(time), this.AllowInversion);
        }
    }
}
