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
                if (this.duration != value)
                {
                    this.duration = value;

                    this.Refresh();
                }
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
                if (this.timeWrap != value)
                {
                    this.timeWrap = value;

                    this.Refresh();
                }
            }
        }

        public IEnumerable<Channel> Channels
        {
            get
            {
                return this.channels;
            }
        }

        public void AddChannel(Channel channel)
        {
            this.channels.Add(channel);

            this.Refresh();
        }

        public Channel AddChannel(float threshold, float span, IEnumerable<Effect> effects)
        {
            Channel newChannel = new Channel(threshold, span, effects);

            this.AddChannel(newChannel);

            return newChannel;
        }

        public void RemoveChannel(Channel channel)
        {
            this.channels.Remove(channel);

            this.Refresh();
        }

        protected override void ProcessEffect(float weight)
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

            public Channel()
            {
            }

            public Channel(float threshold, float span, IEnumerable<Effect> effects)
            {
                this.threshold = threshold;
                this.span = span;

                this.effects.AddRange(effects);
            }

            public float Threshold
            {
                get
                {
                    return this.threshold;
                }
            }

            public float Span
            {
                get
                {
                    return this.span;
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
