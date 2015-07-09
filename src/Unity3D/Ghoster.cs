using System;
using System.Collections.Generic;

using UnityEngine;

namespace Slerpy.Unity3D
{
    public sealed class Ghoster : MonoBehaviour
    {
        private readonly LinkedList<Ghost> ghosts = new LinkedList<Ghost>();

        public IEnumerable<Ghost> Ghosts
        {
            get
            {
                return this.ghosts;
            }
        }
        
        public void SpawnTemporary(Func<bool> sustainQuery)
        {
            if (Application.isPlaying)
            {
                foreach (Renderer renderer in this.GetComponentsInChildren<Renderer>())
                {
                    MeshRenderer meshRenderer = renderer as MeshRenderer;
                    SkinnedMeshRenderer skinnedMeshRenderer = meshRenderer == null ? renderer as SkinnedMeshRenderer : null;

                    if (meshRenderer != null || skinnedMeshRenderer != null)
                    {
                        Mesh mesh = null;
                        Matrix4x4 transform = Matrix4x4.identity;

                        if (meshRenderer != null)
                        {
                            MeshFilter filter = renderer.gameObject.GetComponent<MeshFilter>();
                            if (filter != null && filter.mesh != null)
                            {
                                mesh = filter.mesh;
                                transform = renderer.transform.localToWorldMatrix;
                            }
                        }

                        if (skinnedMeshRenderer != null)
                        {
                            if (skinnedMeshRenderer.sharedMesh != null)
                            {
                                mesh = new Mesh();

                                skinnedMeshRenderer.BakeMesh(mesh);

                                mesh.UploadMeshData(true);

                                transform = Matrix4x4.TRS(renderer.transform.position, renderer.transform.rotation, Vector3.one);
                            }
                        }

                        if (mesh != null)
                        {
                            ghosts.AddLast(new Ghost(mesh, transform, renderer.materials, sustainQuery));
                        }
                    }
                }
            }
        }

        public void SpawnTemporary(float lifeTime)
        {
            float endTime = Time.timeSinceLevelLoad + lifeTime;

            this.SpawnTemporary(() => Time.timeSinceLevelLoad < endTime);
        }

        [ContextMenu("Spawn")]
        public void SpawnPermanent()
        {
            this.SpawnTemporary(() => true);
        }

        [ContextMenu("Clear")]
        public void Clear()
        {
            this.ghosts.Clear();
        }

        private void LateUpdate()
        {
            LinkedListNode<Ghost> ghostNode = this.ghosts.First;
            while (ghostNode != null)
            {
                if (!ghostNode.Value.SustainQuery())
                {
                    LinkedListNode<Ghost> deadNode = ghostNode;

                    ghostNode = ghostNode.Previous;

                    this.ghosts.Remove(deadNode);

                    if (ghostNode == null)
                    {
                        break;
                    }
                }
                else
                {
                    int submeshIndex = 0;
                    foreach (Material material in ghostNode.Value.Materials)
                    {
                        Graphics.DrawMesh(ghostNode.Value.Mesh, ghostNode.Value.Transform, material, 0, null, submeshIndex);

                        ++submeshIndex;
                    }
                }

                ghostNode = ghostNode.Next;
            }
        }

        public struct Ghost
        {
            public Mesh Mesh { get; private set; }
            public Matrix4x4 Transform { get; private set; }
            public IEnumerable<Material> Materials { get; private set; }

            public Func<bool> SustainQuery { get; private set; }

            public Ghost(Mesh mesh, Matrix4x4 transform, IEnumerable<Material> materials, Func<bool> sustainQuery)
                : this()
            {
                this.Mesh = mesh;
                this.Transform = transform;
                this.Materials = materials;
                this.SustainQuery = sustainQuery;
            }
        }
    }
}
