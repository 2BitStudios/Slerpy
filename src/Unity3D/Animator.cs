using System;
using System.Text;
using System.Reflection;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

namespace Slerpy.Unity3D
{
    public sealed class Animator : Effect
    {
        [SerializeField]
        private FloatAnimation[] floatAnimations = null;

        [SerializeField]
        private Vector3Animation[] vector3Animations = null;

        [SerializeField]
        private RotationAnimation[] rotationAnimations = null;

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

        protected override void ProcessEffect(float deltaTime)
        {
            for (int i = 0; i < this.floatAnimations.Length; ++i)
            {
                this.floatAnimations[i].Evaluate(this.TimeRunning);
            }

            for (int i = 0; i < this.vector3Animations.Length; ++i)
            {
                this.vector3Animations[i].Evaluate(this.TimeRunning);
            }

            for (int i = 0; i < this.rotationAnimations.Length; ++i)
            {
                this.rotationAnimations[i].Evaluate(this.TimeRunning);
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

            public abstract void Evaluate(float time);

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
                    errorString.AppendLine("\t" + otherProperties[k].Name);
                }

                Debug.LogError(errorString.ToString());
            }
        }

        [Serializable]
        public sealed class FloatAnimation : Animation
        {
            [SerializeField]
            private AnimationCurve values = null;

            public AnimationCurve Values
            {
                get
                {
                    return this.values;
                }
            }

            public override void Evaluate(float time)
            {
                this.SetValue(this.values.Evaluate(time));
            }
        }

        [Serializable]
        public sealed class Vector3Animation : Animation
        {
            [SerializeField]
            private AnimationCurve xValues = null;

            [SerializeField]
            private AnimationCurve yValues = null;

            [SerializeField]
            private AnimationCurve zValues = null;

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

            public override void Evaluate(float time)
            {
                this.SetValue(new Vector3(this.xValues.Evaluate(time), this.yValues.Evaluate(time), this.zValues.Evaluate(time)));
            }
        }

        [Serializable]
        public sealed class RotationAnimation : Animation
        {
            [SerializeField]
            private AnimationCurve xValues = null;

            [SerializeField]
            private AnimationCurve yValues = null;

            [SerializeField]
            private AnimationCurve zValues = null;

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

            public override void Evaluate(float time)
            {
                this.SetValue(UnityEngine.Quaternion.Euler(this.xValues.Evaluate(time), this.yValues.Evaluate(time), this.zValues.Evaluate(time)));
            }
        }
    }
}
