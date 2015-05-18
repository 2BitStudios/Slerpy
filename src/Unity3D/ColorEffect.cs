using System;
using System.Collections.Generic;

using UnityEngine;

namespace Slerpy.Unity3D
{
    public sealed class ColorEffect : Effect
    {
        [SerializeField]
        [Tooltip(Effect.TOOLTIP_INTERPOLATE)]
        private InterpolateType interpolate = InterpolateType.Standard;

        [SerializeField]
        [Tooltip(Effect.TOOLTIP_CYCLETIME)]
        private float cycleTime = 1.0f;
        
        [SerializeField]
        [Tooltip(Effect.TOOLTIP_TIMEWRAP)]
        private TimeWrapType timeWrap = TimeWrapType.PingPong;

        [SerializeField]
        [Tooltip("Color property to set.\nDefault: _Color")]
        private string materialProperty = "_Color";

        [SerializeField]
        [Tooltip("Color to blend from.")]
        private Color fromColor = Color.white;

        [SerializeField]
        [Tooltip("Color to blend towards.")]
        private Color toColor = Color.red;

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

        public override float CycleTime
        {
            get
            {
                return this.cycleTime;
            }

            set
            {
                this.cycleTime = value;
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

        public string MaterialProperty
        {
            get
            {
                return this.materialProperty;
            }

            set
            {
                this.materialProperty = value;
            }
        }

        public Color FromColor
        {
            get
            {
                return this.fromColor;
            }

            set
            {
                this.fromColor = value;
            }
        }

        public Color ToColor
        {
            get
            {
                return this.toColor;
            }

            set
            {
                this.toColor = value;
            }
        }

        protected override void ProcessEffect(float weight, float strength)
        {
            Color interpolatedColor = new Color(
                Slerpy.Interpolate.WithType(this.interpolate, this.fromColor.r, this.toColor.r * strength, weight),
                Slerpy.Interpolate.WithType(this.interpolate, this.fromColor.g, this.toColor.g * strength, weight),
                Slerpy.Interpolate.WithType(this.interpolate, this.fromColor.b, this.toColor.b * strength, weight),
                Slerpy.Interpolate.WithType(this.interpolate, this.fromColor.a, this.toColor.a * strength, weight));

            Renderer[] renderers = this.gameObject.GetComponents<Renderer>();
            for (int i = 0; i < renderers.Length; ++i)
            {
                Material[] materials = renderers[i].materials;
                for (int k = 0; k < materials.Length; ++k)
                {
                    materials[k].SetColor(materialProperty, interpolatedColor);
                }
            }
        }
    }
}
