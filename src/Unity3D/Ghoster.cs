using System;
using System.Collections.Generic;

using UnityEngine;

namespace Slerpy.Unity3D
{
    public sealed class Ghoster : MonoBehaviour
    {
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
        
        public Ghoster SpawnAndGetTemporary(Func<bool> sustainQuery)
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

                ghost.CleanObject(ghost.gameObject);

                this.ghosts.AddLast(ghost);

                return ghost;
            }

            return null;
        }

        public void SpawnTemporary(Func<bool> sustainQuery)
        {
            this.SpawnAndGetTemporary(sustainQuery);
        }

        public Ghoster SpawnAndGetTemporary(float lifeTime)
        {
            float endTime = Time.timeSinceLevelLoad + lifeTime;

            return this.SpawnAndGetTemporary(() => Time.timeSinceLevelLoad < endTime);
        }

        public void SpawnTemporary(float lifeTime)
        {
            this.SpawnAndGetTemporary(lifeTime);
        }

        [ContextMenu("Spawn")]
        public Ghoster SpawnAndGetPermanent()
        {
            return this.SpawnAndGetTemporary(() => true);
        }

        public void SpawnPermanent()
        {
            this.SpawnAndGetPermanent();
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

        private void CleanObject(GameObject target)
        {
            foreach (Transform child in target.transform)
            {
                this.CleanObject(child.gameObject);
            }

            // Reverse iteration is a partial solution to required components, as they are added recursively
            // Note that re-ordering the required components will still break the cleaning process
            Component[] components = target.GetComponents<Component>();
            for (int i = components.Length - 1; i >= 0; --i)
            {
                Component component = components[i];

                if (!(component is Transform
                        || component is Ghoster 
                        || component is Renderer 
                        || component is MeshFilter 
                        || component is Effect)
                    && !this.preservedComponents.Contains(component))
                {
                    Component.DestroyImmediate(component);
                }
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
