using UnityEngine;

namespace Slerpy.Unity3D
{
    public sealed class CurveModifier : Effect.Modifier
    {
        [SerializeField]
        private AnimationCurve speed = AnimationCurve.Linear(0.0f, 1.0f, 1.0f, 1.0f);

        [SerializeField]
        private AnimationCurve strength = AnimationCurve.Linear(0.0f, 1.0f, 1.0f, 1.0f);

        private float evaluationTime = 0.0f;

        public AnimationCurve Speed
        {
            get
            {
                return this.speed;
            }
        }

        public AnimationCurve Strength
        {
            get
            {
                return this.strength;
            }
        }

        public float EvaluationTime
        {
            get
            {
                return this.evaluationTime;
            }

            set
            {
                this.evaluationTime = value;
            }
        }

        public override float ProcessSpeed(float speed)
        {
            return this.speed.Evaluate(this.evaluationTime) * speed;
        }

        public override float ProcessStrength(float strength)
        {
            return this.strength.Evaluate(this.evaluationTime) * strength;
        }

        public override void Rewind()
        {
            this.evaluationTime = 0.0f;
        }

        private void Update()
        {
            this.evaluationTime += Time.deltaTime;
        }
    }
}
