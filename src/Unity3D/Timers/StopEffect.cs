using UnityEngine;

namespace Slerpy.Unity3D.Timers
{
    public sealed class StopEffect : Timer<Effect>
    {
        protected override void Trigger()
        {
            this.Target.Stop();
        }
    }
}
