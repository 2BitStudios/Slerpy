using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace Slerpy.Unity3D
{
    public sealed class ColorEffect : Effect
    {
        [SerializeField]
        [Tooltip(Effect.TOOLTIP_INTERPOLATE)]
        private InterpolateType interpolate = InterpolateType.Standard;

        [SerializeField]
        [Tooltip(Effect.TOOLTIP_DURATION)]
        private float duration = 1.0f;
        
        [SerializeField]
        [Tooltip(Effect.TOOLTIP_TIMEWRAP)]
        private TimeWrapType timeWrap = TimeWrapType.PingPong;

        [SerializeField]
        [Tooltip("Color property to set on materials. Ignored for UI Image color.\nDefault: _Color")]
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
            Color interpolatedColor = Extensions.Interpolate(
                this.fromColor, 
                this.toColor * strength, 
                weight, 
                this.interpolate);

            if (this.transform is RectTransform)
            {
                Image[] images = this.gameObject.GetComponents<Image>();
                for (int i = 0; i < images.Length; ++i)
                {
                    images[i].color = interpolatedColor;
                }
            }
            else
            {
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
}
