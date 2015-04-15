using System;

using UnityEngine;

namespace Slerpy.Unity3D
{
    public abstract class Offset : MonoBehaviour
    {
        private Slerpy.Transform previousOffset = new Slerpy.Transform(
            new Slerpy.Vector3D(0.0f, 0.0f, 0.0f),
            new Slerpy.Quaternion(0.0f, 0.0f, 0.0f, 1.0f),
            new Slerpy.Vector3D(0.0f, 0.0f, 0.0f));

        private float timeRunning = 0.0f;

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

        protected abstract Slerpy.Transform CalculateOffset(float time);

        protected void Update()
        {
            this.timeRunning += Time.deltaTime;

            Slerpy.Transform currentOffset = this.CalculateOffset(this.timeRunning);

            this.transform.position += (currentOffset.Position - this.previousOffset.Position).ToUnity3D();
            this.transform.rotation *= currentOffset.Rotation.ToUnity3D() * UnityEngine.Quaternion.Inverse(this.previousOffset.Rotation.ToUnity3D());
            this.transform.localScale += (currentOffset.Scale - this.previousOffset.Scale).ToUnity3D();

            this.previousOffset = currentOffset;
        }

        [Serializable]
        public sealed class SerializableAxisWeightings
        {
            [SerializeField]
            private float xRate = 1.0f;

            [SerializeField]
            private AnimationCurve xStrength = AnimationCurve.Linear(0.0f, 1.0f, 1.0f, 1.0f);

            [SerializeField]
            private float yRate = 1.0f;

            [SerializeField]
            private AnimationCurve yStrength = AnimationCurve.Linear(0.0f, 1.0f, 1.0f, 1.0f);

            [SerializeField]
            private float zRate = 1.0f;

            [SerializeField]
            private AnimationCurve zStrength = AnimationCurve.Linear(0.0f, 1.0f, 1.0f, 1.0f);

            public SerializableAxisWeightings()
            {
                xStrength.preWrapMode = WrapMode.PingPong;
                xStrength.postWrapMode = WrapMode.PingPong;

                yStrength.preWrapMode = WrapMode.PingPong;
                yStrength.postWrapMode = WrapMode.PingPong;

                zStrength.preWrapMode = WrapMode.PingPong;
                zStrength.postWrapMode = WrapMode.PingPong;
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

            public AnimationCurve XStrength
            {
                get
                {
                    return this.xStrength;
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

            public AnimationCurve YStrength
            {
                get
                {
                    return this.yStrength;
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

            public AnimationCurve ZStrength
            {
                get
                {
                    return this.zStrength;
                }
            }

            public float GetXStrength(float time)
            {
                if (this.xStrength.length > 0)
                {
                    return this.xStrength.Evaluate(time);
                }

                return 0.0f;
            }

            public void SetXStrength(float time, float value)
            {
                this.xStrength.AddKey(time, value);
            }

            public float GetYStrength(float time)
            {
                if (this.yStrength.length > 0)
                {
                    return this.yStrength.Evaluate(time);
                }

                return 0.0f;
            }

            public void SetYStrength(float time, float value)
            {
                this.yStrength.AddKey(time, value);
            }

            public float GetZStrength(float time)
            {
                if (this.zStrength.length > 0)
                {
                    return this.zStrength.Evaluate(time);
                }

                return 0.0f;
            }

            public void SetZStrength(float time, float value)
            {
                this.zStrength.AddKey(time, value);
            }

            public Slerpy.AxisWeightings AsStruct(float time)
            {
                return new Slerpy.AxisWeightings(
                    new Weighting(this.XRate, this.GetXStrength(time)),
                    new Weighting(this.YRate, this.GetYStrength(time)),
                    new Weighting(this.ZRate, this.GetZStrength(time)));
            }
        }
    }
}
