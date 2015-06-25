using System;
using System.Text;
using System.Reflection;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

namespace Slerpy.Unity3D
{
    public sealed class Animator : MonoBehaviour
    {
        [SerializeField]
        private float strength = 1.0f;

        [SerializeField]
        private BoolAnimation[] boolAnimations = null;

        [SerializeField]
        private IntAnimation[] intAnimations = null;

        [SerializeField]
        private FloatAnimation[] floatAnimations = null;

        [SerializeField]
        private Vector3Animation[] vector3Animations = null;

        [SerializeField]
        private RotationAnimation[] rotationAnimations = null;

        [SerializeField]
        private ColorAnimation[] colorAnimations = null;

        [SerializeField]
        [HideInInspector]
        private float timeRunning = 0.0f;

        public IEnumerable<BoolAnimation> BoolAnimations
        {
            get
            {
                return this.boolAnimations;
            }
        }

        public IEnumerable<IntAnimation> IntAnimations
        {
            get
            {
                return this.intAnimations;
            }
        }

        public IEnumerable<FloatAnimation> FloatAnimations
        {
            get
            {
                return this.floatAnimations;
            }
        }

        public IEnumerable<Vector3Animation> Vector3Animations
        {
            get
            {
                return this.vector3Animations;
            }
        }

        public IEnumerable<RotationAnimation> RotationAnimations
        {
            get
            {
                return this.rotationAnimations;
            }
        }

        public IEnumerable<ColorAnimation> ColorAnimations
        {
            get
            {
                return this.colorAnimations;
            }
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;

            this.timeRunning += deltaTime;

            for (int i = 0; i < this.boolAnimations.Length; ++i)
            {
                this.boolAnimations[i].Evaluate(deltaTime, this.timeRunning, this.strength);
            }

            for (int i = 0; i < this.intAnimations.Length; ++i)
            {
                this.intAnimations[i].Evaluate(deltaTime, this.timeRunning, this.strength);
            }

            for (int i = 0; i < this.floatAnimations.Length; ++i)
            {
                this.floatAnimations[i].Evaluate(deltaTime, this.timeRunning, this.strength);
            }

            for (int i = 0; i < this.vector3Animations.Length; ++i)
            {
                this.vector3Animations[i].Evaluate(deltaTime, this.timeRunning, this.strength);
            }

            for (int i = 0; i < this.rotationAnimations.Length; ++i)
            {
                this.rotationAnimations[i].Evaluate(deltaTime, this.timeRunning, this.strength);
            }

            for (int i = 0; i < this.colorAnimations.Length; ++i)
            {
                this.colorAnimations[i].Evaluate(deltaTime, this.timeRunning, this.strength);
            }
        }

        [Serializable]
        public abstract class Animation
        {
            private const BindingFlags BINDING_FLAGS = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

            [SerializeField]
            private Component target = null;

            [SerializeField]
            private string propertyAccess = null;

            public Component Target
            {
                get
                {
                    return this.target;
                }
            }

            public string PropertyAccess
            {
                get
                {
                    return this.propertyAccess;
                }
            }

            public abstract void Evaluate(float deltaTime, float totalTime, float strength);

            protected void SetValue(object value)
            {
                string[] propertyNames = this.propertyAccess.Split('.');
                if (propertyNames.Length > 0)
                {
                    object propertyOwner = this.target;

                    PropertyInfo propertyInfo = propertyOwner.GetType().GetProperty(propertyNames[0], BINDING_FLAGS);

                    if (propertyInfo == null)
                    {
                        this.WritePropertyAccessError(propertyNames[0], propertyOwner);

                        return;
                    }

                    for (int i = 1; i < propertyNames.Length; ++i)
                    {
                        propertyOwner = propertyInfo.GetValue(propertyOwner, null);

                        if (propertyOwner == null)
                        {
                            Debug.LogError(String.Concat(
                                "Could not retrieve value from property '",
                                this.propertyAccess,
                                "' stage '",
                                propertyNames[i - 1],
                                "'."));

                            return;
                        }

                        propertyInfo = propertyOwner.GetType().GetProperty(propertyNames[i], BINDING_FLAGS);

                        if (propertyInfo == null)
                        {
                            this.WritePropertyAccessError(propertyNames[i], propertyOwner);

                            return;
                        }
                    }

                    propertyInfo.SetValue(propertyOwner, value, null);
                }
            }

            private void WritePropertyAccessError(string failureStage, object propertyOwner)
            {
                StringBuilder errorString = new StringBuilder();

                errorString.AppendLine(String.Concat(
                    "Could not retrieve property access '",
                    this.propertyAccess, "' stage '",
                    failureStage,
                    "', candidates are:"));

                PropertyInfo[] otherProperties = propertyOwner.GetType().GetProperties(BINDING_FLAGS);
                for (int k = 0; k < otherProperties.Length; ++k)
                {
                    if (otherProperties[k].CanWrite)
                    {
                        errorString.AppendLine("\t" + otherProperties[k].Name);
                    }
                }

                Debug.LogError(errorString.ToString());
            }
        }

        [Serializable]
        public sealed class BoolAnimation : Animation
        {
            [SerializeField]
            private float timeOffset = 0.0f;

            [SerializeField]
            private float time = 0.0f;

            [SerializeField]
            private bool value = true;

            [SerializeField]
            private bool loop = false;

            public float TimeOffset
            {
                get
                {
                    return this.timeOffset;
                }
            }

            public float Time
            {
                get
                {
                    return this.time;
                }
            }

            public bool Value
            {
                get
                {
                    return this.value;
                }
            }

            public bool Loop
            {
                get
                {
                    return this.loop;
                }
            }

            public override void Evaluate(float deltaTime, float totalTime, float strength)
            {
                totalTime -= this.timeOffset;

                if (this.time <= totalTime && ((this.loop && (totalTime % this.time) <= deltaTime) || !this.loop))
                {
                    this.SetValue(this.value);
                }
            }
        }

        [Serializable]
        public sealed class IntAnimation : Animation
        {
            [SerializeField]
            private float timeOffset = 0.0f;

            [SerializeField]
            private float time = 0.0f;

            [SerializeField]
            private int value = 0;

            [SerializeField]
            private bool loop = false;

            public float TimeOffset
            {
                get
                {
                    return this.timeOffset;
                }
            }

            public float Time
            {
                get
                {
                    return this.time;
                }
            }

            public int Value
            {
                get
                {
                    return this.value;
                }
            }

            public bool Loop
            {
                get
                {
                    return this.loop;
                }
            }

            public override void Evaluate(float deltaTime, float totalTime, float strength)
            {
                totalTime -= this.timeOffset;

                if (this.time <= totalTime && ((this.loop && (totalTime % this.time) <= deltaTime) || !this.loop))
                {
                    this.SetValue((int)(this.value * strength + 0.5f));
                }
            }
        }

        [Serializable]
        public sealed class FloatAnimation : Animation
        {
            [SerializeField]
            private AnimationCurve values = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 0.0f);

            public AnimationCurve Values
            {
                get
                {
                    return this.values;
                }
            }

            public override void Evaluate(float deltaTime, float totalTime, float strength)
            {
                this.SetValue(this.values.Evaluate(totalTime) * strength);
            }
        }

        [Serializable]
        public sealed class Vector3Animation : Animation
        {
            [SerializeField]
            private AnimationCurve xValues = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 0.0f);

            [SerializeField]
            private AnimationCurve yValues = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 0.0f);

            [SerializeField]
            private AnimationCurve zValues = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 0.0f);

            public AnimationCurve XValues
            {
                get
                {
                    return this.xValues;
                }
            }

            public AnimationCurve YValues
            {
                get
                {
                    return this.yValues;
                }
            }

            public AnimationCurve ZValues
            {
                get
                {
                    return this.zValues;
                }
            }

            public override void Evaluate(float deltaTime, float totalTime, float strength)
            {
                this.SetValue(
                    new Vector3(
                        this.xValues.Evaluate(totalTime), 
                        this.yValues.Evaluate(totalTime), 
                        this.zValues.Evaluate(totalTime)) * strength);
            }
        }

        [Serializable]
        public sealed class RotationAnimation : Animation
        {
            [SerializeField]
            private AnimationCurve xValues = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 0.0f);

            [SerializeField]
            private AnimationCurve yValues = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 0.0f);

            [SerializeField]
            private AnimationCurve zValues = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 0.0f);

            public AnimationCurve XValues
            {
                get
                {
                    return this.xValues;
                }
            }

            public AnimationCurve YValues
            {
                get
                {
                    return this.yValues;
                }
            }

            public AnimationCurve ZValues
            {
                get
                {
                    return this.zValues;
                }
            }

            public override void Evaluate(float deltaTime, float totalTime, float strength)
            {
                this.SetValue(
                    Quaternion.Euler(
                        new Vector3(
                            this.xValues.Evaluate(totalTime), 
                            this.yValues.Evaluate(totalTime), 
                            this.zValues.Evaluate(totalTime)) * strength));
            }
        }

        [Serializable]
        public sealed class ColorAnimation : Animation
        {
            [SerializeField]
            private AnimationCurve rValues = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 0.0f);

            [SerializeField]
            private AnimationCurve gValues = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 0.0f);

            [SerializeField]
            private AnimationCurve bValues = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 0.0f);

            [SerializeField]
            private AnimationCurve aValues = AnimationCurve.Linear(0.0f, 1.0f, 1.0f, 1.0f);

            public AnimationCurve RValues
            {
                get
                {
                    return this.rValues;
                }
            }

            public AnimationCurve GValues
            {
                get
                {
                    return this.gValues;
                }
            }

            public AnimationCurve BValues
            {
                get
                {
                    return this.bValues;
                }
            }

            public AnimationCurve AValues
            {
                get
                {
                    return this.aValues;
                }
            }

            public override void Evaluate(float deltaTime, float totalTime, float strength)
            {
                this.SetValue(
                    new Color(
                        this.rValues.Evaluate(totalTime), 
                        this.gValues.Evaluate(totalTime), 
                        this.bValues.Evaluate(totalTime),
                        this.aValues.Evaluate(totalTime)) * strength);
            }
        }
    }
}
