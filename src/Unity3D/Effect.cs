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
        private float strength = 1.0f;

        private float totalTime = 0.0f;

        public float Rate
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

        public float Strength
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
            float deltaTime = Time.deltaTime * this.rate;

            this.totalTime += deltaTime;

            this.ProcessEffect(deltaTime, this.totalTime, this.strength);
        }
    }
}
