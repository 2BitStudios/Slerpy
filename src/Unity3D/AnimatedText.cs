using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Slerpy;
using Slerpy.Unity3D;

public sealed class AnimatedText : Text
{
    [SerializeField]
    [Tooltip("How time continues to affect the text once the duration ends.")]
    private WrapType timeWrap = WrapType.PingPong;

    [SerializeField]
    [Tooltip("Run time of the entire text animation.")]
    private float duration = 1.0f;

    [SerializeField]
    [Tooltip("Time delay before each character of the text (can stagger the effect or reverse it.")]
    private float interval = 0.1f;

    [SerializeField]
    private TransformEffect.Detachable[] transformEffects = new TransformEffect.Detachable[0];

    [SerializeField]
    private ColorEffect.Detachable[] colorEffects = new ColorEffect.Detachable[0];

    private float timePlaying = 0.0f;

    public WrapType TimeWrap
    {
        get
        {
            return this.timeWrap;
        }

        set
        {
            this.timeWrap = value;
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

    public float Interval
    {
        get
        {
            return this.interval;
        }

        set
        {
            this.interval = value;
        }
    }

    public IEnumerable<TransformEffect.Detachable> TransformEffects
    {
        get
        {
            return this.transformEffects;
        }
    }

    public IEnumerable<ColorEffect.Detachable> ColorEffects
    {
        get
        {
            return this.colorEffects;
        }
    }

    private float TimePlaying
    {
        get
        {
            return this.timePlaying;
        }
    }

    [ContextMenu("Rewind")]
    public void Rewind()
    {
        this.timePlaying = 0.0f;
    }

    protected override void OnFillVBO(List<UIVertex> vbo)
    {
        base.OnFillVBO(vbo);
        
        for (int i = 0; i < vbo.Count; i += 4)
        {
            float rawWeight = Weight.FromTime(this.timeWrap, Mathf.Max(this.timePlaying - i * this.interval, 0.0f), this.duration);

            Matrix4x4 transformResult = Matrix4x4.identity;

            foreach (TransformEffect.Detachable effect in this.transformEffects)
            {
                transformResult *= effect.CalculateState(rawWeight);
            }

            Color colorResult = Color.white;

            foreach (ColorEffect.Detachable effect in this.colorEffects)
            {
                colorResult *= effect.CalculateState(rawWeight);
            }

            Vector3 centre = Vector3.zero;

            for (int k = 0; k < 4; ++k)
            {
                centre += vbo[i + k].position;
            }

            centre /= 4;

            for (int k = 0; k < 4; ++k)
            {
                UIVertex vertex = vbo[i + k];
                
                vertex.position = transformResult.MultiplyPoint(vertex.position - centre) + centre;
                vertex.color *= colorResult;
                
                vbo[i + k] = vertex;
            }
        }
    }

    private void Update()
    {
        this.timePlaying += Time.deltaTime;

        this.SetVerticesDirty();
    }
}
