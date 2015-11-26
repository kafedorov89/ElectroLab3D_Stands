using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(RopesManager))]
[CanEditMultipleObjects]
public class RopesManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //Get link to current script object RopesManager
        RopesManager ropesManager = (RopesManager)target;
        // Draw the default inspector
        DrawDefaultInspector();

        //Draw button
        //GUILayout.Button ("InitNewSockets");
        if (GUILayout.Button("InitNewSockets"))
        {
            Debug.Log("InitNewSockets");
            ropesManager.InitNewSockets();
        }
    }
}
