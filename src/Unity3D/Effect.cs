using System.Collections.Generic;

using UnityEngine;

namespace Slerpy.Unity3D
{
    public abstract class Effect : MonoBehaviour
    {
        protected const string TOOLTIP_INTERPOLATE = "Weight interpolation method.";
        protected const string TOOLTIP_DURATION = "Run time of the effect, to be modified by 'rate'.";
        protected const string TOOLTIP_TIMEWRAP = "How time continues to affect the effect once the duration ends.";

        [SerializeField]
        [Tooltip("Rate that time passes. Speeds up or slows down effects.")]
        private float rate = 1.0f;

        [SerializeField]
        [Tooltip("Multiplies against rate. Evaluation time is raw running time, not rate-modified time.")]
        private AnimationCurve rateScale = AnimationCurve.Linear(0.0f, 1.0f, 1.0f, 1.0f);

        [SerializeField]
        [Tooltip("Strength of effect. For example, an effect that moves the object would move twice as far with a strength of 2.0.")]
        private float strength = 1.0f;

        [SerializeField]
        [Tooltip("Multiplies against strength. Evaluation time is raw running time, not rate-modified time.")]
        private AnimationCurve strengthScale = AnimationCurve.Linear(0.0f, 1.0f, 1.0f, 1.0f);

        [SerializeField]
        [Tooltip("List of weight modifiers to be applied to the base weight of the effect. Will be applied in order listed here.")]
        private WeightType[] weights = new WeightType[] { WeightType.Linear };

        private float rawTime = 0.0f;
        private float simulatedTime = 0.0f;

        public float UnscaledRate
        {
            get
            {
                return this.rate;
            }

            set
            {
                this.rate = value;
            }
        }

        public float ScaledRate
        {
            get
            {
                return this.UnscaledRate * this.rateScale.Evaluate(this.rawTime);
            }
        }

        public AnimationCurve RateScale
        {
            get
            {
                return this.rateScale;
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
            this.rate = -this.rate;
        }

        [ContextMenu("Rewind")]
        public void Rewind()
        {
            this.rawTime = 0.0f;
            this.simulatedTime = 0.0f;
        }

        public float CalculateWeight()
        {
            float weight = Weight.FromTime(
                this.TimeWrap,
                this.simulatedTime,
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
            this.ProcessEffect(0.0f, this.ScaledStrength);
        }

        protected void Update()
        {
            float deltaTime = Time.deltaTime;

            this.rawTime += deltaTime;

            deltaTime *= this.ScaledRate;
            
            this.simulatedTime += deltaTime;

            this.ProcessEffect(this.CalculateWeight(), this.ScaledStrength);
        }
    }
}
