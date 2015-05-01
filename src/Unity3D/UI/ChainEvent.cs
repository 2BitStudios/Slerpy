using System;

using UnityEngine;
using UnityEngine.Events;

namespace Slerpy.Unity3D.UI
{
    public abstract class ChainEvent<TArg, TEvent> : MonoBehaviour
        where TEvent : UnityEvent<TArg>
        // The event cannot be serialized by Unity3D from a generic class
        // This means it must be passed to the base as a generic parameter
    {
        [SerializeField]
        private TArg argument = default(TArg);

        [SerializeField]
        private TEvent chainEvent = null;

        public TArg Argument
        {
            get
            {
                return this.argument;
            }
        }

        public void Trigger()
        {
            this.chainEvent.Invoke(this.argument);
        }

        public void TriggerWithArgument(TArg replacementArgument)
        {
            this.chainEvent.Invoke(replacementArgument);
        }
    }

    public sealed class ChainEventBOOL : ChainEvent<bool, ChainEventBOOL.SerializableEvent>
    {
        [Serializable]
        public sealed class SerializableEvent : UnityEvent<bool> { }
    }

    public sealed class ChainEventINT : ChainEvent<int, ChainEventINT.SerializableEvent>
    {
        [Serializable]
        public sealed class SerializableEvent : UnityEvent<int> { }
    }

    public sealed class ChainEventFLOAT : ChainEvent<float, ChainEventFLOAT.SerializableEvent>
    {
        [Serializable]
        public sealed class SerializableEvent : UnityEvent<float> { }
    }

    public sealed class ChainEventSTR : ChainEvent<string, ChainEventSTR.SerializableEvent>
    {
        [Serializable]
        public sealed class SerializableEvent : UnityEvent<string> { }
    }

    public sealed class ChainEventVEC2 : ChainEvent<Vector2, ChainEventVEC2.SerializableEvent>
    {
        [Serializable]
        public sealed class SerializableEvent : UnityEvent<Vector2> { }
    }

    public sealed class ChainEventVEC3 : ChainEvent<Vector3, ChainEventVEC3.SerializableEvent>
    {
        [Serializable]
        public sealed class SerializableEvent : UnityEvent<Vector3> { }
    }

    public sealed class ChainEventQUAT : ChainEvent<Quaternion, ChainEventQUAT.SerializableEvent>
    {
        [Serializable]
        public sealed class SerializableEvent : UnityEvent<Quaternion> { }
    }

    public sealed class ChainEventCOLOR : ChainEvent<Color, ChainEventCOLOR.SerializableEvent>
    {
        [Serializable]
        public sealed class SerializableEvent : UnityEvent<Color> { }
    }

    public sealed class ChainEventGOBJ : ChainEvent<GameObject, ChainEventGOBJ.SerializableEvent>
    {
        [Serializable]
        public sealed class SerializableEvent : UnityEvent<GameObject> { }
    }

    public abstract class ChainEvent<TArg1, TArg2, TEvent> : MonoBehaviour
        where TEvent : UnityEvent<TArg1, TArg2>
        // The event cannot be serialized by Unity3D from a generic class
        // This means it must be passed to the base as a generic parameter
    {
        [SerializeField]
        private TArg1 firstArgument = default(TArg1);

        [SerializeField]
        private TArg2 secondArgument = default(TArg2);

        [SerializeField]
        private TEvent chainEvent = null;

        public TArg1 FirstArgument
        {
            get
            {
                return this.firstArgument;
            }
        }

        public TArg2 SecondArgument
        {
            get
            {
                return this.secondArgument;
            }
        }

        public void Trigger()
        {
            this.chainEvent.Invoke(this.firstArgument, this.secondArgument);
        }

        public void TriggerWithFirstArgument(TArg1 replacementArgument)
        {
            this.chainEvent.Invoke(replacementArgument, this.secondArgument);
        }

        public void TriggerWithSecondArgument(TArg2 replacementArgument)
        {
            this.chainEvent.Invoke(this.firstArgument, replacementArgument);
        }
    }

    public sealed class ChainEventGOBJ_BOOL : ChainEvent<GameObject, bool, ChainEventGOBJ_BOOL.SerializableEvent>
    {
        [Serializable]
        public sealed class SerializableEvent : UnityEvent<GameObject, bool> { }
    }

    public sealed class ChainEventGOBJ_INT : ChainEvent<GameObject, int, ChainEventGOBJ_INT.SerializableEvent>
    {
        [Serializable]
        public sealed class SerializableEvent : UnityEvent<GameObject, int> { }
    }

    public sealed class ChainEventGOBJ_FLOAT : ChainEvent<GameObject, float, ChainEventGOBJ_FLOAT.SerializableEvent>
    {
        [Serializable]
        public sealed class SerializableEvent : UnityEvent<GameObject, float> { }
    }

    public sealed class ChainEventGOBJ_STR : ChainEvent<GameObject, string, ChainEventGOBJ_STR.SerializableEvent>
    {
        [Serializable]
        public sealed class SerializableEvent : UnityEvent<GameObject, string> { }
    }

    public sealed class ChainEventGOBJ_VEC2 : ChainEvent<GameObject, Vector2, ChainEventGOBJ_VEC2.SerializableEvent>
    {
        [Serializable]
        public sealed class SerializableEvent : UnityEvent<GameObject, Vector2> { }
    }

    public sealed class ChainEventGOBJ_VEC3 : ChainEvent<GameObject, Vector3, ChainEventGOBJ_VEC3.SerializableEvent>
    {
        [Serializable]
        public sealed class SerializableEvent : UnityEvent<GameObject, Vector3> { }
    }

    public sealed class ChainEventGOBJ_QUAT : ChainEvent<GameObject, Quaternion, ChainEventGOBJ_QUAT.SerializableEvent>
    {
        [Serializable]
        public sealed class SerializableEvent : UnityEvent<GameObject, Quaternion> { }
    }

    public sealed class ChainEventGOBJ_COLOR : ChainEvent<GameObject, Color, ChainEventGOBJ_COLOR.SerializableEvent>
    {
        [Serializable]
        public sealed class SerializableEvent : UnityEvent<GameObject, Color> { }
    }

    public sealed class ChainEventGOBJ_GOBJ : ChainEvent<GameObject, GameObject, ChainEventGOBJ_GOBJ.SerializableEvent>
    {
        [Serializable]
        public sealed class SerializableEvent : UnityEvent<GameObject, GameObject> { }
    }
}
