using UnityEngine;

namespace Slerpy.Unity3D.Timers
{
    public sealed class ReverseEffect : Timer<Effect>
    {
        protected override void Trigger()
        {
            this.Target.Reverse();
        }
    }
}
