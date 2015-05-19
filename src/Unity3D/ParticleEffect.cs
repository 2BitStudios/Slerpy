using System;
using System.Collections.Generic;

using UnityEngine;

namespace Slerpy.Unity3D
{
    [RequireComponent(typeof(ParticleSystem))]
    public sealed class ParticleEffect : Effect
    {
        [SerializeField]
        [Tooltip(Effect.TOOLTIP_INTERPOLATE)]
        private InterpolateType interpolate = InterpolateType.Standard;

        [SerializeField]
        [Tooltip(Effect.TOOLTIP_DURATION)]
        private float duration = 1.0f;

        [SerializeField]
        [Tooltip(Effect.TOOLTIP_TIMEWRAP)]
        private TimeWrapType timeWrap = TimeWrapType.Clamp;

        private ParticleSystem particleSystemComponent = null;

        private float emitCount = 0.0f;

        public override InterpolateType Interpolate
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

        public override float Duration
        {
            get
            {
                return this.duration;
            }

            set
            {
                this.duration = value;
            }
        }

        public override TimeWrapType TimeWrap
        {
            get
            {
                return this.timeWrap;
            }

            set
            {
                this.timeWrap = value;
            }
        }

        protected override void ProcessEffect(float deltaTime, float weight, float strength)
        {
            float frameEmitCount = this.particleSystemComponent.emissionRate * deltaTime * weight * strength;

            this.particleSystemComponent.Emit((int)(frameEmitCount + emitCount) - (int)emitCount);

            emitCount += frameEmitCount;

            emitCount %= this.particleSystemComponent.emissionRate * 2.0f;
        }

        protected override void Start()
        {
            this.particleSystemComponent = this.gameObject.GetComponent<ParticleSystem>();

            base.Start();
        }
    }
}
