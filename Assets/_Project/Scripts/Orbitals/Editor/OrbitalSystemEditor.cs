using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(NBodySystem))]
public class OrbitalSystemEditor : Editor
{
    NBodySystem _os;

    private void OnEnable()
    {
        _os = target as NBodySystem;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        using (var scope = new GUILayout.HorizontalScope())
        {
            if (GUILayout.Button("Slower")) _os.Step /= 10.0f;
            if (GUILayout.Button("Faster")) _os.Step *= 10.0f; 
        }
    }
}