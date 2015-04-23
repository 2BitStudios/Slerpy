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
        Throb = 7
    }

    public sealed class Transformer : Effect
    {
        private const TransformerPreset PRESET_DEFAULT = TransformerPreset.Custom;

        private static readonly Dictionary<TransformerPreset, PresetData> presetData = new Dictionary<TransformerPreset, PresetData>()
        {
            { TransformerPreset.ShakeX, new PresetData(12.0f, 1.0f, new Vector3(0.1f, 0.0f, 0.0f), Vector3.zero, Vector3.zero) },
            { TransformerPreset.ShakeY, new PresetData(12.0f, 1.0f, new Vector3(0.0f, 0.1f, 0.0f), Vector3.zero, Vector3.zero) },
            { TransformerPreset.ShakeZ, new PresetData(12.0f, 1.0f, new Vector3(0.0f, 0.0f, 0.1f), Vector3.zero, Vector3.zero) },
            { TransformerPreset.TwistX, new PresetData(2.0f, 1.0f, Vector3.zero, new Vector3(180.0f, 0.0f, 0.0f), Vector3.zero) },
            { TransformerPreset.TwistY, new PresetData(2.0f, 1.0f, Vector3.zero, new Vector3(0.0f, 180.0f, 0.0f), Vector3.zero) },
            { TransformerPreset.TwistZ, new PresetData(2.0f, 1.0f, Vector3.zero, new Vector3(0.0f, 0.0f, 180.0f), Vector3.zero) },
            { TransformerPreset.Throb, new PresetData(2.0f, 1.0f, Vector3.zero, Vector3.zero, new Vector3(0.1f, 0.1f, 0.1f)) }
        };

        [SerializeField]
        private TransformerPreset preset = PRESET_DEFAULT;

        [SerializeField]
        private WeightType[] weightTypes = null;

        [SerializeField]
        private TimeWrapType timeWrapType = TimeWrapType.Clamp;

        [SerializeField]
        private InterpolateType interpolateType = InterpolateType.Standard;

        [SerializeField]
        private bool restoreTransformOnDestruction = false;

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

        public TransformerPreset Preset
        {
            get
            {
                return this.preset;
            }

            set
            {
                this.preset = value;

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

                this.previousPreset = this.preset;
            }
        }

        public IEnumerable<WeightType> WeightTypes
        {
            get
            {
                return this.weightTypes;
            }
        }

        public TimeWrapType TimeWrapType
        {
            get
            {
                return this.timeWrapType;
            }

            set
            {
                if (this.timeWrapType != value)
                {
                    this.timeWrapType = value;

                    this.Preset = TransformerPreset.Custom;
                }
            }
        }

        public InterpolateType InterpolateType
        {
            get
            {
                return this.interpolateType;
            }

            set
            {
                if (this.interpolateType != value)
                {
                    this.interpolateType = value;

                    this.Preset = TransformerPreset.Custom;
                }
            }
        }

        public bool RestoreTransformOnDestruction
        {
            get
            {
                return this.restoreTransformOnDestruction;
            }

            set
            {
                this.restoreTransformOnDestruction = value;
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

        public void RestoreTransform()
        {
            this.PositionOffset = Vector3.zero;
            this.RotationOffset = Quaternion.identity;
            this.ScaleOffset = Vector3.zero;
        }

        protected override void ProcessEffect(float deltaTime, float totalTime, float strength)
        {
            float weight = Weight.FromTime(
                this.timeWrapType,
                totalTime,
                1.0f);

            for (int i = 0; i < this.weightTypes.Length; ++i)
            {
                weight = Weight.WithType(this.weightTypes[i], weight);
            }

            this.PositionOffset = new Vector3(
                Slerpy.Interpolate.Standard(0.0f, this.positionExtent.x * strength, weight),
                Slerpy.Interpolate.Standard(0.0f, this.positionExtent.y * strength, weight),
                Slerpy.Interpolate.Standard(0.0f, this.positionExtent.z * strength, weight));

            this.RotationOffset = Quaternion.Euler(new Vector3(
                Slerpy.Interpolate.Standard(0.0f, this.rotationExtent.x * strength, weight),
                Slerpy.Interpolate.Standard(0.0f, this.rotationExtent.y * strength, weight),
                Slerpy.Interpolate.Standard(0.0f, this.rotationExtent.z * strength, weight)));

            this.ScaleOffset = new Vector3(
                Slerpy.Interpolate.Standard(0.0f, this.scaleExtent.x * strength, weight),
                Slerpy.Interpolate.Standard(0.0f, this.scaleExtent.y * strength, weight),
                Slerpy.Interpolate.Standard(0.0f, this.scaleExtent.z * strength, weight));
        }

        protected override void OnRateChanged(float oldRate, float newRate)
        {
            this.Preset = TransformerPreset.Custom;
        }

        protected override void OnStrengthChanged(float oldStrength, float newStrength)
        {
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

        private void OnDestroy()
        {
            if (this.restoreTransformOnDestruction)
            {
                this.RestoreTransform();
            }
        }

        public struct PresetData
        {
            public static PresetData Default
            {
                get
                {
                    return new PresetData(0.0f, 0.0f, Vector3.zero, Vector3.zero, Vector3.zero);
                }
            }

            private readonly float rate;
            private readonly float strength;

            private readonly Vector3 positionExtent;
            private readonly Vector3 rotationExtent;
            private readonly Vector3 scaleExtent;

            public PresetData(float rateModifier, float strengthModifier, Vector3 positionExtent, Vector3 rotationExtent, Vector3 scaleExtent)
            {
                this.rate = rateModifier;
                this.strength = strengthModifier;

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
                return Mathf.Approximately(target.Rate, this.rate)
                    && Mathf.Approximately(target.Strength, this.strength)
                    && target.positionExtent == this.positionExtent
                    && target.rotationExtent == this.rotationExtent
                    && target.scaleExtent == this.scaleExtent;
            }

            public void SetTo(Transformer target)
            {
                target.Rate = this.rate;
                target.Strength = this.strength;

                target.positionExtent = this.positionExtent;
                target.rotationExtent = this.rotationExtent;
                target.scaleExtent = this.scaleExtent;
            }
        }
    }
}
