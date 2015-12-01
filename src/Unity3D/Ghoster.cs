using System;
using System.Collections.Generic;

using UnityEngine;

namespace Slerpy.Unity3D
{
    public sealed class Ghoster : MonoBehaviour
    {
        public static readonly Type[] defaultPreservedTypes = new Type[]
        {
            typeof(Renderer),
            typeof(MeshFilter),
            typeof(Effect)
        };

        public static IEnumerable<Type> DefaultPreservedTypes
        {
            get
            {
                return Ghoster.defaultPreservedTypes;
            }
        }

        private static bool TypeListContains(IEnumerator<Type> list, Type target)
        {
            while (list.MoveNext())
            {
                if (list.Current.IsAssignableFrom(target))
                {
                    return true;
                }
            }

            return false;
        }

        [SerializeField]
        [Tooltip("Effects to play on new ghosts.")]
        private Effect[] ghostEffects = new Effect[0];

        [SerializeField]
        [Tooltip("Effects to stop on new ghosts.")]
        private Effect[] corporealEffects = new Effect[0];

        [SerializeField]
        [Tooltip("Specific components to preserve on ghosts, in addition to the types preserved by default: 'Transform', 'Slerpy.Unity3D.Ghoster', 'Renderer', 'MeshFilter', and 'Slerpy.Unity3D.Effect'.")]
        private List<Component> preservedComponents = new List<Component>();

        private readonly LinkedList<Ghoster> ghosts = new LinkedList<Ghoster>();

        public IEnumerable<Ghoster> Ghosts
        {
            get
            {
                return this.ghosts;
            }
        }

        public int GhostCount
        {
            get
            {
                return this.ghosts.Count;
            }
        }

        public bool IsGhost
        {
            get
            {
                return this.SustainQuery != null;
            }
        }

        public Ghoster Origin { get; private set; }

        private Func<bool> SustainQuery { get; set; }
        
        public Ghoster SpawnTemporary(Func<bool> sustainQuery, IEnumerable<Type> preservedTypes = null)
        {
            if (Application.isPlaying && sustainQuery != null)
            {
                Ghoster ghost = (Ghoster)GameObject.Instantiate(this);

                ghost.Origin = this;
                ghost.SustainQuery = sustainQuery;

                ghost.transform.parent = this.transform.parent;

                ghost.transform.localPosition = this.transform.localPosition;
                ghost.transform.localRotation = this.transform.localRotation;
                ghost.transform.localScale = this.transform.localScale;

                ghost.transform.parent = null;

                ghost.gameObject.hideFlags = HideFlags.HideInHierarchy | HideFlags.NotEditable;

                ghost.CleanObject(ghost.gameObject, preservedTypes == null ? Ghoster.DefaultPreservedTypes : preservedTypes);

                this.ghosts.AddLast(ghost);

                return ghost;
            }

            return null;
        }

        public Ghoster SpawnTemporary(float lifeTime, IEnumerable<Type> preservedTypes = null)
        {
            float endTime = Time.timeSinceLevelLoad + lifeTime;

            return this.SpawnTemporary(() => Time.timeSinceLevelLoad < endTime, preservedTypes);
        }

        public void UI_SpawnTemporary(float lifeTime)
        {
            this.SpawnTemporary(lifeTime);
        }

        public Ghoster SpawnPermanent(IEnumerable<Type> preservedTypes = null)
        {
            return this.SpawnTemporary(() => true, preservedTypes);
        }

        [ContextMenu("Spawn")]
        public void UI_SpawnPermanent()
        {
            this.SpawnPermanent();
        }

        [ContextMenu("Clear")]
        public void ClearGhosts()
        {
            foreach (Ghoster ghost in this.ghosts)
            {
                Destroy(ghost.gameObject);
            }

            this.ghosts.Clear();
        }

        private void CleanObject(GameObject target, IEnumerable<Type> preservedTypes)
        {
            foreach (Transform child in target.transform)
            {
                this.CleanObject(child.gameObject, preservedTypes);
            }

            IEnumerator<Type> preservedTypesEnumerator = preservedTypes.GetEnumerator();
            
            // Reverse iteration is a partial solution to required components, as they are added recursively
            // Note that re-ordering the required components will still break the cleaning process
            Component[] components = target.GetComponents<Component>();
            for (int i = components.Length - 1; i >= 0; --i)
            {
                Component component = components[i];

                if (!(component is Transform)
                    && !(component is Ghoster)
                    && !this.preservedComponents.Contains(component) 
                    && !Ghoster.TypeListContains(preservedTypesEnumerator, component.GetType()))
                {
                    Component.DestroyImmediate(component);
                }

                preservedTypesEnumerator.Reset();
            }
        }

        private void LateUpdate()
        {
            if (this.IsGhost && !this.SustainQuery.Invoke())
            {
                GameObject.Destroy(this.gameObject);
            }
        }

        private void Start()
        {
            foreach (Effect effect in this.ghostEffects)
            {
                effect.IsPlaying = this.IsGhost;
            }

            foreach (Effect effect in this.corporealEffects)
            {
                effect.IsPlaying = !this.IsGhost;
            }
        }

        private void OnDestroy()
        {
            this.ClearGhosts();

            if (this.IsGhost)
            {
                GameObject.Destroy(this.gameObject);

                if (this.Origin != null)
                {
                    this.Origin.ghosts.Remove(this);
                }
            }
        }
    }
}
