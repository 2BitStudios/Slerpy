using UnityEngine;

namespace Slerpy.Unity3D.Timers
{
    public sealed class DestroyGameObject : Timer<GameObject>
    {
        protected override void Trigger()
        {
            GameObject.Destroy(this.Target);
        }
    }
}
