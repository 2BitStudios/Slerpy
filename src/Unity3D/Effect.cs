using System;
using System.Collections.Generic;

using UnityEngine;

namespace Slerpy.Unity3D
{
    public abstract class Effect : MonoBehaviour
    {
        private float timeRunning = 0.0f;

        [SerializeField]
        private float timeRemaining = 0.0f;

        public float TimeRunning
        {
            get
            {
                return this.timeRunning;
            }
        }

        public float TimeRemaining
        {
            get
            {
                return this.timeRemaining;
            }
        }

        protected abstract void ProcessEffect(float deltaTime);

        protected void Start()
        {
            this.ProcessEffect(this.timeRunning);
        }

        protected void Update()
        {
            if (this.timeRemaining > 0.0f && (this.timeRemaining -= Time.deltaTime) < 0.0f)
            {
                MonoBehaviour.Destroy(this);
            }
            else
            {
                this.timeRunning += Time.deltaTime;

                this.ProcessEffect(Time.deltaTime);
            }
        }
    }
}
