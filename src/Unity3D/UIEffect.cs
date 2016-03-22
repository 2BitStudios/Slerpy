using System;

using UnityEngine;

namespace Slerpy.Unity3D
{
    public struct AnchorOffset
    {
        public Vector2 AbsolutePosition { get; private set; }
        public Vector2 RelativePosition { get; private set; }

        public AnchorOffset(Vector2 absolutePosition, Vector2 relativePosition)
        {
            this.AbsolutePosition = absolutePosition;
            this.RelativePosition = relativePosition;
        }

        public void AddTo(RectTransform target)
        {
            target.anchoredPosition += this.AbsolutePosition;

            target.anchorMin += this.RelativePosition;
            target.anchorMax += this.RelativePosition;
        }

        public void SubtractFrom(RectTransform target)
        {
            target.anchoredPosition -= this.AbsolutePosition;

            target.anchorMin -= this.RelativePosition;
            target.anchorMax -= this.RelativePosition;
        }

        public void WriteTo(RectTransform target)
        {
            target.anchoredPosition = this.AbsolutePosition;

            target.anchorMin = this.RelativePosition;
            target.anchorMax = this.RelativePosition;
        }
    }

    public enum UIEffectAnchorMode
    {
        Absolute = 0,
        Relative = 1
    }

    [RequireComponent(typeof(RectTransform))]
    public sealed class UIEffect : Effect
    {
        private const string TOOLTIP_ANCHORMODE = "Canvas-space units of anchor transformation.";
        private const string TOOLTIP_ANCHOREXTENT = "Maximum anchor position change at a weight of 1.0. Can be exceeded or inverted by weight modifiers or time wrap type.";

        private const UIEffectAnchorMode DEFAULT_ANCHORMODE = UIEffectAnchorMode.Relative;

        public static AnchorOffset CalculateAnchorOffset(float weight, UIEffectAnchorMode anchorMode, Vector2 extent)
        {
            Vector2 anchorOffset = Interpolate.Vector2(
                Vector2.zero, 
                extent, 
                weight);

            switch (anchorMode)
            {
                case UIEffectAnchorMode.Relative:
                    return new AnchorOffset(Vector2.zero, anchorOffset);
                case UIEffectAnchorMode.Absolute:
                default:
                    return new AnchorOffset(anchorOffset, Vector2.zero);
            }
        }

        [SerializeField]
        [Tooltip(Effect.TOOLTIP_DURATION)]
        private float duration = 1.0f;
        
        [SerializeField]
        [Tooltip(Effect.TOOLTIP_TIMEWRAP)]
        private WrapType timeWrap = WrapType.Cycle;

        [SerializeField]
        [Tooltip(UIEffect.TOOLTIP_ANCHORMODE)]
        private UIEffectAnchorMode anchorMode = DEFAULT_ANCHORMODE;

        [SerializeField]
        [Tooltip(UIEffect.TOOLTIP_ANCHOREXTENT)]
        private Vector2 anchorExtent = Vector2.zero;

        [SerializeField]
        [HideInInspector]
        private UIEffectAnchorMode previousAnchorMode = DEFAULT_ANCHORMODE;

        [SerializeField]
        [HideInInspector]
        private AnchorOffset anchorOffset = new AnchorOffset(Vector2.zero, Vector2.zero);

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
                    AnchorOffset anchorOffset = this.AnchorOffset;

                    this.AnchorOffset = new AnchorOffset(Vector2.zero, Vector2.zero);

                    this.anchorMode = value;

                    this.AnchorOffset = new AnchorOffset(anchorOffset.RelativePosition, anchorOffset.AbsolutePosition);

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

        public AnchorOffset AnchorOffset
        {
            get
            {
                return this.anchorOffset;
            }

            set
            {
                this.anchorOffset.SubtractFrom((RectTransform)this.transform);

                this.anchorOffset = value;

                this.anchorOffset.AddTo((RectTransform)this.transform);
            }
        }

        protected override void ProcessEffect(float weight)
        {
            this.AnchorOffset = UIEffect.CalculateAnchorOffset(weight, this.anchorMode, this.anchorExtent);
        }

        protected override void OnValidate()
        {
            base.OnValidate();

            if (this.previousAnchorMode != this.anchorMode)
            {
                UIEffectAnchorMode newAnchorMode = this.anchorMode;

                this.anchorMode = this.previousAnchorMode;

                this.AnchorMode = newAnchorMode;
            }
        }

        [Serializable]
        public sealed class Detachable : Effect.Detachable<AnchorOffset>
        {
            [SerializeField]
            [Tooltip(UIEffect.TOOLTIP_ANCHORMODE)]
            private UIEffectAnchorMode anchorMode = DEFAULT_ANCHORMODE;

            [SerializeField]
            [Tooltip(UIEffect.TOOLTIP_ANCHOREXTENT)]
            private Vector2 anchorExtent = Vector2.zero;

            public UIEffectAnchorMode AnchorMode
            {
                get
                {
                    return this.anchorMode;
                }

                set
                {
                    this.anchorMode = value;
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

            protected override AnchorOffset Internal_CalculateState(float weight)
            {
                return UIEffect.CalculateAnchorOffset(weight, this.anchorMode, this.anchorExtent);
            }
        }
    }
}
