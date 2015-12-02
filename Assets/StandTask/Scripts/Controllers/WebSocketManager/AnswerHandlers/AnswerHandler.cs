using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnswerHandler : MonoBehaviour
{
    public string RequestTypeName;

    public bool CallbackBoolValue;
    public int CallbackIntValue;
    public float CallbackFloatValue;
    public string CallbackStringValue;
    public Vector3 CallbackVector3Value;
    public Vector2 CallbackVector2Value;

    public List<bool> CallbackBoolList;
    public List<int> CallbackIntList;
    public List<float> CallbackFloatList;
    public List<string> CallbackStringList;
    public List<Vector3> CallbackVector3List;
    public List<Vector2> CallbackVector2List;

    public GameObject CallbackObject; //Object where exist any Callback script

    public virtual void SendAnswerObject(AnswerClass answer) { }

    public virtual void ExecuteCallback() { }

}
