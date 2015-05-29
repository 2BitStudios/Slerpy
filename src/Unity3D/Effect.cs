using System;
using System.Collections.Generic;

using UnityEngine;

namespace Slerpy.Unity3D
{
    [Serializable]
    public sealed class TimeOptions
    {
        [SerializeField]
        [Tooltip("Raw time to begin the effect at.")]
        private float offset = 0.0f;

        [SerializeField]
        [Tooltip("Random time range to add to the effect at start.")]
        private float randomness = 0.0f;

        [SerializeField]
        [Tooltip("Whether to ignore engine time scaling (such as pauses). Does not ignore local scaling via 'speedScale'.")]
        private bool useUnscaledDelta = false;

        [SerializeField]
        [Tooltip("Whether to lock simulated time to 'duration' (clamping between 0 and 'duration').")]
        private bool clampToDuration = false;

        public float Offset
        {
            get
            {
                return this.offset;
            }
        }

        public float Randomness
        {
            get
            {
                return this.randomness;
            }
        }

        public bool UseUnscaledDelta
        {
            get
            {
                return this.useUnscaledDelta;
            }
        }

        public bool ClampToDuration
        {
            get
            {
                return this.clampToDuration;
            }
        }
    }

    public abstract class Effect : MonoBehaviour
    {
        protected const string TOOLTIP_INTERPOLATE = "Weight interpolation method.";
        protected const string TOOLTIP_DURATION = "Run time of the effect, affected by 'speed'.";
        protected const string TOOLTIP_TIMEWRAP = "How time continues to affect the effect once the duration ends.";

        [SerializeField]
        [Tooltip("Time settings, such as offsets and boundaries.")]
        private TimeOptions timeOptions = null;

        [SerializeField]
        [Tooltip("Rate that time passes. Speeds up or slows down effects.")]
        private float speed = 1.0f;

        [SerializeField]
        [Tooltip("Multiplies against 'speed'. Evaluation time is raw running time, not speed-modified time.")]
        private AnimationCurve speedScale = AnimationCurve.Linear(0.0f, 1.0f, 1.0f, 1.0f);

        [SerializeField]
        [Tooltip("Strength of effect. For example, an effect that moves the object would move twice as far with a strength of 2.0.")]
        private float strength = 1.0f;

        [SerializeField]
        [Tooltip("Multiplies against 'strength'. Evaluation time is raw running time, not speed-modified time.")]
        private AnimationCurve strengthScale = AnimationCurve.Linear(0.0f, 1.0f, 1.0f, 1.0f);

        [SerializeField]
        [Tooltip("List of weight modifiers to be applied to the base weight of the effect. Will be applied in order they appear here.")]
        private WeightType[] weights = new WeightType[] { WeightType.Linear };

        private float rawTime = 0.0f;
        private float simulatedTime = 0.0f;

        public TimeOptions TimeOptions
        {
            get
            {
                return this.timeOptions;
            }
        }

        public float UnscaledSpeed
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

        public float ScaledSpeed
        {
            get
            {
                return this.UnscaledSpeed * this.speedScale.Evaluate(this.rawTime);
            }
        }

        public AnimationCurve SpeedScale
        {
            get
            {
                return this.speedScale;
            }
        }

        public float UnscaledStrength
        {
            get
            {
                return this.strength;
            }

            set
            {
                if (this.strength != value)
                {
                    this.strength = value;
                }
            }
        }

        public float ScaledStrength
        {
            get
            {
                return this.UnscaledStrength * this.strengthScale.Evaluate(this.rawTime);
            }
        }

        public AnimationCurve StrengthScale
        {
            get
            {
                return this.strengthScale;
            }
        }

        public IEnumerable<WeightType> Weights
        {
            get
            {
                return this.weights;
            }
        }

        public abstract InterpolateType Interpolate { get; set; }

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
                if (this.timeOptions.ClampToDuration)
                {
                    this.simulatedTime = Mathf.Clamp(value, 0.0f, this.Duration);
                }
                else
                {
                    this.simulatedTime = value;
                }
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
            this.enabled = true;
        }

        public void Stop()
        {
            this.enabled = false;
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

            this.AddRawTime(this.timeOptions.Offset + UnityEngine.Random.Range(0.0f, this.timeOptions.Randomness));
        }

        public float CalculateWeight()
        {
            float weight = Weight.FromTime(
                this.TimeWrap,
                this.SimulatedTime,
                this.Duration);

            for (int i = 0; i < this.weights.Length; ++i)
            {
                weight = Weight.WithType(this.weights[i], weight);
            }

            return weight;
        }

        protected abstract void ProcessEffect(float weight, float strength);
        
        protected void Start()
        {
            this.Rewind();

            this.ProcessEffect(this.CalculateWeight(), this.ScaledStrength);
        }

        protected void Update()
        {
            this.AddRawTime(this.timeOptions.UseUnscaledDelta ? Time.unscaledDeltaTime : Time.deltaTime);

            this.ProcessEffect(this.CalculateWeight(), this.ScaledStrength);
        }

        private void AddRawTime(float deltaTime)
        {
            this.rawTime += deltaTime;

            deltaTime *= this.ScaledSpeed;

            this.SimulatedTime += deltaTime;
        }
    }
}
