using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(OrbitalSystem))]
public class OrbitalSystemEditor : Editor
{
    OrbitalSystem _os;

    private void OnEnable()
    {
        _os = target as OrbitalSystem;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        if (GUILayout.Button("Start Simulation")) _os.InitializeSimulation();

        using (var scope = new GUILayout.HorizontalScope())
        {
            if (GUILayout.Button("Slower")) _os.Step *= 10.0f;
            if (GUILayout.Button("Faster")) _os.Step /= 10.0f;
        }
    }
}