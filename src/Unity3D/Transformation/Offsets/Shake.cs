using UnityEngine;

namespace Slerpy.Unity3D
{
    public sealed class Shake : Offset
    {
        protected override Slerpy.Transform CalculateOffset(float time)
        {
            return Slerpy.Transformation.OffsetShake(time, this.AxisWeightings.AsStruct(time), this.AllowInversion);
        }
    }
}
