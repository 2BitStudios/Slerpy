using System;

using UnityEngine;

namespace Slerpy.Unity3D
{
    public abstract class Offset : Effect
    {
        private Slerpy.Transform offset = new Slerpy.Transform(
            new Slerpy.Vector3D(0.0f, 0.0f, 0.0f),
            new Slerpy.Quaternion(0.0f, 0.0f, 0.0f, 1.0f),
            new Slerpy.Vector3D(0.0f, 0.0f, 0.0f));

        [SerializeField]
        private bool restoreTransformOnDestruction = true;

        [SerializeField]
        private bool allowInversion = false;

        [SerializeField]
        private SerializableAxisWeightings axisWeightings = null;

        public bool AllowInversion
        {
            get
            {
                return this.allowInversion;
            }
        }

        public SerializableAxisWeightings AxisWeightings
        {
            get
            {
                return this.axisWeightings;
            }
        }

        public void SetOffsetTo(Slerpy.Transform newOffset)
        {
            this.transform.position -= this.offset.Position.ToUnity3D();
            this.transform.rotation *= UnityEngine.Quaternion.Inverse(this.offset.Rotation.ToUnity3D());
            this.transform.localScale -= this.offset.Scale.ToUnity3D();
            
            this.offset = newOffset;

            this.transform.position += this.offset.Position.ToUnity3D();
            this.transform.rotation *= this.offset.Rotation.ToUnity3D();
            this.transform.localScale += this.offset.Scale.ToUnity3D();
        }

        protected abstract Slerpy.Transform CalculateOffset(float time);

        protected override void ProcessEffect(float deltaTime)
        {
            this.SetOffsetTo(this.CalculateOffset(this.TimeRunning));
        }

        protected void OnDestroy()
        {
            if (this.restoreTransformOnDestruction)
            {
                this.SetOffsetTo(new Slerpy.Transform(
                    new Slerpy.Vector3D(0.0f, 0.0f, 0.0f),
                    new Slerpy.Quaternion(0.0f, 0.0f, 0.0f, 1.0f),
                    new Slerpy.Vector3D(0.0f, 0.0f, 0.0f)));
            }
        }

        [Serializable]
        public sealed class SerializableAxisWeightings
        {
            [SerializeField]
            private float rateModifier = 1.0f;

            [SerializeField]
            private float strengthModifier = 1.0f;

            [SerializeField]
            private float xRate = 1.0f;

            [SerializeField]
            private float xStrength = 1.0f;

            [SerializeField]
            private float yRate = 1.0f;

            [SerializeField]
            private float yStrength = 1.0f;

            [SerializeField]
            private float zRate = 1.0f;

            [SerializeField]
            private float zStrength = 1.0f;

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

            public float StrengthModifier
            {
                get
                {
                    return this.strengthModifier;
                }

                set
                {
                    this.strengthModifier = value;
                }
            }

            public float XRate
            {
                get
                {
                    return this.xRate;
                }

                set
                {
                    this.xRate = value;
                }
            }

            public float XStrength
            {
                get
                {
                    return this.xStrength;
                }

                set
                {
                    this.xStrength = value;
                }
            }

            public float YRate
            {
                get
                {
                    return this.yRate;
                }

                set
                {
                    this.yRate = value;
                }
            }

            public float YStrength
            {
                get
                {
                    return this.yStrength;
                }

                set
                {
                    this.yStrength = value;
                }
            }

            public float ZRate
            {
                get
                {
                    return this.zRate;
                }

                set
                {
                    this.zRate = value;
                }
            }

            public float ZStrength
            {
                get
                {
                    return this.zStrength;
                }

                set
                {
                    this.zStrength = value;
                }
            }

            public Slerpy.AxisWeightings AsStruct(float time)
            {
                return new Slerpy.AxisWeightings(
                    new Weighting(this.rateModifier * this.xRate, this.strengthModifier * this.xStrength),
                    new Weighting(this.rateModifier * this.yRate, this.strengthModifier * this.yStrength),
                    new Weighting(this.rateModifier * this.zRate, this.strengthModifier * this.zStrength));
            }
        }
    }
}
