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

        private readonly List<Modifier> modifiers = new List<Modifier>();

        [SerializeField]
        [Tooltip("Time settings, such as offsets and boundaries.")]
        private TimeOptions timeOptions = new TimeOptions();

        [SerializeField]
        [Tooltip("Rate that time passes. Speeds up or slows down effects.")]
        private float speed = 1.0f;

        [SerializeField]
        [Tooltip("Strength of effect. For example, an effect that moves the object would move twice as far with a strength of 2.0.")]
        private float strength = 1.0f;

        [SerializeField]
        [Tooltip("List of weight modifiers to be applied to the base weight of the effect. Will be applied in order they appear here.")]
        private WeightType[] weights = new WeightType[] { WeightType.Linear };

        private float rawTime = 0.0f;
        private float simulatedTime = 0.0f;

        public IEnumerable<Modifier> Modifiers
        {
            get
            {
                return this.modifiers;
            }
        }

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

        public float ModifiedSpeed
        {
            get
            {
                float returnValue = this.Speed;

                for (int i = 0; i < this.modifiers.Count; ++i)
                {
                    returnValue = this.modifiers[i].ProcessSpeed(returnValue);
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

        public float ModifiedStrength
        {
            get
            {
                float returnValue = this.Strength;

                for (int i = 0; i < this.modifiers.Count; ++i)
                {
                    returnValue = this.modifiers[i].ProcessStrength(returnValue);
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

            for (int i = 0; i < this.modifiers.Count; ++i)
            {
                this.modifiers[i].Rewind();
            }

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
            
            Modifier[] modifiers = this.gameObject.GetComponents<Modifier>();
            for (int i = 0; i < modifiers.Length; ++i)
            {
                if (modifiers[i].enabled && !this.modifiers.Contains(modifiers[i]))
                {
                    this.modifiers.Add(modifiers[i]);
                }
            }

            this.ProcessEffect(this.CalculateWeight(), this.ModifiedStrength);
        }

        protected void Update()
        {
            this.AddRawTime(this.timeOptions.UseUnscaledDelta ? Time.unscaledDeltaTime : Time.deltaTime);

            this.ProcessEffect(this.CalculateWeight(), this.ModifiedStrength);
        }

        private void AddRawTime(float deltaTime)
        {
            this.rawTime += deltaTime;

            deltaTime *= this.ModifiedSpeed;

            this.SimulatedTime += deltaTime;
        }

        public abstract class Modifier : MonoBehaviour
        {
            public abstract float ProcessSpeed(float speed);
            public abstract float ProcessStrength(float strength);

            [ContextMenu("Rewind")]
            public abstract void Rewind();

            protected void OnEnable()
            {
                Effect[] effects = this.gameObject.GetComponents<Effect>();
                for (int i = 0; i < effects.Length; ++i)
                {
                    effects[i].modifiers.Add(this);
                }
            }

            protected void OnDisable()
            {
                Effect[] effects = this.gameObject.GetComponents<Effect>();
                for (int i = 0; i < effects.Length; ++i)
                {
                    effects[i].modifiers.Remove(this);
                }
            }

            protected virtual void Start() { }
        }
    }
}
