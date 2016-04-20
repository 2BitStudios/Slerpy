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
        Expand = 10,
        RotateX = 11,
        RotateY = 12,
        RotateZ = 13
    }

    public sealed class TransformEffect : Effect
    {
        private const string TOOLTIP_POSITIONEXTENT = "Maximum local-space position change at a weight of 1.0. Can be exceeded or inverted by weight modifiers or time wrap type.";
        private const string TOOLTIP_ROTATIONEXTENT = "Maximum local-space rotation change at a weight of 1.0. Can be exceeded or inverted by weight modifiers or time wrap type.";
        private const string TOOLTIP_SCALEEXTENT = "Maximum local-space scale change at a weight of 1.0. Can be exceeded or inverted by weight modifiers or time wrap type.";

        private const TransformEffectPreset DEFAULT_PRESET = TransformEffectPreset.Custom;

        private static readonly Dictionary<TransformEffectPreset, PresetData> presetData = new Dictionary<TransformEffectPreset, PresetData>()
        {
            { TransformEffectPreset.ShakeX, new PresetData(0.1f, WrapType.Cycle, new Vector3(0.1f, 0.0f, 0.0f), Vector3.zero, Vector3.zero) },
            { TransformEffectPreset.ShakeY, new PresetData(0.1f, WrapType.Cycle, new Vector3(0.0f, 0.1f, 0.0f), Vector3.zero, Vector3.zero) },
            { TransformEffectPreset.ShakeZ, new PresetData(0.1f, WrapType.Cycle, new Vector3(0.0f, 0.0f, 0.1f), Vector3.zero, Vector3.zero) },
            { TransformEffectPreset.TwistX, new PresetData(0.5f, WrapType.Cycle, Vector3.zero, new Vector3(180.0f, 0.0f, 0.0f), Vector3.zero) },
            { TransformEffectPreset.TwistY, new PresetData(0.5f, WrapType.Cycle, Vector3.zero, new Vector3(0.0f, 180.0f, 0.0f), Vector3.zero) },
            { TransformEffectPreset.TwistZ, new PresetData(0.5f, WrapType.Cycle, Vector3.zero, new Vector3(0.0f, 0.0f, 180.0f), Vector3.zero) },
            { TransformEffectPreset.Throb, new PresetData(0.5f, WrapType.Cycle, Vector3.zero, Vector3.zero, new Vector3(0.1f, 0.1f, 0.1f)) },
            { TransformEffectPreset.Raise, new PresetData(1.0f, WrapType.Clamp, new Vector3(0.0f, 1.0f, 0.0f), Vector3.zero, Vector3.zero) },
            { TransformEffectPreset.Flip, new PresetData(1.0f, WrapType.Clamp, Vector3.zero, new Vector3(180.0f, 0.0f, 0.0f), Vector3.zero) },
            { TransformEffectPreset.Expand, new PresetData(1.0f, WrapType.Clamp, Vector3.zero, Vector3.zero, new Vector3(1.0f, 1.0f, 1.0f)) },
            { TransformEffectPreset.RotateX, new PresetData(1.0f, WrapType.Repeat, Vector3.zero, new Vector3(360.0f, 0.0f, 0.0f), Vector3.zero) },
            { TransformEffectPreset.RotateY, new PresetData(1.0f, WrapType.Repeat, Vector3.zero, new Vector3(0.0f, 360.0f, 0.0f), Vector3.zero) },
            { TransformEffectPreset.RotateZ, new PresetData(1.0f, WrapType.Repeat, Vector3.zero, new Vector3(0.0f, 0.0f, 360.0f), Vector3.zero) }
        };

        public static Vector3 CalculatePositionOffset(float weight, Vector3 extent)
        {
            return Interpolate.Vector3(
                Vector3.zero, 
                extent, 
                weight);
        }

        public static Quaternion CalculateRotationOffset(float weight, Vector3 extent)
        {
            return Quaternion.Euler(Interpolate.Vector3(
                Vector3.zero,
                extent, 
                weight));
        }

        public static Vector3 CalculateScaleOffset(float weight, Vector3 extent)
        {
            return Interpolate.Vector3(
                Vector3.zero,
                extent, 
                weight);
        }

        public static Matrix4x4 CalculateOffsets(float weight, Vector3 positionExtent, Vector3 rotationExtent, Vector3 scaleExtent)
        {
            return Matrix4x4.TRS(
                TransformEffect.CalculatePositionOffset(weight, positionExtent),
                TransformEffect.CalculateRotationOffset(weight, rotationExtent),
                Vector3.one + TransformEffect.CalculateScaleOffset(weight, scaleExtent));
        }

        [SerializeField]
        [Tooltip("Strength of effect. For example, an effect that moves the object would move twice as far with a strength of 2.0.")]
        private float strength = 1.0f;

        [SerializeField]
        [Tooltip("Pre-defined common settings for the values that follow.")]
        private TransformEffectPreset preset = DEFAULT_PRESET;

        [SerializeField]
        [Tooltip(Effect.TOOLTIP_DURATION)]
        private float duration = 1.0f;
        
        [SerializeField]
        [Tooltip(Effect.TOOLTIP_TIMEWRAP)]
        private WrapType timeWrap = WrapType.Cycle;

        [SerializeField]
        [Tooltip(TransformEffect.TOOLTIP_POSITIONEXTENT)]
        private Vector3 positionExtent = Vector3.zero;

        [SerializeField]
        [Tooltip(TransformEffect.TOOLTIP_ROTATIONEXTENT)]
        private Vector3 rotationExtent = Vector3.zero;

        [SerializeField]
        [Tooltip(TransformEffect.TOOLTIP_SCALEEXTENT)]
        private Vector3 scaleExtent = Vector3.zero;

        [SerializeField]
        [HideInInspector]
        private TransformEffectPreset previousPreset = DEFAULT_PRESET;

        [SerializeField]
        [HideInInspector]
        private Vector3 positionOffset = Vector3.zero;

        [SerializeField]
        [HideInInspector]
        private Quaternion rotationOffset = Quaternion.identity;

        [SerializeField]
        [HideInInspector]
        private Vector3 scaleOffset = Vector3.zero;

        public float Strength
        {
            get
            {
                return this.strength;
            }

            set
            {
                this.strength = value;
            }
        }

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

            private set
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

            private set
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

            private set
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

        protected override void ProcessEffect(float weight)
        {
            this.PositionOffset = TransformEffect.CalculatePositionOffset(weight, this.positionExtent * this.strength);
            this.RotationOffset = TransformEffect.CalculateRotationOffset(weight, this.rotationExtent * this.strength);
            this.ScaleOffset = TransformEffect.CalculateScaleOffset(weight, this.scaleExtent * this.strength);
        }

        private void TrySetToPreset()
        {
            PresetData data = default(PresetData);

            if (TransformEffect.presetData.TryGetValue(this.preset, out data))
            {
                data.SetTo(this);
            }
        }

        protected override void OnValidate()
        {
            base.OnValidate();

            if (this.previousPreset != TransformEffectPreset.Custom && this.previousPreset == this.preset)
            {
                this.preset = TransformEffectPreset.Custom;
            }

            this.Preset = this.preset;
        }

        public struct PresetData
        {
            private readonly float duration;

            private readonly WrapType timeWrap;

            private readonly Vector3 positionExtent;
            private readonly Vector3 rotationExtent;
            private readonly Vector3 scaleExtent;

            public PresetData(float duration, WrapType timeWrap, Vector3 positionExtent, Vector3 rotationExtent, Vector3 scaleExtent)
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

            public WrapType TimeWrap
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

        [Serializable]
        public sealed class Detachable : Effect.Detachable<Matrix4x4>
        {
            [SerializeField]
            [Tooltip(TransformEffect.TOOLTIP_POSITIONEXTENT)]
            private Vector3 positionExtent = Vector3.zero;

            [SerializeField]
            [Tooltip(TransformEffect.TOOLTIP_ROTATIONEXTENT)]
            private Vector3 rotationExtent = Vector3.zero;

            [SerializeField]
            [Tooltip(TransformEffect.TOOLTIP_SCALEEXTENT)]
            private Vector3 scaleExtent = Vector3.zero;

            public Vector3 PositionExtent
            {
                get
                {
                    return this.positionExtent;
                }

                set
                {
                    this.positionExtent = value;
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
                    this.rotationExtent = value;
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
                    this.scaleExtent = value;
                }
            }

            protected override Matrix4x4 Internal_CalculateState(float weight)
            {
                return TransformEffect.CalculateOffsets(weight, this.positionExtent, this.rotationExtent, this.scaleExtent);
            }
        }
    }
}
