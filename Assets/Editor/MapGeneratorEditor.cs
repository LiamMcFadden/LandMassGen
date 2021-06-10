using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// Create a button to generate the map within the editor so that we don't have to build each time
[CustomEditor (typeof (MapGenerator))]
public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI() 
    {
        MapGenerator mapGen = (MapGenerator)target;

        if (DrawDefaultInspector()) {
            if (mapGen.autoUpdate)
                mapGen.GenerateMap();
        }

        if (GUILayout.Button("Generate")) {
            mapGen.GenerateMap();
        }
    }
}
