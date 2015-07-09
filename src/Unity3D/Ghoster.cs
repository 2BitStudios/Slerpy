using System;
using System.Collections.Generic;

using UnityEngine;

namespace Slerpy.Unity3D
{
    public sealed class Ghoster : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Effects to play on new ghosts.")]
        private Effect[] ghostEffects = null;

        [SerializeField]
        [Tooltip("Effects to stop on new ghosts.")]
        private Effect[] corporealEffects = null;

        [SerializeField]
        [HideInInspector]
        private bool isGhost = true;

        private readonly LinkedList<Ghost> ghosts = new LinkedList<Ghost>();

        public bool IsGhost
        {
            get
            {
                return isGhost;
            }
        }

        public IEnumerable<Ghost> Ghosts
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
        
        public Ghost SpawnTemporaryAndGet(Func<bool> sustainQuery)
        {
            if (Application.isPlaying)
            {
                bool wasGhost = this.isGhost;

                this.isGhost = false;

                Ghost newGhost = Ghost.Create(this.gameObject, sustainQuery);

                this.isGhost = wasGhost;

                this.ghosts.AddLast(newGhost);

                return newGhost;
            }

            return null;
        }

        public void SpawnTemporary(Func<bool> sustainQuery)
        {
            this.SpawnTemporaryAndGet(sustainQuery);
        }

        public Ghost SpawnTemporaryAndGet(float lifeTime)
        {
            float endTime = Time.timeSinceLevelLoad + lifeTime;

            return this.SpawnTemporaryAndGet(() => Time.timeSinceLevelLoad < endTime);
        }

        public void SpawnTemporary(float lifeTime)
        {
            this.SpawnTemporaryAndGet(lifeTime);
        }

        [ContextMenu("Spawn")]
        public Ghost SpawnPermanentAndGet()
        {
            return this.SpawnTemporaryAndGet(() => true);
        }

        public void SpawnPermanent()
        {
            this.SpawnPermanentAndGet();
        }

        [ContextMenu("Clear")]
        public void Clear()
        {
            foreach (Ghost ghost in this.ghosts)
            {
                ghost.Dispose();
            }

            this.ghosts.Clear();
        }

        private void LateUpdate()
        {
            LinkedListNode<Ghost> ghostNode = this.ghosts.First;
            while (ghostNode != null)
            {
                if (!ghostNode.Value.AttemptSustain())
                {
                    LinkedListNode<Ghost> deadNode = ghostNode;

                    ghostNode = ghostNode.Previous;

                    this.ghosts.Remove(deadNode);

                    if (ghostNode == null)
                    {
                        break;
                    }
                }

                ghostNode = ghostNode.Next;
            }
        }

        private void Awake()
        {
            this.isGhost = !this.isGhost;
        }

        private void Start()
        {
            foreach (Effect effect in this.ghostEffects)
            {
                effect.IsPlaying = this.isGhost;
            }

            foreach (Effect effect in this.corporealEffects)
            {
                effect.IsPlaying = !this.isGhost;
            }
        }

        private void OnDestroy()
        {
            this.Clear();
        }

        public sealed class Ghost : IDisposable
        {
            public static Ghost Create(GameObject target, Func<bool> sustainQuery)
            {
                GameObject proxy = (GameObject)GameObject.Instantiate(target);

                proxy.transform.parent = target.transform.parent;

                proxy.transform.localPosition = target.transform.localPosition;
                proxy.transform.localRotation = target.transform.localRotation;
                proxy.transform.localScale = target.transform.localScale;

                proxy.transform.parent = null;

                proxy.hideFlags = HideFlags.HideAndDontSave;

                Ghost.CleanObject(proxy);

                return new Ghost(proxy, sustainQuery);
            }

            private static void CleanObject(GameObject target)
            {
                foreach (Transform child in target.transform)
                {
                    Ghost.CleanObject(child.gameObject);
                }

                foreach (Component component in target.GetComponents<Component>())
                {
                    if (!(component is Transform || component is Ghoster || component is Renderer || component is MeshFilter || component is Effect))
                    {
                        Component.Destroy(component);
                    }
                }
            }

            public GameObject Proxy { get; private set; }

            public Func<bool> SustainQuery { get; private set; }

            public bool IsAlive
            {
                get
                {
                    return this.Proxy != null;
                }
            }

            private Ghost(GameObject proxy, Func<bool> sustainQuery)
            {
                this.Proxy = proxy;

                this.SustainQuery = sustainQuery;
            }

            public bool AttemptSustain()
            {
                if (this.IsAlive)
                {
                    if (!this.SustainQuery.Invoke())
                    {
                        this.Dispose();
                    }
                    else
                    {
                        return true;
                    }
                }

                return false;
            }

            public TComponent AddComponent<TComponent>()
                where TComponent : Component
            {
                return this.Proxy.AddComponent<TComponent>();
            }

            public void Dispose()
            {
                GameObject.Destroy(this.Proxy);
            }
        }
    }
}
