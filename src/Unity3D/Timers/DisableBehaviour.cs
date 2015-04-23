using UnityEngine;

namespace Slerpy.Unity3D.Timers
{
    public sealed class DisableBehaviour : Timer<Behaviour>
    {
        protected override void Trigger()
        {
            this.Target.enabled = false;
        }
    }
}
