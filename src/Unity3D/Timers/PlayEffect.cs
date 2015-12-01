using UnityEngine;

namespace Slerpy.Unity3D.Timers
{
    public sealed class PlayEffect : Timer<Effect>
    {
        protected override void Trigger()
        {
            this.Target.Play();
        }
    }
}
