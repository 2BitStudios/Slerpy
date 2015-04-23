using System;
using System.Collections.Generic;

using UnityEngine;

namespace Slerpy.Unity3D
{
    public enum TransformerPreset
    {
        Custom = 0,
        Shake = 1,
        Twist = 2,
        Throb = 3
    }

    public sealed class Transformer : Effect
    {
        private const TransformerPreset PRESET_DEFAULT = TransformerPreset.Custom;

        private static readonly Dictionary<TransformerPreset, PresetData> presetData = new Dictionary<TransformerPreset, PresetData>()
        {
            { TransformerPreset.Shake, new PresetData(8.0f, 1.0f, new Vector3(0.1f, 0.1f, 0.1f), Vector3.zero, Vector3.zero) },
            { TransformerPreset.Twist, new PresetData(1.0f, 1.0f, Vector3.zero, new Vector3(180.0f, 180.0f, 180.0f), Vector3.zero) },
            { TransformerPreset.Throb, new PresetData(1.0f, 1.0f, Vector3.zero, Vector3.zero, new Vector3(0.4f, 0.4f, 0.4f)) }
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
        private float rateModifier = 1.0f;

        [SerializeField]
        private float extentModifier = 1.0f;

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

        public float RateModifier
        {
            get
            {
                return this.rateModifier;
            }

            set
            {
                if (this.rateModifier != value)
                {
                    this.rateModifier = value;

                    this.Preset = TransformerPreset.Custom;
                }
            }
        }

        public float ExtentModifier
        {
            get
            {
                return this.extentModifier;
            }

            set
            {
                if (this.extentModifier != value)
                {
                    this.extentModifier = value;

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

        public void RestoreTransform()
        {
            this.PositionOffset = Vector3.zero;
            this.RotationOffset = Quaternion.identity;
            this.ScaleOffset = Vector3.zero;
        }

        protected override void ProcessEffect(float deltaTime)
        {
            float weight = Weight.FromTime(
                this.timeWrapType,
                this.TimeRunning * this.rateModifier,
                1.0f);

            for (int i = 0; i < this.weightTypes.Length; ++i)
            {
                weight = Weight.WithType(this.weightTypes[i], weight);
            }

            this.PositionOffset = new Vector3(
                Slerpy.Interpolate.Standard(0.0f, this.positionExtent.x * this.extentModifier, weight),
                Slerpy.Interpolate.Standard(0.0f, this.positionExtent.y * this.extentModifier, weight),
                Slerpy.Interpolate.Standard(0.0f, this.positionExtent.z * this.extentModifier, weight));

            this.RotationOffset = Quaternion.Euler(new Vector3(
                Slerpy.Interpolate.Standard(0.0f, this.rotationExtent.x * this.extentModifier, weight),
                Slerpy.Interpolate.Standard(0.0f, this.rotationExtent.y * this.extentModifier, weight),
                Slerpy.Interpolate.Standard(0.0f, this.rotationExtent.z * this.extentModifier, weight)));

            this.ScaleOffset = new Vector3(
                Slerpy.Interpolate.Standard(0.0f, this.scaleExtent.x * this.extentModifier, weight),
                Slerpy.Interpolate.Standard(0.0f, this.scaleExtent.y * this.extentModifier, weight),
                Slerpy.Interpolate.Standard(0.0f, this.scaleExtent.z * this.extentModifier, weight));
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

            private readonly float rateModifier;
            private readonly float extentModifier;

            private readonly Vector3 positionExtent;
            private readonly Vector3 rotationExtent;
            private readonly Vector3 scaleExtent;

            public PresetData(float rateModifier, float extentModifier, Vector3 positionExtent, Vector3 rotationExtent, Vector3 scaleExtent)
            {
                this.rateModifier = rateModifier;
                this.extentModifier = extentModifier;

                this.positionExtent = positionExtent;
                this.rotationExtent = rotationExtent;
                this.scaleExtent = scaleExtent;
            }

            public float RateModifier
            {
                get
                {
                    return this.rateModifier;
                }
            }

            public float ExtentModifier
            {
                get
                {
                    return this.extentModifier;
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
                return Mathf.Approximately(target.rateModifier, this.rateModifier)
                    && Mathf.Approximately(target.extentModifier, this.extentModifier)
                    && target.positionExtent == this.positionExtent
                    && target.rotationExtent == this.rotationExtent
                    && target.scaleExtent == this.scaleExtent;
            }

            public void SetTo(Transformer target)
            {
                target.rateModifier = this.rateModifier;
                target.extentModifier = this.extentModifier;

                target.positionExtent = this.positionExtent;
                target.rotationExtent = this.rotationExtent;
                target.scaleExtent = this.scaleExtent;
            }
        }
    }
}
