using System;
using System.Collections.Generic;

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
            return Interpolate.Color(
                fromColor, 
                toColor, 
                weight);
        }

        [SerializeField]
        [Tooltip(Effect.TOOLTIP_DURATION)]
        private float duration = 1.0f;
        
        [SerializeField]
        [Tooltip(Effect.TOOLTIP_TIMEWRAP)]
        private WrapType timeWrap = WrapType.PingPong;

        [SerializeField]
        [Tooltip("Color property to set on materials. Ignored for UI and Light color.\nDefault: _Color")]
        private string materialProperty = "_Color";

        [SerializeField]
        [Tooltip("Whether to modify color of components on child objects (performance penalty).")]
        private bool affectChildren = false;

        [SerializeField]
        [Tooltip("Whether to modify color of Light components (performance penalty). Ignored when on UI objects.")]
        private bool affectLights = false;

        [SerializeField]
        [Tooltip(ColorEffect.TOOLTIP_FROMCOLOR)]
        private Color fromColor = Color.white;

        [SerializeField]
        [Tooltip(ColorEffect.TOOLTIP_TOCOLOR)]
        private Color toColor = Color.red;

        private List<Graphic> graphicComponentCache = null;
        private List<Renderer> rendererComponentCache = null;
        private List<Light> lightComponentCache = null;

        public override float Duration
        {
            get
            {
                return this.duration;
            }

            set
            {
                if (this.duration != value)
                {
                    this.duration = value;

                    this.Refresh();
                }
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
                if (this.timeWrap != value)
                {
                    this.timeWrap = value;

                    this.Refresh();
                }
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
                if (this.materialProperty != value)
                {
                    this.materialProperty = value;

                    this.Refresh();
                }
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
                if (this.affectChildren != value)
                {
                    this.affectChildren = value;

                    this.Refresh();
                }
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
                if (this.fromColor != value)
                {
                    this.fromColor = value;

                    this.Refresh();
                }
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
                if (this.toColor != value)
                {
                    this.toColor = value;

                    this.Refresh();
                }
            }
        }

        protected override void ProcessEffect(float weight)
        {
            Color interpolatedColor = ColorEffect.CalculateColor(weight, this.fromColor, this.toColor);

            if (this.transform is RectTransform)
            {
                if (this.graphicComponentCache == null)
                {
                    this.graphicComponentCache = new List<Graphic>();
                }

                this.GetTargetComponents<Graphic>(this.graphicComponentCache);
                for (int i = 0; i < this.graphicComponentCache.Count; ++i)
                {
                    this.graphicComponentCache[i].color = interpolatedColor;
                }
            }
            else
            {
                if (this.rendererComponentCache == null)
                {
                    this.rendererComponentCache = new List<Renderer>();
                }

                this.GetTargetComponents<Renderer>(this.rendererComponentCache);
                for (int i = 0; i < this.rendererComponentCache.Count; ++i)
                {
                    Material[] materials = this.rendererComponentCache[i].materials;
                    for (int k = 0; k < materials.Length; ++k)
                    {
                        materials[k].SetColor(materialProperty, interpolatedColor);
                    }
                }

                if (this.affectLights)
                {
                    if (this.lightComponentCache == null)
                    {
                        this.lightComponentCache = new List<Light>();
                    }

                    this.GetTargetComponents<Light>(this.lightComponentCache);
                    for (int i = 0; i < this.lightComponentCache.Count; ++i)
                    {
                        this.lightComponentCache[i].color = interpolatedColor;
                    }
                }
            }
        }

        private void GetTargetComponents<TComponent>(List<TComponent> componentCache)
            where TComponent : Component
        {
            if (this.affectChildren)
            {
                this.gameObject.GetComponentsInChildren<TComponent>(componentCache);
            }
            else
            {
                this.gameObject.GetComponents<TComponent>(componentCache);
            }
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
