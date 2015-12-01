using UnityEditor;

using Slerpy.Unity3D;

[CustomEditor(typeof(AnimatedText))]
[CanEditMultipleObjects]
public sealed class AnimatedTextEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}
