using UnityEngine;

namespace Slerpy.Unity3D.Timers
{
    public sealed class PlayAnimation : Timer<Animation>
    {
        [SerializeField]
        private string animationName = null;

        [SerializeField]
        private bool shouldCrossfade = true;

        public string AnimationName
        {
            get
            {
                return this.animationName;
            }

            set
            {
                this.animationName = value;
            }
        }

        public bool ShouldCrossfade
        {
            get
            {
                return this.shouldCrossfade;
            }

            set
            {
                this.shouldCrossfade = value;
            }
        }

        protected override void Trigger()
        {
            if (this.shouldCrossfade)
            {
                this.Target.CrossFade(this.animationName);
            }
            else
            {
                this.Target.Play(this.animationName);
            }
        }
    }
}
