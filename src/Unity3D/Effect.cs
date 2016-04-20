using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

namespace Slerpy.Unity3D
{
    public enum EffectSettingTimeScaling
    {
        Scaled = 0,
        Unscaled = 1,
        ByTransformType = 2
    }

    public enum EffectSettingReverseClamp
    {
        Never = 0,
        ByTimeWrap = 1
    }

    [Serializable]
    public sealed class EffectSettings
    {
        [SerializeField]
        [Tooltip("How to handle engine time scaling (such as pauses).")]
        private EffectSettingTimeScaling timeScaling = EffectSettingTimeScaling.ByTransformType;

        [SerializeField]
        [Tooltip("How to clamp simulated time when effect is reversed. Necessary to play some time wraps (such as Clamp) backward smoothly.")]
        private EffectSettingReverseClamp reverseClamp = EffectSettingReverseClamp.ByTimeWrap;

        public EffectSettingTimeScaling TimeScaling
        {
            get
            {
                return this.timeScaling;
            }

            set
            {
                this.timeScaling = value;
            }
        }

        public EffectSettingReverseClamp ReverseClamp
        {
            get
            {
                return this.reverseClamp;
            }

            set
            {
                this.reverseClamp = value;
            }
        }
    }

    [Serializable]
    public sealed class EffectEvents
    {
        public enum Trigger
        {
            Play = 0,
            Stop = 1,
            DurationReached = 2,
            RawWeightApex = 3
        }

        public event Action OnSubscriptionChange = null;

        private event Action OnPlay = null;
        private event Action OnStop = null;
        private event Action OnDurationReached = null;
        private event Action OnRawWeightApex = null;

        [SerializeField]
        private UnityEvent onPlay = new UnityEvent();

        [SerializeField]
        private UnityEvent onStop = new UnityEvent();

        [SerializeField]
        private UnityEvent onDurationReached = new UnityEvent();

        [SerializeField]
        private UnityEvent onRawWeightApex = new UnityEvent();

        public EffectEvents()
        {
            this.SetEvent(Trigger.Play, this.onPlay.Invoke);
            this.SetEvent(Trigger.Stop, this.onStop.Invoke);
            this.SetEvent(Trigger.DurationReached, this.onDurationReached.Invoke);
            this.SetEvent(Trigger.RawWeightApex, this.onRawWeightApex.Invoke);
        }

        public bool HasSubscribers()
        {
            return 
                this.OnPlay.Target != this.onPlay || this.onPlay.GetPersistentEventCount() > 0
                || this.OnStop.Target != this.onStop || this.onStop.GetPersistentEventCount() > 0
                || this.OnDurationReached.Target != this.onDurationReached || this.onDurationReached.GetPersistentEventCount() > 0
                || this.OnRawWeightApex.Target != this.onRawWeightApex || this.onRawWeightApex.GetPersistentEventCount() > 0;
        }

        public void Register(Trigger trigger, Action callback)
        {
            this.SetEvent(
                trigger,
                this.GetEvent(trigger) + callback);

            if (this.OnSubscriptionChange != null)
            {
                this.OnSubscriptionChange();
            }
        }

        public void Invoke(Trigger trigger)
        {
            this.GetEvent(trigger).Invoke();
        }

        public void Unregister(Trigger trigger, Action callback)
        {
            this.SetEvent(
                trigger,
                this.GetEvent(trigger) - callback);

            if (this.OnSubscriptionChange != null)
            {
                this.OnSubscriptionChange();
            }
        }

        public void UnregisterAll(Trigger trigger)
        {
            Action baseEvent = null;

            switch (trigger)
            {
                case Trigger.Play:
                    baseEvent = this.onPlay.Invoke;

                    break;
                case Trigger.Stop:
                    baseEvent = this.onStop.Invoke;

                    break;
                case Trigger.DurationReached:
                    baseEvent = this.onDurationReached.Invoke;

                    break;
                case Trigger.RawWeightApex:
                    baseEvent = this.onRawWeightApex.Invoke;

                    break;
            }

            this.SetEvent(
                trigger,
                baseEvent);

            if (this.OnSubscriptionChange != null)
            {
                this.OnSubscriptionChange();
            }
        }

        private Action GetEvent(Trigger trigger)
        {
            switch (trigger)
            {
                case Trigger.Play:
                    return this.OnPlay;
                case Trigger.Stop:
                    return this.OnStop;
                case Trigger.DurationReached:
                    return this.OnDurationReached;
                case Trigger.RawWeightApex:
                    return this.OnRawWeightApex;
            }

            return null;
        }

        private void SetEvent(Trigger trigger, Action newEvent)
        {
            switch (trigger)
            {
                case Trigger.Play:
                    this.OnPlay = newEvent;

                    break;
                case Trigger.Stop:
                    this.OnStop = newEvent;

                    break;
                case Trigger.DurationReached:
                    this.OnDurationReached = newEvent;

                    break;
                case Trigger.RawWeightApex:
                    this.OnRawWeightApex = newEvent;

                    break;
            }
        }
    }

    public enum EffectCustomWeightStimulus
    {
        CalculatedWeight = 0,
        RawWeight = 1,
        Time = 2
    }

    [Serializable]
    public sealed class EffectCustomWeight
    {
        [SerializeField]
        [Tooltip("The source of the value that drives the curve.")]
        private EffectCustomWeightStimulus stimulus = EffectCustomWeightStimulus.CalculatedWeight;

        [SerializeField]
        [Tooltip("The curve to evaluate against the 'stimulus'.")]
        private AnimationCurve curve = AnimationCurve.Linear(0.0f, 1.0f, 1.0f, 1.0f);

        public EffectCustomWeightStimulus Stimulus
        {
            get
            {
                return this.stimulus;
            }
        }

        public AnimationCurve Curve
        {
            get
            {
                return this.curve;
            }
        }
    }

    public enum EffectDirection
    {
        Stalled = 0,
        Forward = 1,
        Backward = 2
    }

    public abstract class Effect : MonoBehaviour
    {
        protected const string TOOLTIP_WEIGHTS = "List of weight modifiers to be applied while calculating the weight of the effect. Will be applied in order they appear here.";
        protected const string TOOLTIP_DURATION = "Run time of the effect, affected by 'speed'.";
        protected const string TOOLTIP_TIMEWRAP = "How time continues to affect the effect once the duration is reached.";

        public static float CalculateWeight(float rawWeight, params WeightType[] weightModifiers)
        {
            for (int i = 0; i < weightModifiers.Length; ++i)
            {
                rawWeight = Weight.WithType(weightModifiers[i], rawWeight);
            }

            return rawWeight;
        }

        [SerializeField]
        [Tooltip("General settings.")]
        private EffectSettings settings = new EffectSettings();

        [SerializeField]
        [Tooltip("General events.")]
        private EffectEvents events = new EffectEvents();

        [SerializeField]
        [HideInInspector]
        private bool isSleeping = false;
        private bool isModifyingSleep = false;

        [SerializeField]
        [Tooltip(Effect.TOOLTIP_WEIGHTS)]
        private WeightType[] weights = new WeightType[] { WeightType.Linear };

        [SerializeField]
        [Tooltip("Custom weight modifier to be applied while calculating the weight of the effect. Will be applied after all other modifiers.")]
        private EffectCustomWeight customWeight = new EffectCustomWeight();

        [SerializeField]
        [Tooltip("Rate that time passes. Speeds up or slows down effects.")]
        private float speed = 1.0f;

        [SerializeField]
        [HideInInspector]
        private float rawTime = 0.0f;

        [SerializeField]
        [HideInInspector]
        private float simulatedTime = 0.0f;

        private WeightMetadata rawWeightMetadata = null;

        public EffectSettings Settings
        {
            get
            {
                return this.settings;
            }
        }

        public EffectEvents Events
        {
            get
            {
                return this.events;
            }
        }

        public IEnumerable<WeightType> Weights
        {
            get
            {
                return this.weights;
            }
        }

        public EffectCustomWeight CustomWeight
        {
            get
            {
                return this.customWeight;
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

                this.Wake();
            }
        }

        public EffectDirection Direction
        {
            get
            {
                return this.speed > 0.0f ? EffectDirection.Forward : (this.speed < 0.0f ? EffectDirection.Backward : EffectDirection.Stalled);
            }
        }

        public float RawWeight
        {
            get
            {
                return Weight.FromTime(
                    this.TimeWrap,
                    this.SimulatedTime,
                    this.Duration);
            }
        }

        /// <remarks>
        /// Can be null. Must be enabled via <see cref="EnableRawWeightMetadata"/>. Automatically enabled when any events are subscribed.
        /// </remarks>
        public WeightMetadata RawWeightMetadata
        {
            get
            {
                return this.rawWeightMetadata;
            }
        }

        public abstract float Duration { get; set; }

        public abstract WrapType TimeWrap { get; set; }

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
                float previousSimulatedTime = this.simulatedTime;

                this.simulatedTime = value;

                this.Refresh(false);

                if (Mathf.Abs(previousSimulatedTime) < this.Duration && Mathf.Abs(this.simulatedTime) >= this.Duration)
                {
                    this.events.Invoke(EffectEvents.Trigger.DurationReached);
                }

                this.Wake();
            }
        }

        public bool IsPlaying
        {
            get
            {
                return this.enabled || this.IsSleeping;
            }

            set
            {
                this.enabled = value;

                this.Wake();
            }
        }

        public bool IsSleeping
        {
            get
            {
                return this.isSleeping;
            }

            private set
            {
                if (this.isSleeping != value)
                {
                    this.isModifyingSleep = true;

                    this.isSleeping = value;
                    this.enabled = !this.isSleeping;

                    this.isModifyingSleep = false;
                }
            }
        }

        public void SetWeights(params WeightType[] newWeights)
        {
            this.weights = newWeights;

            this.Wake();
        }

        public void AddWeights(params WeightType[] additionalWeights)
        {
            WeightType[] newWeights = new WeightType[this.weights.Length + additionalWeights.Length];

            this.weights.CopyTo(newWeights, 0);
            additionalWeights.CopyTo(newWeights, this.weights.Length);

            this.SetWeights(newWeights);
        }

        public void ClearWeights()
        {
            this.weights = new WeightType[0];

            this.Wake();
        }

        public void Play()
        {
            this.IsPlaying = true;
        }

        public void PlayForward()
        {
            this.Play();

            if (this.Direction == EffectDirection.Backward)
            {
                this.Reverse();
            }
        }

        public void PlayForward(float speed)
        {
            this.PlayForward();

            this.Speed = Mathf.Abs(speed);
        }

        public void PlayBackward()
        {
            this.Play();

            if (this.Direction == EffectDirection.Forward)
            {
                this.Reverse();
            }
        }

        public void PlayBackward(float speed)
        {
            this.PlayBackward();

            this.Speed = -Mathf.Abs(speed);
        }

        public void Stop()
        {
            this.IsPlaying = false;
        }

        [ContextMenu("Reverse")]
        public void Reverse()
        {
            this.Speed = -this.Speed;

            if (this.settings.ReverseClamp == EffectSettingReverseClamp.ByTimeWrap)
            {
                if (this.TimeWrap == WrapType.Clamp)
                {
                    this.SimulatedTime = Mathf.Clamp(this.SimulatedTime, 0.0f, this.Duration);
                }

                if (this.TimeWrap == WrapType.MirrorClamp)
                {
                    this.SimulatedTime = Mathf.Clamp(this.SimulatedTime, 0.0f, this.Duration * 2.0f);
                }
            }
        }

        [ContextMenu("Rewind")]
        public void Rewind()
        {
            this.rawTime = 0.0f;

            this.SimulatedTime = 0.0f;
        }

        [ContextMenu("Wake")]
        public void Wake()
        {
            this.IsSleeping = false;
        }

        public void Refresh(bool wake = true)
        {
            if (wake)
            {
                this.Wake();
            }

            this.ProcessEffect(this.CalculateWeight(this.RawWeightMetadata));
        }

        public float CalculateWeight(float time, WeightMetadata rawWeightMetadataReceiver = null)
        {
            float rawWeight = this.CalculateRawWeight(time, rawWeightMetadataReceiver);

            float weight = rawWeight;

            weight = Effect.CalculateWeight(weight, this.weights);

            float customWeightStimulus = 0.0f;

            switch (this.customWeight.Stimulus)
            {
                case EffectCustomWeightStimulus.RawWeight:
                    customWeightStimulus = rawWeight;

                    break;
                case EffectCustomWeightStimulus.Time:
                    customWeightStimulus = time;

                    break;
                case EffectCustomWeightStimulus.CalculatedWeight:
                default:
                    customWeightStimulus = weight;

                    break;
            }

            return weight * this.customWeight.Curve.Evaluate(customWeightStimulus);
        }

        public float CalculateWeight(WeightMetadata rawWeightMetadataReceiver = null)
        {
            return this.CalculateWeight(this.SimulatedTime, rawWeightMetadataReceiver);
        }

        public void EnableRawWeightMetadata()
        {
            this.rawWeightMetadata = new WeightMetadata();
        }

        public void DisableRawWeightMetadata()
        {
            if (!this.events.HasSubscribers())
            {
                this.rawWeightMetadata = null;
            }
        }

        protected abstract void ProcessEffect(float weight);
        
        protected virtual void Start()
        {
            this.events.OnSubscriptionChange += this.EnsureEventsHaveRawWeightMetadata;

            this.EnsureEventsHaveRawWeightMetadata();

            /// Allows time-based events to trigger even if starting time is non-zero
            float tempSimulatedTime = this.simulatedTime;

            this.simulatedTime = 0.0f;

            this.SimulatedTime = tempSimulatedTime;
            ///
        }

        protected virtual void OnEnable()
        {
            if (!this.isModifyingSleep)
            {
                if (Application.isPlaying && !this.IsSleeping)
                {
                    this.events.Invoke(EffectEvents.Trigger.Play);
                }

                this.Wake();
            }
        }

        protected virtual void OnDisable()
        {
            if (!this.isModifyingSleep)
            {
                if (Application.isPlaying && !this.IsSleeping)
                {
                    this.events.Invoke(EffectEvents.Trigger.Stop);
                }

                this.Wake();
            }
        }

        protected virtual void OnValidate()
        {
            if (Application.isPlaying)
            {
                this.EnsureEventsHaveRawWeightMetadata();

                this.Wake();
            }
        }

        protected void Update()
        {
            EffectSettingTimeScaling timeScaling = this.settings.TimeScaling;

            if (timeScaling == EffectSettingTimeScaling.ByTransformType)
            {
                timeScaling = this.transform is RectTransform ? EffectSettingTimeScaling.Unscaled : EffectSettingTimeScaling.Scaled;
            }

            bool hasRawWeightMetadata = this.RawWeightMetadata != null;

            int previousApexCount = 0;

            if (hasRawWeightMetadata)
            {
                previousApexCount = this.RawWeightMetadata.ApexCount;
            }

            this.AddRawTime(timeScaling == EffectSettingTimeScaling.Unscaled ? Time.unscaledDeltaTime : Time.deltaTime);

            if (hasRawWeightMetadata)
            {
                if (previousApexCount != this.RawWeightMetadata.ApexCount)
                {
                    this.events.Invoke(EffectEvents.Trigger.RawWeightApex);
                }
            }

            if (this.IsPlaying)
            {
                if (this.TimeWrap == WrapType.Clamp
                    && ((this.Direction == EffectDirection.Forward && this.SimulatedTime >= this.Duration)
                        || (this.Direction == EffectDirection.Backward && this.SimulatedTime <= 0.0f)))
                {
                    this.IsSleeping = true;
                }

                if (this.TimeWrap == WrapType.MirrorClamp
                    && ((this.Direction == EffectDirection.Forward && this.SimulatedTime >= this.Duration * 2.0f)
                        || (this.Direction == EffectDirection.Backward && this.SimulatedTime <= 0.0f)))
                {
                    this.IsSleeping = true;
                }
            }
        }

        private float CalculateRawWeight(float time, WeightMetadata weightMetadataReceiver = null)
        {
            return Weight.FromTime(
                this.TimeWrap,
                time,
                this.Duration,
                weightMetadataReceiver);
        }

        private void AddRawTime(float deltaTime)
        {
            this.rawTime += deltaTime;

            deltaTime *= this.Speed;

            this.SimulatedTime += deltaTime;
        }

        private void EnsureEventsHaveRawWeightMetadata()
        {
            if (this.events.HasSubscribers())
            {
                this.EnableRawWeightMetadata();
            }
        }

        [Serializable]
        public abstract class Detachable<TState>
        {
            [SerializeField]
            [Tooltip(Effect.TOOLTIP_WEIGHTS)]
            private WeightType[] weights = new WeightType[] { WeightType.Linear };

            public IEnumerable<WeightType> Weights
            {
                get
                {
                    return this.weights;
                }
            }

            public void SetWeights(params WeightType[] newWeights)
            {
                this.weights = newWeights;
            }

            public void AddWeights(params WeightType[] additionalWeights)
            {
                WeightType[] newWeights = new WeightType[this.weights.Length + additionalWeights.Length];

                this.weights.CopyTo(newWeights, 0);
                additionalWeights.CopyTo(newWeights, this.weights.Length);

                this.SetWeights(newWeights);
            }

            public void ClearWeights()
            {
                this.weights = new WeightType[0];
            }

            public TState CalculateState(float rawWeight)
            {
                return this.Internal_CalculateState(Effect.CalculateWeight(rawWeight, this.weights));
            }

            protected abstract TState Internal_CalculateState(float weight);
        }
    }
}
