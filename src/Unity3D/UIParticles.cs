﻿using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Random = UnityEngine.Random;

namespace Slerpy.Unity3D
{
    [RequireComponent(typeof(RectTransform))]
    public sealed class UIParticles : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Sprite to use for each spawned particle.")]
        private Sprite sprite = null;

        [SerializeField]
        [Tooltip("Number of particle spawns per second.")]
        private float rate = 10.0f;

        [SerializeField]
        [Tooltip("Pixel size of each spawned particle.")]
        private float size = 10.0f;

        [SerializeField]
        [Tooltip("How to handle engine time scaling (such as pauses).")]
        private bool useUnscaledTime = true;

        [SerializeField]
        [Tooltip("Life time of all particles, as well as the duration of applied Effects.")]
        private float duration = 1.0f;

        [SerializeField]
        private List<ParticleEffect> effects = new List<ParticleEffect>();

        private readonly Queue<Particle> particles = new Queue<Particle>();

        private float spawnPressure = 0.0f;

        public Sprite Sprite
        {
            get
            {
                return this.sprite;
            }

            set
            {
                this.sprite = value;
            }
        }

        public float Rate
        {
            get
            {
                return this.rate;
            }

            set
            {
                this.rate = value;
            }
        }

        public float Size
        {
            get
            {
                return this.size;
            }

            set
            {
                this.size = value;
            }
        }

        public bool UseUnscaledTime
        {
            get
            {
                return this.useUnscaledTime;
            }

            set
            {
                this.useUnscaledTime = value;
            }
        }

        public float Duration
        {
            get
            {
                return this.duration;
            }

            set
            {
                this.duration = value;
            }
        }

        public IEnumerable<ParticleEffect> Effects
        {
            get
            {
                return this.effects;
            }
        }

        private float GetParticleLifeTime(Particle particle)
        {
            if (this.useUnscaledTime)
            {
                return Time.unscaledTime - particle.UnscaledStartTime;
            }
            else
            {
                return Time.time - particle.ScaledStartTime;
            }
        }

        private void Update()
        {
            while (this.particles.Count > 0 && this.GetParticleLifeTime(this.particles.Peek()) > this.duration)
            {
                GameObject.Destroy(this.particles.Dequeue().Image.gameObject);
            }

            float durationMod = 1.0f / this.duration;

            int previousRandomSeed = Random.seed;
            
            foreach (Particle particle in this.particles)
            {
                particle.Image.rectTransform.anchoredPosition = Vector2.zero;
                particle.Image.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                particle.Image.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);

                float rawWeight = this.GetParticleLifeTime(particle) * durationMod;

                foreach (ParticleEffect effect in this.effects)
                {
                    Random.seed = particle.RandomSeed;

                    Vector2 randomExtentMod = new Vector2(
                        Random.Range(-effect.Randomness.x, effect.Randomness.x),
                        Random.Range(-effect.Randomness.y, effect.Randomness.y));

                    effect.Effect.AnchorExtent += randomExtentMod;

                    effect.Effect.CalculateState(rawWeight).AddTo(particle.Image.rectTransform);

                    effect.Effect.AnchorExtent -= randomExtentMod;
                }
            }

            Random.seed = previousRandomSeed;

            this.spawnPressure += Time.deltaTime;

            float minimumSpawnPressure = 1.0f / this.rate;

            while (this.spawnPressure >= minimumSpawnPressure)
            {
                this.spawnPressure -= minimumSpawnPressure;

                Image particleImage = new GameObject("Particle", typeof(Image)).GetComponent<Image>();

                particleImage.gameObject.hideFlags = HideFlags.HideInHierarchy | HideFlags.NotEditable;

                particleImage.rectTransform.SetParent(this.transform, false);
                particleImage.rectTransform.sizeDelta = new Vector2(this.size, this.size);

                particleImage.sprite = this.sprite;

                this.particles.Enqueue(new Particle(particleImage));
            }
        }

        [Serializable]
        public sealed class ParticleEffect
        {
            [SerializeField]
            private UIEffect.Detachable effect = null;

            [SerializeField]
            private Vector2 randomness = Vector2.zero;

            public UIEffect.Detachable Effect
            {
                get
                {
                    return this.effect;
                }

                set
                {
                    this.effect = value;
                }
            }

            public Vector2 Randomness
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
        }

        private struct Particle
        {
            public Image Image { get; private set; }

            public float ScaledStartTime { get; private set; }
            public float UnscaledStartTime { get; private set; }
            public int RandomSeed { get; private set; }

            public Particle(Image image)
            {
                this.Image = image;

                this.ScaledStartTime = Time.time;
                this.UnscaledStartTime = Time.unscaledTime;
                this.RandomSeed = (int)DateTime.Now.Ticks;
            }
        }
    }
}
