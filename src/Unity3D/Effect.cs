using System;
using System.Collections.Generic;

using UnityEngine;

namespace Slerpy.Unity3D
{
    public abstract class Effect : MonoBehaviour
    {
        [SerializeField]
        private float rate = 1.0f;

        [SerializeField]
        private Modifier[] rateModifiers = null;

        [SerializeField]
        private float strength = 1.0f;

        [SerializeField]
        private Modifier[] strengthModifiers = null;

        private float rawTime = 0.0f;
        private float totalTime = 0.0f;

        public float UnmodifiedRate
        {
            get
            {
                return this.rate;
            }

            set
            {
                if (this.rate != value)
                {
                    this.OnRateChanged(this.rate, this.rate = value);
                }
            }
        }

        public float ModifiedRate
        {
            get
            {
                float modifiedRate = this.UnmodifiedRate;

                for (int i = 0; i < this.rateModifiers.Length; ++i)
                {
                    modifiedRate = this.rateModifiers[i].Evaluate(modifiedRate, this.rawTime);
                }

                return modifiedRate;
            }
        }

        public IEnumerable<Modifier> RateModifiers
        {
            get
            {
                return this.rateModifiers;
            }
        }

        public float UnmodifiedStrength
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

        public float ModifiedStrength
        {
            get
            {
                float modifiedStrength = this.UnmodifiedStrength;

                for (int i = 0; i < this.strengthModifiers.Length; ++i)
                {
                    modifiedStrength = this.strengthModifiers[i].Evaluate(modifiedStrength, this.rawTime);
                }

                return modifiedStrength;
            }
        }

        public IEnumerable<Modifier> StrengthModifiers
        {
            get
            {
                return this.strengthModifiers;
            }
        }

        public void ResetTime()
        {
            this.rawTime = 0.0f;
            this.totalTime = 0.0f;
        }

        protected abstract void ProcessEffect(float deltaTime, float totalTime, float strength);

        protected virtual void OnRateChanged(float oldRate, float newRate)
        {
        }

        protected virtual void OnStrengthChanged(float oldStrength, float newStrength)
        {
        }

        protected void Start()
        {
            this.ProcessEffect(0.0f, this.totalTime, this.strength);
        }

        protected void Update()
        {
            float deltaTime = Time.deltaTime;

            this.rawTime += deltaTime;

            deltaTime *= this.ModifiedRate;
            
            this.totalTime += deltaTime;

            this.ProcessEffect(deltaTime, this.totalTime, this.ModifiedStrength);
        }
    }
}
