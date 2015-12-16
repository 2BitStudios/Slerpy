using UnityEngine;
using UnityEditor;

namespace Slerpy.Unity3D.Editor
{
    [CustomEditor(typeof(Effect))]
    [CanEditMultipleObjects]
    public abstract class EffectEditor : UnityEditor.Editor
    {
        private const float MAX_WEIGHT = 1.2f;

        private Texture2D lineTexture = null;

        private GUIStyle errorStyle = null;

        public void OnEnable()
        {
            this.lineTexture = new Texture2D(1, 1);
            this.lineTexture.SetPixel(0, 0, Color.white);
            this.lineTexture.Apply();
        }

        public override bool RequiresConstantRepaint()
        {
            return Application.isPlaying;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.Space();

            Rect weightCurveRect = GUILayoutUtility.GetAspectRect(2.0f);

            Color previousGUIColor = GUI.color;
            GUI.color = Color.black;

            GUI.DrawTexture(
                new Rect(
                    weightCurveRect.x - 1.0f,
                    weightCurveRect.y - 1.0f,
                    weightCurveRect.width + 2.0f,
                    weightCurveRect.height + 2.0f),
                this.lineTexture);

            GUI.color = Color.grey;

            GUI.Box(weightCurveRect, "");

            GUI.color = previousGUIColor;

            string error = null;

            if (this.targets.Length <= 1)
            {
                Effect targetEffect = (Effect)this.target;

                if (targetEffect.Duration != 0.0f)
                {
                    Keyframe[] weightCurveKeyframes = new Keyframe[(int)(weightCurveRect.width * 0.4f)];

                    for (int i = 0; i < weightCurveKeyframes.Length; ++i)
                    {
                        float time = targetEffect.SimulatedTime - targetEffect.Duration + (i / (float)weightCurveKeyframes.Length) * targetEffect.Duration * 2.0f;

                        weightCurveKeyframes[i] = new Keyframe(time, targetEffect.CalculateWeight(time));
                    }

                    for (int i = 0; i < weightCurveKeyframes.Length - 1; ++i)
                    {
                        weightCurveKeyframes[i].outTangent =
                            (weightCurveKeyframes[i + 1].value - weightCurveKeyframes[i].value)
                            / (weightCurveKeyframes[i + 1].time - weightCurveKeyframes[i].time);
                        weightCurveKeyframes[i + 1].inTangent =
                            (weightCurveKeyframes[i].value - weightCurveKeyframes[i + 1].value)
                            / (weightCurveKeyframes[i].time - weightCurveKeyframes[i + 1].time);
                    }

                    AnimationCurve weightCurve = new AnimationCurve(weightCurveKeyframes);

                    EditorGUIUtility.DrawRegionSwatch(
                        weightCurveRect,
                        weightCurve,
                        AnimationCurve.Linear(
                            targetEffect.SimulatedTime - targetEffect.Duration,
                            0.0f,
                            targetEffect.SimulatedTime + targetEffect.Duration,
                            0.0f),
                        Color.cyan,
                        Color.grey,
                        new Rect(
                            targetEffect.SimulatedTime - targetEffect.Duration,
                            -MAX_WEIGHT,
                            targetEffect.Duration * 2.0f,
                            MAX_WEIGHT * 2.0f));

                    GUI.color = new Color(0.0f, 0.0f, 0.0f, 0.3f);

                    float topLineY = weightCurveRect.y + weightCurveRect.height * ((MAX_WEIGHT - 1.0f) / (MAX_WEIGHT * 2.0f));

                    // Top Line
                    GUI.DrawTexture(
                        new Rect(
                            weightCurveRect.x,
                            topLineY,
                            weightCurveRect.width,
                            1.0f),
                        this.lineTexture);

                    // Bottom Line
                    GUI.DrawTexture(
                        new Rect(
                            weightCurveRect.x,
                            weightCurveRect.y + weightCurveRect.height * ((2.0f + MAX_WEIGHT - 1.0f) / (MAX_WEIGHT * 2.0f)),
                            weightCurveRect.width,
                            1.0f),
                        this.lineTexture);

                    GUI.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);

                    // Middle Horizontal Line
                    float middleLineThickness = 1.0f + (weightCurveRect.height % 2.0f == 1.0f ? 0.0f : 1.0f);
                    GUI.DrawTexture(
                        new Rect(
                            weightCurveRect.x,
                            weightCurveRect.y + weightCurveRect.height * 0.5f - (int)(middleLineThickness / 2.0f),
                            weightCurveRect.width,
                            middleLineThickness),
                        this.lineTexture);

                    //Middle Vertical Line
                    middleLineThickness = 1.0f + (weightCurveRect.width % 2.0f == 1.0f ? 0.0f : 1.0f);
                    GUI.DrawTexture(
                        new Rect(
                            weightCurveRect.x + weightCurveRect.width * 0.5f - (int)(middleLineThickness / 2.0f),
                            weightCurveRect.y,
                            middleLineThickness,
                            weightCurveRect.height),
                        this.lineTexture);

                    GUI.color = new Color(0.0f, 0.0f, 0.0f, 0.5f);

                    // Time Text
                    EditorGUI.LabelField(
                        new Rect(
                            weightCurveRect.x + 5.0f,
                            topLineY + 5.0f,
                            95.0f,
                            16.0f),
                        "Time: " + targetEffect.SimulatedTime.ToString("0.00"));

                    // Weight Text
                    EditorGUI.LabelField(
                        new Rect(
                            weightCurveRect.x + 5.0f,
                            topLineY + 21.0f,
                            95.0f,
                            16.0f),
                        "Weight: " + targetEffect.CalculateWeight().ToString("0.00"));
                }
                else
                {
                    error = "zero duration";
                }
            }
            else
            {
                error = "multiple objects selected";
            }

            if (error != null)
            {
                if (this.errorStyle == null)
                {
                    this.errorStyle = new GUIStyle(GUI.skin.label);

                    this.errorStyle.alignment = TextAnchor.MiddleCenter;
                }

                GUI.color = Color.black;

                this.errorStyle.fontSize = 72;
                EditorGUI.LabelField(weightCurveRect, "X", this.errorStyle);

                GUI.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);

                this.errorStyle.fontSize = 16;
                EditorGUI.LabelField(
                    new Rect(
                        weightCurveRect.x,
                        weightCurveRect.y + weightCurveRect.height * 0.5f,
                        weightCurveRect.width,
                        weightCurveRect.height * 0.5f), 
                    error, 
                    this.errorStyle);
            }

            EditorGUILayout.Space();

            GUI.color = previousGUIColor;

            base.OnInspectorGUI();
        }
    }

    [CustomEditor(typeof(UIEffect))]
    [CanEditMultipleObjects]
    public sealed class UIEffectEditor : EffectEditor
    {
    }

    [CustomEditor(typeof(TransformEffect))]
    [CanEditMultipleObjects]
    public sealed class TransformEffectEditor : EffectEditor
    {
    }

    [CustomEditor(typeof(ColorEffect))]
    [CanEditMultipleObjects]
    public sealed class ColorEffectEditor : EffectEditor
    {
    }

    [CustomEditor(typeof(AnimationEffect))]
    [CanEditMultipleObjects]
    public sealed class AnimationEffectEditor : EffectEditor
    {
    }
}
