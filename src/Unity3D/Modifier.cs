using UnityEngine;

namespace Slerpy.Unity3D
{
    public sealed class Modifier : MonoBehaviour
    {
        [SerializeField]
        private AnimationCurve modifier = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);

        public float Evaluate(float value, float time)
        {
            return value * modifier.Evaluate(time);
        }
    }
}