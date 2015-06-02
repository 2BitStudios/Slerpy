using UnityEngine;

namespace Slerpy.Unity3D
{
    public sealed class ClampModifier : Effect.Modifier
    {
        [SerializeField]
        private float speedMin = 0.0f;

        [SerializeField]
        private float speedMax = 1.0f;

        [SerializeField]
        private float strengthMin = 0.0f;

        [SerializeField]
        private float strengthMax = 1.0f;

        public float SpeedMin
        {
            get
            {
                return this.speedMin;
            }

            set
            {
                this.speedMin = value;
            }
        }

        public float SpeedMax
        {
            get
            {
                return this.speedMax;
            }

            set
            {
                this.speedMax = value;
            }
        }

        public float StrengthMin
        {
            get
            {
                return this.strengthMin;
            }

            set
            {
                this.strengthMin = value;
            }
        }

        public float StrengthMax
        {
            get
            {
                return this.strengthMax;
            }

            set
            {
                this.strengthMax = value;
            }
        }

        public override float ProcessSpeed(float speed)
        {
            return Mathf.Clamp(speed, this.speedMin, this.speedMax);
        }

        public override float ProcessStrength(float strength)
        {
            return Mathf.Clamp(strength, this.strengthMin, this.strengthMax);
        }

        public override void Rewind() { }
    }
}
