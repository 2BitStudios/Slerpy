using UnityEngine;

namespace Slerpy.Unity3D
{
    public abstract class Effect : MonoBehaviour
    {
        [SerializeField]
        private float rate = 1.0f;

        [SerializeField]
        private AnimationCurve rateScale = AnimationCurve.Linear(0.0f, 1.0f, 1.0f, 1.0f);

        [SerializeField]
        private float strength = 1.0f;

        [SerializeField]
        private AnimationCurve strengthScale = AnimationCurve.Linear(0.0f, 1.0f, 1.0f, 1.0f);

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

        protected abstract void ProcessEffect(float deltaTime, float totalTime, float strength);
        
        protected void Start()
        {
            this.ProcessEffect(0.0f, this.simulatedTime, this.ScaledStrength);
        }

        protected void Update()
        {
            float deltaTime = Time.deltaTime;

            this.rawTime += deltaTime;

            deltaTime *= this.ScaledRate;
            
            this.simulatedTime += deltaTime;

            this.ProcessEffect(deltaTime, this.simulatedTime, this.ScaledStrength);
        }
    }
}
