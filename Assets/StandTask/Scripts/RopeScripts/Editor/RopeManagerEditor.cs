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
        RopeManager ropesManager = (RopeManager)target;
        // Draw the default inspector
        DrawDefaultInspector();

        //Draw button
        //GUILayout.Button ("InitNewSockets");
        if (GUILayout.Button("InitNewSockets"))
        {
            Debug.Log("InitNewSockets");
            ropesManager.InitNewSockets();
        }

        if (GUILayout.Button("ClearSockets"))
        {
            Debug.Log("ClearSockets");
            ropesManager.ClearSockets();
        }

        if (GUILayout.Button("EncodeAllRopesToJSON"))
        {
            Debug.Log("EncodeAllRopesToJSON");
            ropesManager.EncodeAllRopesToJSON();
        }

        if (GUILayout.Button("DecodeAllRopesFromJSON"))
        {
            Debug.Log("DecodeAllRopesFromJSON");
            ropesManager.DecodeAllRopesFromJSON();
        }
    }
}
