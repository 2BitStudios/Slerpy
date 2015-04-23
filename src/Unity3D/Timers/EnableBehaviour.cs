using UnityEngine;

namespace Slerpy.Unity3D.Timers
{
    public sealed class EnableBehaviour : Timer<Behaviour>
    {
        protected override void Trigger()
        {
            this.Target.enabled = true;
        }
    }
}
