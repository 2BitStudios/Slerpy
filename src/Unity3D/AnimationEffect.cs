using System;
using System.Collections.Generic;

using UnityEngine;

namespace Slerpy.Unity3D
{
    public sealed class AnimationEffect : Effect
    {
        [SerializeField]
        [Tooltip(Effect.TOOLTIP_DURATION)]
        private float duration = 1.0f;
        
        [SerializeField]
        [Tooltip(Effect.TOOLTIP_TIMEWRAP)]
        private WrapType timeWrap = WrapType.Clamp;

        [SerializeField]
        [Tooltip("Independent sequences in the effect.")]
        private List<Channel> channels = new List<Channel>();

        public override float Duration
        {
            get
            {
                return this.duration;
            }

            set
            {
                this.duration = value;
            }
        }

        public override WrapType TimeWrap
        {
            get
            {
                return this.timeWrap;
            }

            set
            {
                this.timeWrap = value;
            }
        }

        public IEnumerable<Channel> Channels
        {
            get
            {
                return this.channels;
            }
        }

        protected override void ProcessEffect(float weight, float strength)
        {
            for (int i = 0; i < this.channels.Count; ++i)
            {
                this.channels[i].Process(weight);
            }
        }

        [Serializable]
        public sealed class Channel
        {
            [SerializeField]
            [Tooltip("Minimum weight required to play these effects forward, otherwise the effects are played backward.")]
            private float threshold = 0.0f;

            [SerializeField]
            [Tooltip("'Duration' to play these effects forward. Weights at or past this span (plus threshold) will play the effects backward.\nNote: A span of 0.0 or less represents an infinite 'duration'.")]
            private float span = 0.0f;

            [SerializeField]
            [Tooltip("All effects to play. They will never be stopped, only played forward and backward.")]
            private List<Effect> effects = new List<Effect>();

            public float Threshold
            {
                get
                {
                    return this.threshold;
                }

                set
                {
                    this.threshold = value;
                }
            }

            public float Span
            {
                get
                {
                    return this.span;
                }

                set
                {
                    this.span = value;
                }
            }

            public IEnumerable<Effect> Effects
            {
                get
                {
                    return this.effects;
                }
            }

            public void Process(float weight)
            {
                bool isActive = weight >= this.threshold && (span <= 0.0f || weight < this.threshold + this.span);
                
                for (int i = 0; i < this.effects.Count; ++i)
                {
                    if (isActive)
                    {
                        this.effects[i].PlayForward();
                    }
                    else
                    {
                        this.effects[i].PlayBackward();
                    }
                }
            }
        }
    }
}
