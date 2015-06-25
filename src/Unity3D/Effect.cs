using System;
using System.Collections.Generic;

using UnityEngine;

namespace Slerpy.Unity3D
{
    [Serializable]
    public sealed class TimeOptions
    {
        [SerializeField]
        [Tooltip("Raw time to begin the effect at.")]
        private float offset = 0.0f;

        [SerializeField]
        [Tooltip("Random time range to add to the effect at start.")]
        private float randomness = 0.0f;

        [SerializeField]
        [Tooltip("Whether to ignore engine time scaling (such as pauses). Does not ignore local scaling via 'speedScale'.")]
        private bool useUnscaledDelta = false;

        [SerializeField]
        [Tooltip("Whether to lock simulated time to 'duration' (clamping between 0 and 'duration').")]
        private bool clampToDuration = false;

        public float Offset
        {
            get
            {
                return this.offset;
            }

            set
            {
                this.offset = value;
            }
        }

        public float Randomness
        {
            get
            {
                return this.randomness;
            }

            set
            {
                this.randomness = value;
            }
        }

        public bool UseUnscaledDelta
        {
            get
            {
                return this.useUnscaledDelta;
            }

            set
            {
                this.useUnscaledDelta = value;
            }
        }

        public bool ClampToDuration
        {
            get
            {
                return this.clampToDuration;
            }

            set
            {
                this.clampToDuration = value;
            }
        }
    }

    public abstract class Effect : MonoBehaviour
    {
        protected const string TOOLTIP_INTERPOLATE = "Weight interpolation method.";
        protected const string TOOLTIP_DURATION = "Run time of the effect, affected by 'speed'.";
        protected const string TOOLTIP_TIMEWRAP = "How time continues to affect the effect once the duration ends.";

        [SerializeField]
        [Tooltip("Time settings, such as offsets and boundaries.")]
        private TimeOptions timeOptions = new TimeOptions();

        [SerializeField]
        [Tooltip("Rate that time passes. Speeds up or slows down effects.")]
        private float speed = 1.0f;

        [SerializeField]
        [Tooltip("Multiplies against 'speed'. Evaluation time is raw running time, not speed-modified time.")]
        private List<AnimationCurve> speedModifiers = new List<AnimationCurve>();

        [SerializeField]
        [Tooltip("Strength of effect. For example, an effect that moves the object would move twice as far with a strength of 2.0.")]
        private float strength = 1.0f;

        [SerializeField]
        [Tooltip("Multiplies against 'strength'. Evaluation time is raw running time, not speed-modified time.")]
        private List<AnimationCurve> strengthModifiers = new List<AnimationCurve>();

        [SerializeField]
        [Tooltip("List of weight modifiers to be applied to the base weight of the effect. Will be applied in order they appear here.")]
        private WeightType[] weights = new WeightType[] { WeightType.Linear };

        [SerializeField]
        [HideInInspector]
        private float rawTime = 0.0f;

        [SerializeField]
        [HideInInspector]
        private float simulatedTime = 0.0f;

        public TimeOptions TimeOptions
        {
            get
            {
                return this.timeOptions;
            }
        }

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

        public IEnumerable<AnimationCurve> SpeedModifiers
        {
            get
            {
                return this.speedModifiers;
            }
        }

        public float ModifiedSpeed
        {
            get
            {
                float returnValue = this.Speed;

                for (int i = 0; i < this.speedModifiers.Count; ++i)
                {
                    returnValue = this.speedModifiers[i].Evaluate(this.rawTime) * returnValue;
                }

                return returnValue;
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

        public IEnumerable<AnimationCurve> StrengthModifiers
        {
            get
            {
                return this.strengthModifiers;
            }
        }

        public float ModifiedStrength
        {
            get
            {
                float returnValue = this.Strength;

                for (int i = 0; i < this.strengthModifiers.Count; ++i)
                {
                    returnValue = this.strengthModifiers[i].Evaluate(this.rawTime) * returnValue;
                }

                return returnValue;
            }
        }

        public IEnumerable<WeightType> Weights
        {
            get
            {
                return this.weights;
            }
        }

        public abstract InterpolateType Interpolate { get; set; }

        public abstract float Duration { get; set; }

        public abstract TimeWrapType TimeWrap { get; set; }

        public float RawTime
        {
            get
            {
                return this.rawTime;
            }
        }

        public float SimulatedTime
        {
            get
            {
                return this.simulatedTime;
            }

            set
            {
                if (this.timeOptions.ClampToDuration)
                {
                    this.simulatedTime = Mathf.Clamp(value, 0.0f, this.Duration);
                }
                else
                {
                    this.simulatedTime = value;
                }

                this.ProcessEffect(this.CalculateWeight(), this.ModifiedStrength);
            }
        }

        public bool IsPlaying
        {
            get
            {
                return this.enabled;
            }

            set
            {
                this.enabled = value;
            }
        }

        public void AddSpeedModifier(AnimationCurve modifier)
        {
            this.speedModifiers.Add(modifier);
        }

        /// <summary>
        /// Clones the <paramref name="baseModifier"/> and offsets it by current time, before adding it and returning it.
        /// </summary>
        /// <param name="baseModifier">The modifier to clone from.</param>
        /// <returns>The transformed modifier curve.</returns>
        public AnimationCurve InjectSpeedModifier(AnimationCurve baseModifier)
        {
            AnimationCurve newModifier = this.OffsetCurveByRawTime(baseModifier);

            this.AddSpeedModifier(newModifier);

            return newModifier;
        }

        public void RemoveSpeedModifier(AnimationCurve modifier)
        {
            this.speedModifiers.Remove(modifier);
        }

        public void AddStrengthModifier(AnimationCurve modifier)
        {
            this.strengthModifiers.Add(modifier);
        }

        /// <summary>
        /// Clones the <paramref name="baseModifier"/> and offsets it by current time, before adding it and returning it.
        /// </summary>
        /// <param name="baseModifier">The modifier to clone from.</param>
        /// <returns>The transformed modifier curve.</returns>
        public AnimationCurve InjectStrengthModifier(AnimationCurve baseModifier)
        {
            AnimationCurve newModifier = this.OffsetCurveByRawTime(baseModifier);

            this.AddStrengthModifier(newModifier);

            return newModifier;
        }

        public void RemoveStrengthModifier(AnimationCurve modifier)
        {
            this.strengthModifiers.Remove(modifier);
        }

        public void SetWeights(params WeightType[] newWeights)
        {
            this.weights = newWeights;
        }

        public void AddWeights(params WeightType[] additionalWeights)
        {
            WeightType[] newWeights = new WeightType[this.weights.Length + additionalWeights.Length];

            for (int i = 0; i < this.weights.Length; ++i)
            {
                newWeights[i] = this.weights[i];
            }

            for (int i = 0; i < additionalWeights.Length; ++i)
            {
                newWeights[i + this.weights.Length] = additionalWeights[i];
            }

            this.SetWeights(newWeights);
        }

        public void ClearWeights()
        {
            this.weights = new WeightType[0];
        }

        public void Play()
        {
            this.enabled = true;
        }

        public void PlayForward()
        {
            this.Play();

            if (this.speed < 0.0f)
            {
                this.speed = -this.speed;
            }
        }

        public void PlayBackward()
        {
            this.Play();

            if (this.speed > 0.0f)
            {
                this.speed = -this.speed;
            }
        }

        public void Stop()
        {
            this.enabled = false;
        }

        [ContextMenu("Reverse")]
        public void Reverse()
        {
            this.speed = -this.speed;
        }

        [ContextMenu("Rewind")]
        public void Rewind()
        {
            this.rawTime = 0.0f;
            this.simulatedTime = 0.0f;

            this.AddRawTime(this.timeOptions.Offset + UnityEngine.Random.Range(0.0f, this.timeOptions.Randomness));
        }

        public float CalculateWeight()
        {
            float weight = Weight.FromTime(
                this.TimeWrap,
                this.SimulatedTime,
                this.Duration);

            for (int i = 0; i < this.weights.Length; ++i)
            {
                weight = Weight.WithType(this.weights[i], weight);
            }

            return weight;
        }

        protected abstract void ProcessEffect(float weight, float strength);
        
        protected void Start()
        {
            this.Rewind();

            this.ProcessEffect(this.CalculateWeight(), this.ModifiedStrength);
        }

        protected void Update()
        {
            this.AddRawTime(this.timeOptions.UseUnscaledDelta ? Time.unscaledDeltaTime : Time.deltaTime);
        }

        private AnimationCurve OffsetCurveByRawTime(AnimationCurve baseCurve)
        {
            AnimationCurve offsetCurve = new AnimationCurve();

            offsetCurve.preWrapMode = baseCurve.preWrapMode;
            offsetCurve.postWrapMode = baseCurve.postWrapMode;

            Keyframe[] keys = baseCurve.keys;
            for (int i = 0; i < keys.Length; ++i)
            {
                keys[i].time += this.rawTime;

                offsetCurve.AddKey(keys[i]);
            }

            return offsetCurve;
        }

        private void AddRawTime(float deltaTime)
        {
            this.rawTime += deltaTime;

            deltaTime *= this.ModifiedSpeed;

            this.SimulatedTime += deltaTime;
        }
    }
}
