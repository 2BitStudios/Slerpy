using System;
using System.Collections.Generic;

using UnityEngine;

namespace Slerpy.Unity3D
{
    public abstract class Effect : MonoBehaviour
    {
        private float timeRunning = 0.0f;

        public float TimeRunning
        {
            get
            {
                return this.timeRunning;
            }
        }

        protected abstract void ProcessEffect(float deltaTime);

        protected void Start()
        {
            this.ProcessEffect(this.timeRunning);
        }

        protected void Update()
        {
            this.timeRunning += Time.deltaTime;

            this.ProcessEffect(Time.deltaTime);
        }
    }
}
