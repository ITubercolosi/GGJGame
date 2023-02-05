using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(OrbitRenderer))]
public class OrbitRendererEditor : Editor
{
    OrbitRenderer _or;

    private void OnEnable()
    {
        _or = target as OrbitRenderer;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        if (GUILayout.Button("Simulate Trajectory")) _or.SimulateTrajectory();
    }
}