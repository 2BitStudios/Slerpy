using UnityEngine;

namespace Slerpy.Unity3D
{
    public enum UIEffectAnchorMode
    {
        Absolute = 0,
        Relative = 1
    }

    [RequireComponent(typeof(RectTransform))]
    public sealed class UIEffect : Effect
    {
        private const UIEffectAnchorMode ANCHORMODE_DEFAULT = UIEffectAnchorMode.Relative;

        public static Vector2 CalculateAnchorOffset(float weight, Vector2 extent)
        {
            return Extensions.InterpolateVector2(
                Vector2.zero, 
                extent, 
                weight, 
                InterpolateType.Standard);
        }

        [SerializeField]
        [Tooltip(Effect.TOOLTIP_DURATION)]
        private float duration = 1.0f;
        
        [SerializeField]
        [Tooltip(Effect.TOOLTIP_TIMEWRAP)]
        private WrapType timeWrap = WrapType.Cycle;

        [SerializeField]
        [Tooltip("Canvas-space units of anchor transformation.")]
        private UIEffectAnchorMode anchorMode = ANCHORMODE_DEFAULT;

        [SerializeField]
        [Tooltip("Maximum anchor position change at a weight of 1.0. Can be exceeded or inverted by weight modifiers or time wrap type.")]
        private Vector2 anchorExtent = Vector2.zero;

        [SerializeField]
        [HideInInspector]
        private UIEffectAnchorMode previousAnchorMode = ANCHORMODE_DEFAULT;

        [SerializeField]
        [HideInInspector]
        private Vector2 anchorOffset = Vector2.zero;

        public RectTransform RectTransform { get; private set; }

        public override float Duration
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

        public override WrapType TimeWrap
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

        public UIEffectAnchorMode AnchorMode
        {
            get
            {
                return this.anchorMode;
            }

            set
            {
                if (this.anchorMode != value)
                {
                    Vector2 anchorOffset = this.AnchorOffset;

                    this.AnchorOffset = Vector2.zero;

                    this.anchorMode = value;

                    this.AnchorOffset = anchorOffset;

                    this.previousAnchorMode = this.anchorMode;
                }
            }
        }

        public Vector2 AnchorExtent
        {
            get
            {
                return this.anchorExtent;
            }

            set
            {
                this.anchorExtent = value;
            }
        }

        public Vector2 AnchorOffset
        {
            get
            {
                return this.anchorOffset;
            }

            set
            {
                Vector2 anchorOffsetDelta = value - this.anchorOffset;

                this.anchorOffset = value;

                switch (this.anchorMode)
                {
                    case UIEffectAnchorMode.Absolute:
                        this.RectTransform.anchoredPosition += anchorOffsetDelta;

                        break;
                    case UIEffectAnchorMode.Relative:
                        this.RectTransform.anchorMin += anchorOffsetDelta;
                        this.RectTransform.anchorMax += anchorOffsetDelta;

                        break;
                }
            }
        }

        protected override void ProcessEffect(float weight)
        {
            this.AnchorOffset = UIEffect.CalculateAnchorOffset(weight, this.anchorExtent);
        }

        protected override void Start()
        {
            this.RectTransform = this.gameObject.GetComponent<RectTransform>();

            base.Start();
        }

        private void OnValidate()
        {
            if (this.previousAnchorMode != this.anchorMode)
            {
                UIEffectAnchorMode newAnchorMode = this.anchorMode;

                this.anchorMode = this.previousAnchorMode;

                this.AnchorMode = newAnchorMode;
            }
        }
    }
}
