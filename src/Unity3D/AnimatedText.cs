using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace Slerpy.Unity3D
{
    public sealed class AnimatedText : Text
    {
        [SerializeField]
        [Tooltip("How to handle engine time scaling (such as pauses).")]
        private bool useUnscaledTime = true;

        [SerializeField]
        [Tooltip("How time continues to affect the text once the duration is reached.")]
        private WrapType timeWrap = WrapType.PingPong;

        [SerializeField]
        [Tooltip("Run time of the entire text animation.")]
        private float duration = 1.0f;

        [SerializeField]
        [Tooltip("Time delay before each character of the text (can stagger the effect or reverse it).")]
        private float interval = 0.1f;

        [SerializeField]
        private TransformEffect.Detachable[] transformEffects = new TransformEffect.Detachable[0];

        [SerializeField]
        private ColorEffect.Detachable[] colorEffects = new ColorEffect.Detachable[0];

        private float animationTime = 0.0f;

        public bool UseUnscaledTime
        {
            get
            {
                return this.useUnscaledTime;
            }

            set
            {
                this.useUnscaledTime = value;
            }
        }

        public WrapType TimeWrap
        {
            get
            {
                return this.timeWrap;
            }

            set
            {
                this.timeWrap = value;
            }
        }

        public float Duration
        {
            get
            {
                return this.duration;
            }

            set
            {
                this.duration = value;
            }
        }

        public float Interval
        {
            get
            {
                return this.interval;
            }

            set
            {
                this.interval = value;
            }
        }

        public IEnumerable<TransformEffect.Detachable> TransformEffects
        {
            get
            {
                return this.transformEffects;
            }
        }

        public IEnumerable<ColorEffect.Detachable> ColorEffects
        {
            get
            {
                return this.colorEffects;
            }
        }

        public float AnimationTime
        {
            get
            {
                return this.animationTime;
            }

            set
            {
                this.animationTime = value;
            }
        }

        [ContextMenu("Rewind")]
        public void Rewind()
        {
            this.animationTime = 0.0f;
        }

        protected override void OnPopulateMesh(VertexHelper toFill)
        {
            base.OnPopulateMesh(toFill);
            
            for (int i = 0; i < toFill.currentVertCount; i += 4)
            {
                float rawWeight = Weight.FromTime(this.timeWrap, Mathf.Max(this.animationTime - i * this.interval, 0.0f), this.duration);

                Matrix4x4 transformResult = Matrix4x4.identity;

                foreach (TransformEffect.Detachable effect in this.transformEffects)
                {
                    transformResult *= effect.CalculateState(rawWeight);
                }

                Color colorResult = Color.white;

                foreach (ColorEffect.Detachable effect in this.colorEffects)
                {
                    colorResult *= effect.CalculateState(rawWeight);
                }

                Vector3 centre = Vector3.zero;

                for (int k = 0; k < 4; ++k)
                {
                    UIVertex vertex = default(UIVertex);

                    toFill.PopulateUIVertex(ref vertex, i + k);

                    centre += vertex.position;
                }

                centre /= 4;

                for (int k = 0; k < 4; ++k)
                {
                    UIVertex vertex = default(UIVertex);

                    toFill.PopulateUIVertex(ref vertex, i + k);
                    
                    vertex.position = transformResult.MultiplyPoint(vertex.position - centre) + centre;
                    vertex.color *= colorResult;
                    
                    toFill.SetUIVertex(vertex, i + k);
                }
            }
        }

        private void Update()
        {
            this.animationTime += this.useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

            this.SetVerticesDirty();
        }
    }
}
