using Assets._Scripts.Terrain;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapGen))]
public class MarchingCubeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MapGen mapGen = (MapGen)target;

        if (DrawDefaultInspector())
        {
            if (mapGen.autoUpdate)
            {
                mapGen.OnGenerateMap();
            }
        }

        if (GUILayout.Button("Generate Map"))
        {
            mapGen.OnGenerateMap();
        }
    }
}
