using UnityEngine;
using UnityEngine.Events;

namespace Slerpy.Unity3D.Timers
{
    public sealed class RaiseEvent : Timer<UnityEvent>
    {
        protected override void Trigger()
        {
            this.Target.Invoke();
        }
    }
}
