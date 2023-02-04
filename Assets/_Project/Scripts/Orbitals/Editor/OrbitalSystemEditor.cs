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

        if (GUILayout.Button("Single Step"))
        {
            _os.Running = false;
            _os.OrbitalUpdate();
        }
    }
}