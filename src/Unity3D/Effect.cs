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
        [Tooltip("How to clamp simulated time when effect is reversed. Necessary to play some time wraps (such as Clamp) backward smoothly.")]
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

    public enum EffectCustomWeightStimulus
    {
        CalculatedWeight = 0,
        RawWeight = 1,
        Time = 2
    }

    [Serializable]
    public sealed class EffectCustomWeight
    {
        [SerializeField]
        [Tooltip("The source of the value that drives the curve.")]
        private EffectCustomWeightStimulus stimulus = EffectCustomWeightStimulus.CalculatedWeight;

        [SerializeField]
        [Tooltip("The curve to evaluate against the 'stimulus'.")]
        private AnimationCurve curve = AnimationCurve.Linear(0.0f, 1.0f, 1.0f, 1.0f);

        public EffectCustomWeightStimulus Stimulus
        {
            get
            {
                return this.stimulus;
            }
        }

        public AnimationCurve Curve
        {
            get
            {
                return this.curve;
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
        [Tooltip("List of weight modifiers to be applied while calculating the weight of the effect. Will be applied in order they appear here.")]
        private WeightType[] weights = new WeightType[] { WeightType.Linear };

        [SerializeField]
        [Tooltip("Custom weight modifier to be applied while calculating the weight of the effect. Will be applied after all other modifiers.")]
        private EffectCustomWeight customWeight = new EffectCustomWeight();

        [SerializeField]
        [Tooltip("Rate that time passes. Speeds up or slows down effects.")]
        private float speed = 1.0f;

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

        public IEnumerable<WeightType> Weights
        {
            get
            {
                return this.weights;
            }
        }

        public EffectCustomWeight CustomWeight
        {
            get
            {
                return this.customWeight;
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

        public abstract float Duration { get; set; }

        public abstract WrapType TimeWrap { get; set; }

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
                this.simulatedTime = value;

                this.ProcessEffect(this.CalculateWeight());
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

            if (this.settings.ReverseClamp == EffectSettingReverseClamp.ByTimeWrap)
            {
                if (this.TimeWrap == WrapType.Clamp)
                {
                    this.SimulatedTime = Mathf.Clamp(this.SimulatedTime, 0.0f, this.Duration);
                }

                if (this.TimeWrap == WrapType.MirrorClamp)
                {
                    this.SimulatedTime = Mathf.Clamp(this.SimulatedTime, 0.0f, this.Duration * 2.0f);
                }
            }
        }

        [ContextMenu("Rewind")]
        public void Rewind()
        {
            this.rawTime = 0.0f;

            this.SimulatedTime = 0.0f;
        }

        public float CalculateWeight(float time)
        {
            float weight = Weight.FromTime(
                this.TimeWrap,
                time,
                this.Duration);

            float rawWeight = weight;

            for (int i = 0; i < this.weights.Length; ++i)
            {
                weight = Weight.WithType(this.weights[i], weight);
            }

            float customWeightStimulus = 0.0f;

            switch (this.customWeight.Stimulus)
            {
                case EffectCustomWeightStimulus.RawWeight:
                    customWeightStimulus = rawWeight;

                    break;
                case EffectCustomWeightStimulus.Time:
                    customWeightStimulus = time;

                    break;
                case EffectCustomWeightStimulus.CalculatedWeight:
                default:
                    customWeightStimulus = weight;

                    break;
            }

            return weight * this.customWeight.Curve.Evaluate(customWeightStimulus);
        }

        public float CalculateWeight()
        {
            return this.CalculateWeight(this.SimulatedTime);
        }

        protected abstract void ProcessEffect(float weight);
        
        protected void Start()
        {
            this.ProcessEffect(this.CalculateWeight());
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
