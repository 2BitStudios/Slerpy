using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace Slerpy.Unity3D
{
    [RequireComponent(typeof(RectTransform))]
    public sealed class UIParticles : MonoBehaviour
    {
        [SerializeField]
        private Sprite sprite = null;

        [SerializeField]
        private float rate = 10.0f;

        [SerializeField]
        private float size = 10.0f;

        [SerializeField]
        private float duration = 1.0f;

        [SerializeField]
        private Vector2 randomness = Vector2.zero;

        [SerializeField]
        private UIEffect.Detachable effect = null;

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

        private void Update()
        {
            while (this.particles.Count > 0 && (Time.time - this.particles.Peek().StartTime) > this.duration)
            {
                GameObject.Destroy(this.particles.Dequeue().Image.gameObject);
            }

            float durationMod = 1.0f / this.duration;
            
            foreach (Particle particle in this.particles)
            {
                float rawWeight = (Time.time - particle.StartTime) * durationMod;

                this.effect.AnchorExtent += particle.RandomExtentMod;
                
                this.effect.CalculateState(rawWeight).WriteTo(particle.Image.rectTransform);

                this.effect.AnchorExtent -= particle.RandomExtentMod;
            }

            this.spawnPressure += Time.deltaTime;

            float minimumSpawnPressure = 1.0f / this.rate;

            while (this.spawnPressure >= minimumSpawnPressure)
            {
                this.spawnPressure -= minimumSpawnPressure;

                Image particleImage = new GameObject("Particle", typeof(Image)).GetComponent<Image>();

                particleImage.rectTransform.SetParent(this.transform, false);
                particleImage.rectTransform.sizeDelta = new Vector2(this.size, this.size);

                particleImage.sprite = this.sprite;

                this.particles.Enqueue(
                    new Particle(
                        particleImage, 
                        new Vector2(
                            Random.Range(-this.randomness.x, this.randomness.x),
                            Random.Range(-this.randomness.y, this.randomness.y))));
            }
        }

        private struct Particle
        {
            public Image Image { get; private set; }

            public float StartTime { get; private set; }
            public Vector2 RandomExtentMod { get; private set; }

            public Particle(Image image, Vector2 randomExtentMod)
            {
                this.Image = image;

                this.StartTime = Time.time;
                this.RandomExtentMod = randomExtentMod;
            }
        }
    }
}
