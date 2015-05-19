using UnityEngine;

namespace Slerpy.Unity3D.Timers
{
    public sealed class ActivateGameObject : Timer<GameObject>
    {
        protected override void Trigger()
        {
            this.Target.SetActive(true);
        }
    }
}
