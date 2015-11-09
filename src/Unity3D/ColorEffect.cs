using System;
using UnityEngine;
using UnityEngine.UI;

namespace Slerpy.Unity3D
{
    public sealed class ColorEffect : Effect
    {
        private const string TOOLTIP_FROMCOLOR = "Color to blend from.";
        private const string TOOLTIP_TOCOLOR = "Color to blend towards.";

        public static Color CalculateColor(float weight, Color fromColor, Color toColor)
        {
            return Extensions.InterpolateColor(
                fromColor, 
                toColor, 
                weight,
                InterpolateType.Standard);
        }

        [SerializeField]
        [Tooltip(Effect.TOOLTIP_DURATION)]
        private float duration = 1.0f;
        
        [SerializeField]
        [Tooltip(Effect.TOOLTIP_TIMEWRAP)]
        private WrapType timeWrap = WrapType.PingPong;

        [SerializeField]
        [Tooltip("Color property to set on materials. Ignored for UI and light color.\nDefault: _Color")]
        private string materialProperty = "_Color";

        [SerializeField]
        private bool affectChildren = false;

        [SerializeField]
        [Tooltip(ColorEffect.TOOLTIP_FROMCOLOR)]
        private Color fromColor = Color.white;

        [SerializeField]
        [Tooltip(ColorEffect.TOOLTIP_TOCOLOR)]
        private Color toColor = Color.red;

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

        public override WrapType TimeWrap
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

        public bool AffectChildren
        {
            get
            {
                return this.affectChildren;
            }

            set
            {
                this.affectChildren = value;
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

        protected override void ProcessEffect(float weight)
        {
            Color interpolatedColor = ColorEffect.CalculateColor(weight, this.fromColor, this.toColor);

            if (this.transform is RectTransform)
            {
                Graphic[] graphics = this.GetTargetComponents<Graphic>();
                for (int i = 0; i < graphics.Length; ++i)
                {
                    graphics[i].color = interpolatedColor;
                }
            }
            else
            {
                Renderer[] renderers = this.GetTargetComponents<Renderer>();
                for (int i = 0; i < renderers.Length; ++i)
                {
                    Material[] materials = renderers[i].materials;
                    for (int k = 0; k < materials.Length; ++k)
                    {
                        materials[k].SetColor(materialProperty, interpolatedColor);
                    }
                }

                Light[] lights = this.GetTargetComponents<Light>();
                for (int i = 0; i < lights.Length; ++i)
                {
                    lights[i].color = interpolatedColor;
                }
            }
        }

        private TComponent[] GetTargetComponents<TComponent>()
            where TComponent : Component
        {
            return this.affectChildren ? this.gameObject.GetComponentsInChildren<TComponent>() : this.gameObject.GetComponents<TComponent>();
        }

        [Serializable]
        public sealed class Detachable : Detachable<Color>
        {
            [SerializeField]
            [Tooltip(ColorEffect.TOOLTIP_FROMCOLOR)]
            private Color fromColor = Color.white;

            [SerializeField]
            [Tooltip(ColorEffect.TOOLTIP_TOCOLOR)]
            private Color toColor = Color.red;

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

            protected override Color Internal_CalculateState(float weight)
            {
                return ColorEffect.CalculateColor(weight, this.fromColor, this.toColor);
            }
        }
    }
}
