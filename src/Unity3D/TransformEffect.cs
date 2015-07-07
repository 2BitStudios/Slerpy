using System;
using System.Collections.Generic;

using UnityEngine;

namespace Slerpy.Unity3D
{
    public enum TransformEffectPreset
    {
        Custom = 0,
        ShakeX = 1,
        ShakeY = 2,
        ShakeZ = 3,
        TwistX = 4,
        TwistY = 5,
        TwistZ = 6,
        Throb = 7,
        Raise = 8,
        Flip = 9,
        Expand = 10
    }

    public sealed class TransformEffect : Effect
    {
        private const TransformEffectPreset PRESET_DEFAULT = TransformEffectPreset.Custom;

        private static readonly Dictionary<TransformEffectPreset, PresetData> presetData = new Dictionary<TransformEffectPreset, PresetData>()
        {
            { TransformEffectPreset.ShakeX, new PresetData(0.1f, TimeWrapType.Cycle, new Vector3(0.1f, 0.0f, 0.0f), Vector3.zero, Vector3.zero) },
            { TransformEffectPreset.ShakeY, new PresetData(0.1f, TimeWrapType.Cycle, new Vector3(0.0f, 0.1f, 0.0f), Vector3.zero, Vector3.zero) },
            { TransformEffectPreset.ShakeZ, new PresetData(0.1f, TimeWrapType.Cycle, new Vector3(0.0f, 0.0f, 0.1f), Vector3.zero, Vector3.zero) },
            { TransformEffectPreset.TwistX, new PresetData(0.5f, TimeWrapType.Cycle, Vector3.zero, new Vector3(180.0f, 0.0f, 0.0f), Vector3.zero) },
            { TransformEffectPreset.TwistY, new PresetData(0.5f, TimeWrapType.Cycle, Vector3.zero, new Vector3(0.0f, 180.0f, 0.0f), Vector3.zero) },
            { TransformEffectPreset.TwistZ, new PresetData(0.5f, TimeWrapType.Cycle, Vector3.zero, new Vector3(0.0f, 0.0f, 180.0f), Vector3.zero) },
            { TransformEffectPreset.Throb, new PresetData(0.5f, TimeWrapType.Cycle, Vector3.zero, Vector3.zero, new Vector3(0.1f, 0.1f, 0.1f)) },
            { TransformEffectPreset.Raise, new PresetData(1.0f, TimeWrapType.Clamp, new Vector3(0.0f, 1.0f, 0.0f), Vector3.zero, Vector3.zero) },
            { TransformEffectPreset.Flip, new PresetData(1.0f, TimeWrapType.Clamp, Vector3.zero, new Vector3(180.0f, 0.0f, 0.0f), Vector3.zero) },
            { TransformEffectPreset.Expand, new PresetData(1.0f, TimeWrapType.Clamp, Vector3.zero, Vector3.zero, new Vector3(1.0f, 1.0f, 1.0f)) }
        };

        [SerializeField]
        [Tooltip("Pre-defined common settings for the values that follow.")]
        private TransformEffectPreset preset = PRESET_DEFAULT;

        [SerializeField]
        [Tooltip(Effect.TOOLTIP_DURATION)]
        private float duration = 1.0f;
        
        [SerializeField]
        [Tooltip(Effect.TOOLTIP_TIMEWRAP)]
        private TimeWrapType timeWrap = TimeWrapType.Cycle;

        [SerializeField]
        [Tooltip("Maximum local-space position change at a weight of 1.0. Can be exceeded or negated by weight modifiers or time wrap type.")]
        private Vector3 positionExtent = Vector3.zero;

        [SerializeField]
        [Tooltip("Maximum local-space rotation change at a weight of 1.0. Can be exceeded or negated by weight modifiers or time wrap type.")]
        private Vector3 rotationExtent = Vector3.zero;

        [SerializeField]
        [Tooltip("Maximum local-space scale change at a weight of 1.0. Can be exceeded or negated by weight modifiers or time wrap type.")]
        private Vector3 scaleExtent = Vector3.zero;

        [SerializeField]
        [HideInInspector]
        private TransformEffectPreset previousPreset = PRESET_DEFAULT;

        [SerializeField]
        [HideInInspector]
        private Vector3 positionOffset = Vector3.zero;

        [SerializeField]
        [HideInInspector]
        private Quaternion rotationOffset = Quaternion.identity;

        [SerializeField]
        [HideInInspector]
        private Vector3 scaleOffset = Vector3.zero;

        public TransformEffectPreset Preset
        {
            get
            {
                return this.preset;
            }

            set
            {
                this.preset = value;

                if (this.preset == TransformEffectPreset.Custom)
                {
                    foreach (KeyValuePair<TransformEffectPreset, PresetData> presetData in TransformEffect.presetData)
                    {
                        if (presetData.Value.CompareTo(this))
                        {
                            this.preset = presetData.Key;
                        }
                    }
                }
                else
                {
                    this.TrySetToPreset();
                }

                this.previousPreset = this.preset;
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
                if (this.duration != value)
                {
                    this.duration = value;

                    this.Preset = TransformEffectPreset.Custom;
                }
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
                if (this.timeWrap != value)
                {
                    this.timeWrap = value;

                    this.Preset = TransformEffectPreset.Custom;
                }
            }
        }

        public Vector3 PositionExtent
        {
            get
            {
                return this.positionExtent;
            }

            set
            {
                if (this.positionExtent != value)
                {
                    this.positionExtent = value;

                    this.Preset = TransformEffectPreset.Custom;
                }
            }
        }

        public Vector3 RotationExtent
        {
            get
            {
                return this.rotationExtent;
            }

            set
            {
                if (this.rotationExtent != value)
                {
                    this.rotationExtent = value;

                    this.Preset = TransformEffectPreset.Custom;
                }
            }
        }

        public Vector3 ScaleExtent
        {
            get
            {
                return this.scaleExtent;
            }

            set
            {
                if (this.scaleExtent != value)
                {
                    this.scaleExtent = value;

                    this.Preset = TransformEffectPreset.Custom;
                }
            }
        }

        public Vector3 PositionOffset
        {
            get
            {
                return this.positionOffset;
            }

            set
            {
                this.transform.localPosition -= this.positionOffset;

                this.positionOffset = value;

                this.transform.localPosition += this.positionOffset;
            }
        }

        public Quaternion RotationOffset
        {
            get
            {
                return this.rotationOffset;
            }

            set
            {
                this.transform.localRotation *= Quaternion.Inverse(this.rotationOffset);

                this.rotationOffset = value;

                this.transform.localRotation *= this.rotationOffset;
            }
        }

        public Vector3 ScaleOffset
        {
            get
            {
                return this.scaleOffset;
            }

            set
            {
                this.transform.localScale -= this.scaleOffset;

                this.scaleOffset = value;

                this.transform.localScale += this.scaleOffset;
            }
        }

        [ContextMenu("Reset Offsets")]
        public void ResetOffsets()
        {
            this.PositionOffset = Vector3.zero;
            this.RotationOffset = Quaternion.identity;
            this.ScaleOffset = Vector3.zero;
        }

        protected override void ProcessEffect(float weight, float strength)
        {
            this.PositionOffset = Extensions.Interpolate(
                Vector3.zero, 
                this.positionExtent * strength, 
                weight, 
                InterpolateType.Standard);

            this.RotationOffset = Quaternion.Euler(Extensions.Interpolate(
                Vector3.zero, 
                this.rotationExtent * strength, 
                weight,
                InterpolateType.Standard));

            this.ScaleOffset = Extensions.Interpolate(
                Vector3.zero, 
                this.scaleExtent * strength, 
                weight,
                InterpolateType.Standard);
        }

        private void TrySetToPreset()
        {
            PresetData data = default(PresetData);

            if (TransformEffect.presetData.TryGetValue(this.preset, out data))
            {
                data.SetTo(this);
            }
        }

        private void OnValidate()
        {
            if (this.previousPreset != TransformEffectPreset.Custom && this.previousPreset == this.preset)
            {
                this.preset = TransformEffectPreset.Custom;
            }

            this.Preset = this.preset;
        }

        public struct PresetData
        {
            private readonly float duration;

            private readonly TimeWrapType timeWrap;

            private readonly Vector3 positionExtent;
            private readonly Vector3 rotationExtent;
            private readonly Vector3 scaleExtent;

            public PresetData(float duration, TimeWrapType timeWrap, Vector3 positionExtent, Vector3 rotationExtent, Vector3 scaleExtent)
            {
                this.duration = duration;

                this.timeWrap = timeWrap;

                this.positionExtent = positionExtent;
                this.rotationExtent = rotationExtent;
                this.scaleExtent = scaleExtent;
            }

            public float Duration
            {
                get
                {
                    return this.duration;
                }
            }

            public TimeWrapType TimeWrap
            {
                get
                {
                    return this.timeWrap;
                }
            }

            public Vector3 PositionExtent
            {
                get
                {
                    return this.positionExtent;
                }
            }

            public Vector3 RotationExtent
            {
                get
                {
                    return this.rotationExtent;
                }
            }

            public Vector3 ScaleExtent
            {
                get
                {
                    return this.scaleExtent;
                }
            }

            public bool CompareTo(TransformEffect target)
            {
                return Mathf.Approximately(target.duration, this.duration)
                    && target.timeWrap == this.timeWrap
                    && target.positionExtent == this.positionExtent
                    && target.rotationExtent == this.rotationExtent
                    && target.scaleExtent == this.scaleExtent;
            }

            public void SetTo(TransformEffect target)
            {
                target.duration = this.duration;

                target.timeWrap = this.timeWrap;

                target.positionExtent = this.positionExtent;
                target.rotationExtent = this.rotationExtent;
                target.scaleExtent = this.scaleExtent;

                target.Preset = TransformEffectPreset.Custom;
            }
        }
    }
}
