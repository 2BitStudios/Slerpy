using System.Collections.Generic;

using UnityEngine;

namespace Slerpy.Unity3D
{
    public abstract class Effect : MonoBehaviour
    {
        protected const string TOOLTIP_CYCLETIME = "Run time of a single cycle, to be modified by 'rate'.";
        protected const string TOOLTIP_TIMEWRAPTYPE = "How 'time' continues to affect the effect once the cycle ends.";

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
        [Tooltip("Weight interpolation method.")]
        private InterpolateType interpolate = InterpolateType.Standard;

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

        public InterpolateType Interpolate
        {
            get
            {
                return this.interpolate;
            }

            set
            {
                this.interpolate = value;
            }
        }

        public IEnumerable<WeightType> Weights
        {
            get
            {
                return this.weights;
            }
        }

        public abstract float CycleTime { get; }

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

        public void ResetTime()
        {
            this.rawTime = 0.0f;
            this.simulatedTime = 0.0f;
        }

        public float CalculateWeight()
        {
            float weight = Weight.FromTime(
                this.TimeWrap,
                this.simulatedTime,
                this.CycleTime);

            for (int i = 0; i < this.weights.Length; ++i)
            {
                weight = Weight.WithType(this.weights[i], weight);
            }

            return weight;
        }

        protected abstract void ProcessEffect(InterpolateType interpolateType, float weight, float strength);
        
        protected void Start()
        {
            this.ProcessEffect(this.interpolate, 0.0f, this.ScaledStrength);
        }

        protected void Update()
        {
            float deltaTime = Time.deltaTime;

            this.rawTime += deltaTime;

            deltaTime *= this.ScaledRate;
            
            this.simulatedTime += deltaTime;

            this.ProcessEffect(this.interpolate, this.CalculateWeight(), this.ScaledStrength);
        }
    }
}
