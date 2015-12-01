using UnityEditor;

namespace Slerpy.Unity3D.Editor
{
    [CustomEditor(typeof(AnimatedText))]
    [CanEditMultipleObjects]
    public sealed class AnimatedTextEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}
