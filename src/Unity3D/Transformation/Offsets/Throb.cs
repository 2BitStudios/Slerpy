using UnityEngine;

namespace Slerpy.Unity3D
{
    public sealed class Throb : Offset
    {
        protected override Slerpy.Transform CalculateOffset(float time)
        {
            return Slerpy.Transformation.OffsetThrob(time, this.AxisWeightings.AsStruct(time), this.AllowInversion);
        }
    }
}
