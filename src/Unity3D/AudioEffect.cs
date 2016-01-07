using System;

using UnityEngine;

namespace Slerpy.Unity3D
{
    public enum AudioFilterMode
    {
        Volume = 0
    }

    public struct AudioFilter
    {
        public AudioFilterMode Mode { get; private set; }
        public float Weight { get; private set; }

        public AudioFilter(AudioFilterMode mode, float weight)
        {
            this.Mode = mode;
            this.Weight = weight;
        }

        public void Apply(float[] samples, int channelCount)
        {
            switch (this.Mode)
            {
                case AudioFilterMode.Volume:
                    for (int i = 0; i < samples.Length; ++i)
                    {
                        samples[i] *= this.Weight;
                    }

                    break;
            }
        }
    }

    public sealed class AudioEffect : Effect
    {
        private const string TOOLTIP_FILTERMODE = "The filter that is applied to the audio stream by this effect.";

        private const AudioFilterMode DEFAULT_FILTERMODE = AudioFilterMode.Volume;

        public static AudioFilter CalculateAudioFilter(float weight, AudioFilterMode filterMode)
        {
            return new AudioFilter(filterMode, weight);
        }

        [SerializeField]
        [Tooltip(Effect.TOOLTIP_DURATION)]
        private float duration = 1.0f;

        [SerializeField]
        [Tooltip(Effect.TOOLTIP_TIMEWRAP)]
        private WrapType timeWrap = WrapType.Clamp;

        [SerializeField]
        [Tooltip(AudioEffect.TOOLTIP_FILTERMODE)]
        private AudioFilterMode filterMode = DEFAULT_FILTERMODE;

        [SerializeField]
        [HideInInspector]
        private AudioFilter filter = new AudioFilter(AudioEffect.DEFAULT_FILTERMODE, 0.0f);

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

        public AudioFilterMode FilterMode
        {
            get
            {
                return this.filterMode;
            }

            set
            {
                this.filterMode = value;
            }
        }

        public AudioFilter Filter
        {
            get
            {
                return this.filter;
            }

            set
            {
                this.filter = value;
            }
        }

        protected override void ProcessEffect(float weight)
        {
            this.filter = AudioEffect.CalculateAudioFilter(weight, this.filterMode);
        }

        private void OnAudioFilterRead(float[] samples, int channelCount)
        {
            this.filter.Apply(samples, channelCount);
        }

        [Serializable]
        public sealed class Detachable : Effect.Detachable<AudioFilter>
        {
            [SerializeField]
            [Tooltip(AudioEffect.TOOLTIP_FILTERMODE)]
            private AudioFilterMode filterMode = DEFAULT_FILTERMODE;

            public AudioFilterMode FilterMode
            {
                get
                {
                    return this.filterMode;
                }

                set
                {
                    this.filterMode = value;
                }
            }

            protected override AudioFilter Internal_CalculateState(float weight)
            {
                return AudioEffect.CalculateAudioFilter(weight, this.filterMode);
            }
        }
    }
}
