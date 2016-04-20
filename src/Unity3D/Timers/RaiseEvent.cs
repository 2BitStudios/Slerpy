using UnityEngine;
using UnityEngine.Events;

namespace Slerpy.Unity3D.Timers
{
    public sealed class RaiseEvent : Timer
    {
        [SerializeField]
        private UnityEvent target = new UnityEvent();

        public UnityEvent Target
        {
            get
            {
                return this.target;
            }
        }

        protected override void Trigger()
        {
            this.Target.Invoke();
        }
    }
}
