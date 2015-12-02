using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(StandObject))]
//[CanEditMultipleObjects]
//[System.Serializable]
public class StandObjectEditor : Editor
{
    //SerializedProperty PresetCameraPositionProp;

    public void OnEnable()
    {
        //objectType = new TrainingObjectType ();
        //PresetCameraPositionProp = serializedObject.FindProperty("PresetCameraPosition");
    }
    
    public override void OnInspectorGUI()
    {
        //serializedObject.Update();
        
        //Get link to current script object RopeManager
        StandObject standObject = (StandObject)target;
        // Draw the default inspector
        DrawDefaultInspector();

        //Draw button
        //GUILayout.Button ("InitNewSockets");
        if (GUILayout.Button("GetCamPos"))
        {
            Debug.Log("GetCamPos");
            standObject.GetCamPos();
        }

        //serializedObject.ApplyModifiedProperties();
    }
}