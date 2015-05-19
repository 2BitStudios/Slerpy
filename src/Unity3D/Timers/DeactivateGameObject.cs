using UnityEngine;

namespace Slerpy.Unity3D.Timers
{
    public sealed class DeactivateGameObject : Timer<GameObject>
    {
        protected override void Trigger()
        {
            this.Target.SetActive(false);
        }
    }
}
