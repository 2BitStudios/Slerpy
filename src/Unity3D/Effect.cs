using System;
using System.Collections.Generic;

using UnityEngine;

namespace Slerpy.Unity3D
{
    public abstract class Effect : MonoBehaviour
    {
        [SerializeField]
        private bool randomiseStartTime = true;

        private float timeRunning = 0.0f;

        [SerializeField]
        private float timeRemaining = 0.0f;

        [SerializeField]
        private List<ChainedEffect> chainedEffects = null;

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
            if (this.randomiseStartTime)
            {
                this.timeRunning += UnityEngine.Random.value;
            }
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

        protected void LateUpdate()
        {
            for (int i = 0; i < this.chainedEffects.Count; ++i)
            {
                if (this.chainedEffects[i].Time <= this.timeRunning && (!this.chainedEffects[i].Loop || (this.timeRunning % this.chainedEffects[i].Time) <= Time.deltaTime))
                {
                    this.chainedEffects[i].Target.enabled = this.chainedEffects[i].State;

                    if (!this.chainedEffects[i].Loop)
                    {
                        this.chainedEffects.RemoveAt(i);
                        --i;
                    }
                }
            }
        }

        [Serializable]
        public sealed class ChainedEffect
        {
            [SerializeField]
            private float time = 0.0f;

            [SerializeField]
            private bool state = true;

            [SerializeField]
            private bool loop = false;

            [SerializeField]
            private Effect target = null;

            public float Time
            {
                get
                {
                    return this.time;
                }
            }

            public bool State
            {
                get
                {
                    return this.state;
                }
            }

            public bool Loop
            {
                get
                {
                    return this.loop;
                }
            }

            public Effect Target
            {
                get
                {
                    return this.target;
                }
            }
        }
    }
}
