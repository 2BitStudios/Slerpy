using UnityEngine;

namespace Slerpy.Unity3D.Timers
{
    public sealed class DestroyComponent : Timer<Component>
    {
        protected override void Trigger()
        {
            Component.Destroy(this.Target);
        }
    }
}
