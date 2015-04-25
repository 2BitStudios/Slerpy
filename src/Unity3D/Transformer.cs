using System;
using System.Collections.Generic;

using UnityEngine;

namespace Slerpy.Unity3D
{
    public enum TransformerPreset
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

    public sealed class Transformer : Effect
    {
        private const TransformerPreset PRESET_DEFAULT = TransformerPreset.Custom;

        private static readonly Dictionary<TransformerPreset, PresetData> presetData = new Dictionary<TransformerPreset, PresetData>()
        {
            { TransformerPreset.ShakeX, new PresetData(TimeWrapType.Cycle, 12.0f, 1.0f, new Vector3(0.1f, 0.0f, 0.0f), Vector3.zero, Vector3.zero) },
            { TransformerPreset.ShakeY, new PresetData(TimeWrapType.Cycle, 12.0f, 1.0f, new Vector3(0.0f, 0.1f, 0.0f), Vector3.zero, Vector3.zero) },
            { TransformerPreset.ShakeZ, new PresetData(TimeWrapType.Cycle, 12.0f, 1.0f, new Vector3(0.0f, 0.0f, 0.1f), Vector3.zero, Vector3.zero) },
            { TransformerPreset.TwistX, new PresetData(TimeWrapType.Cycle, 2.0f, 1.0f, Vector3.zero, new Vector3(180.0f, 0.0f, 0.0f), Vector3.zero) },
            { TransformerPreset.TwistY, new PresetData(TimeWrapType.Cycle, 2.0f, 1.0f, Vector3.zero, new Vector3(0.0f, 180.0f, 0.0f), Vector3.zero) },
            { TransformerPreset.TwistZ, new PresetData(TimeWrapType.Cycle, 2.0f, 1.0f, Vector3.zero, new Vector3(0.0f, 0.0f, 180.0f), Vector3.zero) },
            { TransformerPreset.Throb, new PresetData(TimeWrapType.Cycle, 2.0f, 1.0f, Vector3.zero, Vector3.zero, new Vector3(0.1f, 0.1f, 0.1f)) },
            { TransformerPreset.Raise, new PresetData(TimeWrapType.Clamp, 1.0f, 1.0f, new Vector3(0.0f, 1.0f, 0.0f), Vector3.zero, Vector3.zero) },
            { TransformerPreset.Flip, new PresetData(TimeWrapType.Clamp, 1.0f, 1.0f, Vector3.zero, new Vector3(180.0f, 0.0f, 0.0f), Vector3.zero) },
            { TransformerPreset.Expand, new PresetData(TimeWrapType.Clamp, 1.0f, 1.0f, Vector3.zero, Vector3.zero, new Vector3(1.0f, 1.0f, 1.0f)) }
        };

        [SerializeField]
        private TransformerPreset preset = PRESET_DEFAULT;

        [SerializeField]
        private float cycleTime = 1.0f;

        [SerializeField]
        private WeightType[] weights = new WeightType[] { WeightType.Linear };

        [SerializeField]
        private TimeWrapType timeWrap = TimeWrapType.Cycle;

        [SerializeField]
        private InterpolateType interpolate = InterpolateType.Standard;

        [SerializeField]
        private Vector3 positionExtent = Vector3.zero;

        [SerializeField]
        private Vector3 rotationExtent = Vector3.zero;

        [SerializeField]
        private Vector3 scaleExtent = Vector3.zero;

        [SerializeField]
        [HideInInspector]
        private TransformerPreset previousPreset = PRESET_DEFAULT;

        private Vector3 positionOffset = Vector3.zero;
        private Quaternion rotationOffset = Quaternion.identity;
        private Vector3 scaleOffset = Vector3.zero;

        private bool autodetectPresetValues = true;

        public TransformerPreset Preset
        {
            get
            {
                return this.preset;
            }

            set
            {
                this.preset = value;

                if (this.autodetectPresetValues)
                {
                    if (this.preset == TransformerPreset.Custom)
                    {
                        foreach (KeyValuePair<TransformerPreset, PresetData> presetData in Transformer.presetData)
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
                }

                this.previousPreset = this.preset;
            }
        }

        public float CycleTime
        {
            get
            {
                return this.cycleTime;
            }
        }

        public IEnumerable<WeightType> Weights
        {
            get
            {
                return this.weights;
            }
        }

        public TimeWrapType TimeWrap
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

                    this.Preset = TransformerPreset.Custom;
                }
            }
        }

        public InterpolateType Interpolate
        {
            get
            {
                return this.interpolate;
            }

            set
            {
                if (this.interpolate != value)
                {
                    this.interpolate = value;

                    this.Preset = TransformerPreset.Custom;
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

                    this.Preset = TransformerPreset.Custom;
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

                    this.Preset = TransformerPreset.Custom;
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

                    this.Preset = TransformerPreset.Custom;
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
                this.transform.position -= this.positionOffset;

                this.positionOffset = value;

                this.transform.position += this.positionOffset;
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
                this.transform.rotation *= Quaternion.Inverse(this.rotationOffset);

                this.rotationOffset = value;

                this.transform.rotation *= this.rotationOffset;
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

        public void ResetOffsets()
        {
            this.PositionOffset = Vector3.zero;
            this.RotationOffset = Quaternion.identity;
            this.ScaleOffset = Vector3.zero;
        }

        protected override void ProcessEffect(float deltaTime, float totalTime, float strength)
        {
            float weight = Weight.FromTime(
                this.timeWrap,
                totalTime,
                this.cycleTime);

            for (int i = 0; i < this.weights.Length; ++i)
            {
                weight = Weight.WithType(this.weights[i], weight);
            }

            this.PositionOffset = new Vector3(
                Slerpy.Interpolate.WithType(this.interpolate, 0.0f, this.positionExtent.x * strength, weight),
                Slerpy.Interpolate.WithType(this.interpolate, 0.0f, this.positionExtent.y * strength, weight),
                Slerpy.Interpolate.WithType(this.interpolate, 0.0f, this.positionExtent.z * strength, weight));

            this.RotationOffset = Quaternion.Euler(new Vector3(
                Slerpy.Interpolate.WithType(this.interpolate, 0.0f, this.rotationExtent.x * strength, weight),
                Slerpy.Interpolate.WithType(this.interpolate, 0.0f, this.rotationExtent.y * strength, weight),
                Slerpy.Interpolate.WithType(this.interpolate, 0.0f, this.rotationExtent.z * strength, weight)));

            this.ScaleOffset = new Vector3(
                Slerpy.Interpolate.WithType(this.interpolate, 0.0f, this.scaleExtent.x * strength, weight),
                Slerpy.Interpolate.WithType(this.interpolate, 0.0f, this.scaleExtent.y * strength, weight),
                Slerpy.Interpolate.WithType(this.interpolate, 0.0f, this.scaleExtent.z * strength, weight));
        }

        protected override void OnRateChanged(float oldRate, float newRate)
        {
            base.OnRateChanged(oldRate, newRate);

            this.Preset = TransformerPreset.Custom;
        }

        protected override void OnStrengthChanged(float oldStrength, float newStrength)
        {
            base.OnStrengthChanged(oldStrength, newStrength);

            this.Preset = TransformerPreset.Custom;
        }

        private void TrySetToPreset()
        {
            PresetData data = default(PresetData);

            if (Transformer.presetData.TryGetValue(this.preset, out data))
            {
                data.SetTo(this);
            }
        }

        private void OnValidate()
        {
            if (this.previousPreset != TransformerPreset.Custom && this.previousPreset == this.preset)
            {
                this.preset = TransformerPreset.Custom;
            }

            this.Preset = this.preset;
        }

        public struct PresetData
        {
            private TimeWrapType timeWrap;

            private readonly float rate;
            private readonly float strength;

            private readonly Vector3 positionExtent;
            private readonly Vector3 rotationExtent;
            private readonly Vector3 scaleExtent;

            public PresetData(TimeWrapType timeWrap, float rate, float strength, Vector3 positionExtent, Vector3 rotationExtent, Vector3 scaleExtent)
            {
                this.timeWrap = timeWrap;

                this.rate = rate;
                this.strength = strength;

                this.positionExtent = positionExtent;
                this.rotationExtent = rotationExtent;
                this.scaleExtent = scaleExtent;
            }

            public float Rate
            {
                get
                {
                    return this.rate;
                }
            }

            public float Strength
            {
                get
                {
                    return this.strength;
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

            public bool CompareTo(Transformer target)
            {
                return target.timeWrap == this.timeWrap
                    && Mathf.Approximately(target.UnscaledRate, this.rate)
                    && Mathf.Approximately(target.UnscaledStrength, this.strength)
                    && target.positionExtent == this.positionExtent
                    && target.rotationExtent == this.rotationExtent
                    && target.scaleExtent == this.scaleExtent;
            }

            public void SetTo(Transformer target)
            {
                target.autodetectPresetValues = false;

                target.timeWrap = this.timeWrap;

                target.UnscaledRate = this.rate;
                target.UnscaledStrength = this.strength;

                target.positionExtent = this.positionExtent;
                target.rotationExtent = this.rotationExtent;
                target.scaleExtent = this.scaleExtent;

                target.autodetectPresetValues = true;

                target.Preset = TransformerPreset.Custom;
            }
        }
    }
}
