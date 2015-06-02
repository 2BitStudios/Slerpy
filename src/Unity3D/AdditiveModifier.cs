using UnityEngine;

namespace Slerpy.Unity3D
{
    public sealed class AdditiveModifier : Effect.Modifier
    {
        [SerializeField]
        private float speed = 0.0f;

        [SerializeField]
        private float strength = 0.0f;

        public float Speed
        {
            get
            {
                return this.speed;
            }

            set
            {
                this.speed = value;
            }
        }

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

        public override float ProcessSpeed(float speed)
        {
            return speed + this.speed;
        }

        public override float ProcessStrength(float strength)
        {
            return strength + this.strength;
        }

        public override void Rewind() { }
    }
}
