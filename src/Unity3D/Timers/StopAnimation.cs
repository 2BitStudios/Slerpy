using UnityEngine;

namespace Slerpy.Unity3D.Timers
{
    public sealed class StopAnimation : Timer<Animation>
    {
        protected override void Trigger()
        {
            this.Target.Stop();
        }
    }
}
