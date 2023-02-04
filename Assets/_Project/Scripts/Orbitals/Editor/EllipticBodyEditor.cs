using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EllipticBody))]
public class EllipticBodyEditor : Editor
{
    EllipticBody _eb;

    private void OnEnable()
    {
        _eb = target as EllipticBody;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        if (GUILayout.Button("Generate Orbit"))
        {
            _eb.GetReferences();

            _eb.CalculateParameters();
            _eb.GenerateOrbit();
            _eb.UpdateRenderer();

            SceneView.RepaintAll();
        }
    }
}