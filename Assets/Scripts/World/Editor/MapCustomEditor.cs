using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WorldGenerator))]
public class MapCustomEditor : Editor
{
    private WorldGenerator _target = null;
    private bool _autoUpdate = false;

    private void Awake()
    {
        _target = (WorldGenerator)target;
    }

    public override void OnInspectorGUI()
    {
        int seed = UnityEngine.Random.Range(0, 1000); // DEBUG

        if (DrawDefaultInspector())
            if (_autoUpdate)
                _target.Generate(seed);

        GUILayout.Space(10f);
        GUILayout.BeginHorizontal();

        _autoUpdate = GUILayout.Toggle(_autoUpdate, "Auto update");

        if (GUILayout.Button("Generate"))
            _target.Generate(seed);

        GUILayout.EndHorizontal();
    }
}
