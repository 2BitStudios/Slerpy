using UnityEngine;

namespace Slerpy.Unity3D.Timers
{
    public enum TimerMode
    {
        OneShot = 0,
        Repeatable = 1,
        Looping = 2
    }

    public abstract class Timer<TTarget> : MonoBehaviour
        where TTarget : class
    {
        public static TTimer Set<TTimer>(GameObject owner, float triggerTime, TimerMode mode = TimerMode.OneShot)
            where TTimer : Timer<GameObject>
        {
            return Timer<GameObject>.Set<TTimer>(owner, triggerTime, owner, mode);
        }

        public static TTimer Set<TTimer>(GameObject owner, float triggerTime, TTarget target, TimerMode mode = TimerMode.OneShot)
            where TTimer : Timer<TTarget>
        {
            TTimer timer = owner.AddComponent<TTimer>();

            timer.Target = target;
            timer.TriggerTime = triggerTime;
            timer.Mode = mode;

            return timer;
        }

        [SerializeField]
        private TTarget target = null;

        [SerializeField]
        private TimerMode mode = TimerMode.OneShot;

        [SerializeField]
        private float triggerTime = 1.0f;

        private float runningTime = 0.0f;

        public TTarget Target
        {
            get
            {
                return this.target;
            }

            private set
            {
                this.target = value;
            }
        }

        public TimerMode Mode
        {
            get
            {
                return this.mode;
            }

            set
            {
                this.mode = value;
            }
        }

        public float TriggerTime
        {
            get
            {
                return this.triggerTime;
            }

            set
            {
                this.triggerTime = value;
            }
        }

        public float RunningTime
        {
            get
            {
                return this.runningTime;
            }
        }

        public float RemainingTime
        {
            get
            {
                return this.triggerTime - this.runningTime;
            }
        }

        public void ResetCountdown()
        {
            this.runningTime = 0.0f;
        }

        protected abstract void Trigger();

        protected void LateUpdate()
        {
            this.runningTime += Time.deltaTime;

            if (this.runningTime >= this.triggerTime)
            {
                this.Trigger();

                switch (this.mode)
                {
                    case TimerMode.OneShot:
                        Destroy(this);

                        break;
                    case TimerMode.Repeatable:
                        this.enabled = false;

                        this.runningTime = 0.0f;

                        break;
                    case TimerMode.Looping:
                        this.runningTime -= this.triggerTime;

                        break;
                }
            }
        }
    }
}
