using System;

using UnityEngine;

namespace Slerpy.Unity3D
{
    public enum InterpolationTarget
    {
        Position = 0,
        Rotation = 1,
        Scale = 2
    }

    public sealed class Interpolation : Effect
    {
        [SerializeField]
        private InterpolationTarget target = InterpolationTarget.Position;

        [SerializeField]
        private WeightType weightType = WeightType.Linear;

        [SerializeField]
        private TimeWrapType timeWrapType = TimeWrapType.Clamp;

        [SerializeField]
        private InterpolateType interpolateType = InterpolateType.Standard;

        [SerializeField]
        private float rateModifier = 1.0f;

        [SerializeField]
        private float extentModifier = 1.0f;

        [SerializeField]
        private Vector3 extent = Vector3.zero;

        private Vector3 positionOffset = Vector3.zero;
        private Quaternion rotationOffset = Quaternion.identity;
        private Vector3 scaleOffset = Vector3.zero;

        public WeightType WeightType
        {
            get
            {
                return this.weightType;
            }

            set
            {
                this.weightType = value;
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

            set
            {
                this.extentModifier = value;
            }
        }

        public Vector3 Extent
        {
            get
            {
                return this.extent;
            }

            set
            {
                this.extent = value;
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
            float weight = Weight.WithType(
                this.weightType, 
                Weight.FromTime(
                    this.timeWrapType,
                    this.TimeRunning * this.rateModifier,
                    1.0f));

            Vector3 offset = new Vector3(
                Slerpy.Interpolate.Standard(0.0f, this.extent.x * this.extentModifier, weight),
                Slerpy.Interpolate.Standard(0.0f, this.extent.y * this.extentModifier, weight),
                Slerpy.Interpolate.Standard(0.0f, this.extent.z * this.extentModifier, weight));

            switch (target)
            {
                case InterpolationTarget.Position:
                    this.PositionOffset = offset;
                    break;
                case InterpolationTarget.Rotation:
                    this.RotationOffset = Quaternion.Euler(offset);
                    break;
                case InterpolationTarget.Scale:
                    this.ScaleOffset = offset;
                    break;
            }
        }
    }
}
