using System;
using System.Collections.Generic;

using UnityEngine;

namespace Slerpy.Unity3D
{
    public sealed class Transformer : Effect
    {
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

        private Vector3 positionOffset = Vector3.zero;
        private Quaternion rotationOffset = Quaternion.identity;
        private Vector3 scaleOffset = Vector3.zero;

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
                this.timeWrapType = value;
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
                this.interpolateType = value;
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
                this.rateModifier = value;
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
                this.extentModifier = value;
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

        protected void OnDestroy()
        {
            if (this.restoreTransformOnDestruction)
            {
                this.PositionOffset = Vector3.zero;
                this.RotationOffset = Quaternion.identity;
                this.ScaleOffset = Vector3.zero;
            }
        }
    }
}
