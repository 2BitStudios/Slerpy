using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Serialization;

namespace Slerpy.Unity3D
{
    public enum EffectSettingTimeScaling
    {
        Scaled = 0,
        Unscaled = 1
    }

    public enum EffectSettingReverseClamp
    {
        Never = 0,
        ByTimeWrap = 1
    }

    [Serializable]
    public sealed class EffectSettings
    {
        [SerializeField]
        [Tooltip("How to handle engine time scaling (such as pauses).")]
        private EffectSettingTimeScaling timeScaling = EffectSettingTimeScaling.Scaled;

        [SerializeField]
        [Tooltip("Whether to clamp simulated time between 0.0 and 'duration' when effect is reversed.")]
        private EffectSettingReverseClamp reverseClamp = EffectSettingReverseClamp.ByTimeWrap;

        public EffectSettingTimeScaling TimeScaling
        {
            get
            {
                return this.timeScaling;
            }

            set
            {
                this.timeScaling = value;
            }
        }

        public EffectSettingReverseClamp ReverseClamp
        {
            get
            {
                return this.reverseClamp;
            }

            set
            {
                this.reverseClamp = value;
            }
        }
    }

    public enum EffectDirection
    {
        Stalled = 0,
        Forward = 1,
        Backward = 2
    }

    public abstract class Effect : MonoBehaviour
    {
        protected const string TOOLTIP_DURATION = "Run time of the effect, affected by 'speed'.";
        protected const string TOOLTIP_TIMEWRAP = "How time continues to affect the effect once the duration ends.";

        [SerializeField]
        [Tooltip("General settings.")]
        private EffectSettings settings = new EffectSettings();

        [SerializeField]
        [Tooltip("Rate that time passes. Speeds up or slows down effects.")]
        private float speed = 1.0f;

        [SerializeField]
        [Tooltip("Strength of effect. For example, an effect that moves the object would move twice as far with a strength of 2.0.")]
        private float strength = 1.0f;

        [SerializeField]
        [Tooltip("List of weight modifiers to be applied to the base weight of the effect. Will be applied in order they appear here.")]
        private WeightType[] weights = new WeightType[] { WeightType.Linear };

        [SerializeField]
        [HideInInspector]
        private float rawTime = 0.0f;

        [SerializeField]
        [HideInInspector]
        private float simulatedTime = 0.0f;

        public EffectSettings Settings
        {
            get
            {
                return this.settings;
            }
        }

        public float Speed
        {
            get
            {
                return this.speed;
            }

            set
            {
                this.speed = value;
            }
        }

        public EffectDirection Direction
        {
            get
            {
                return this.speed > 0.0f ? EffectDirection.Forward : (this.speed < 0.0f ? EffectDirection.Backward : EffectDirection.Stalled);
            }
        }

        public float Strength
        {
            get
            {
                return this.strength;
            }

            set
            {
                this.strength = value;
            }
        }

        public IEnumerable<WeightType> Weights
        {
            get
            {
                return this.weights;
            }
        }

        public abstract float Duration { get; set; }

        public abstract TimeWrapType TimeWrap { get; set; }

        public float RawTime
        {
            get
            {
                return this.rawTime;
            }
        }

        public float SimulatedTime
        {
            get
            {
                return this.simulatedTime;
            }

            set
            {
                if (this.settings.ReverseClamp == EffectSettingReverseClamp.ByTimeWrap && this.TimeWrap == TimeWrapType.Clamp)
                {
                    this.simulatedTime = Mathf.Clamp(value, 0.0f, this.Duration);
                }
                else
                {
                    this.simulatedTime = value;
                }

                this.ProcessEffect(this.CalculateWeight(), this.Strength);
            }
        }

        public bool IsPlaying
        {
            get
            {
                return this.enabled;
            }

            set
            {
                this.enabled = value;
            }
        }

        public void SetWeights(params WeightType[] newWeights)
        {
            this.weights = newWeights;
        }

        public void AddWeights(params WeightType[] additionalWeights)
        {
            WeightType[] newWeights = new WeightType[this.weights.Length + additionalWeights.Length];

            for (int i = 0; i < this.weights.Length; ++i)
            {
                newWeights[i] = this.weights[i];
            }

            for (int i = 0; i < additionalWeights.Length; ++i)
            {
                newWeights[i + this.weights.Length] = additionalWeights[i];
            }

            this.SetWeights(newWeights);
        }

        public void ClearWeights()
        {
            this.weights = new WeightType[0];
        }

        public void Play()
        {
            this.IsPlaying = true;
        }

        public void PlayForward()
        {
            this.Play();

            if (this.Direction == EffectDirection.Backward)
            {
                this.Reverse();
            }
        }

        public void PlayBackward()
        {
            this.Play();

            if (this.Direction == EffectDirection.Forward)
            {
                this.Reverse();
            }
        }

        public void Stop()
        {
            this.IsPlaying = false;
        }

        [ContextMenu("Reverse")]
        public void Reverse()
        {
            this.speed = -this.speed;
        }

        [ContextMenu("Rewind")]
        public void Rewind()
        {
            this.rawTime = 0.0f;
            this.simulatedTime = 0.0f;
        }

        public float CalculateWeight(float time)
        {
            float weight = Weight.FromTime(
                this.TimeWrap,
                time,
                this.Duration);

            for (int i = 0; i < this.weights.Length; ++i)
            {
                weight = Weight.WithType(this.weights[i], weight);
            }

            return weight;
        }

        public float CalculateWeight()
        {
            return this.CalculateWeight(this.SimulatedTime);
        }

        protected abstract void ProcessEffect(float weight, float strength);
        
        protected void Start()
        {
            this.ProcessEffect(this.CalculateWeight(), this.Strength);
        }

        protected void Update()
        {
            this.AddRawTime(this.settings.TimeScaling == EffectSettingTimeScaling.Unscaled ? Time.unscaledDeltaTime : Time.deltaTime);
        }

        private void AddRawTime(float deltaTime)
        {
            this.rawTime += deltaTime;

            deltaTime *= this.Speed;

            this.SimulatedTime += deltaTime;
        }
    }
}
