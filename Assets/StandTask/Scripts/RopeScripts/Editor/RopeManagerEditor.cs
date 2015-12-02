using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(RopeManager))]
[CanEditMultipleObjects]
public class RopeManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //Get link to current script object RopeManager
        RopeManager ropeManager = (RopeManager)target;
        // Draw the default inspector
        DrawDefaultInspector();

        //Draw button
        //GUILayout.Button ("InitNewSockets");
        if (GUILayout.Button("InitNewSockets"))
        {
            Debug.Log("InitNewSockets");
            ropeManager.InitNewSockets();
        }

        if (GUILayout.Button("ClearSockets"))
        {
            Debug.Log("ClearSockets");
            ropeManager.ClearSockets();
        }

        if (GUILayout.Button("EncodeAllRopesToJSON"))
        {
            Debug.Log("EncodeAllRopesToJSON");
            string rope_json = ropeManager.EncodeAllRopesToJSON();
            ropeManager.AllRopesFileWriter(rope_json, "");
        }

        if (GUILayout.Button("DecodeAllRopesFromJSON"))
        {
            Debug.Log("CreateRopesFromJSON");
            string rope_json = ropeManager.AllRopesFileReader("");
            ropeManager.CreateRopesFromJSON(rope_json);
        }
    }
}
