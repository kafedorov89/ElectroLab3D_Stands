using UnityEngine;
using System.Collections;

public class AnswerHandlerClass : MonoBehaviour
{
    public string RequestTypeName;
    //public string CallbackValueType;
    public bool CallbackBoolValue;
    public int CallbackIntValue;
    public float CallbackFloatValue;
    public string CallbackStringValue;
    public Vector3 CallbackVector3Value;
    public Vector2 CallbackVector2Value;

    public GameObject CallbackObject;

    public virtual void ExecuteCallback() { }

}
