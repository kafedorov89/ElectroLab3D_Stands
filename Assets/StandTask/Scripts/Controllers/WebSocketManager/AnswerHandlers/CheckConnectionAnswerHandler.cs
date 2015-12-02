using UnityEngine;
using System.Collections;

public class CheckConnectionAnswerHandler : AnswerHandler
{
    public CheckConnectionAnswerHandler()
    {
        RequestTypeName = "CheckConnection";
    }

    public override void SendAnswerObject(AnswerClass answerObject)
    {
        CallbackBoolValue = answerObject.bool_value;
    }

    public override void ExecuteCallback()
    {
        print("CheckConnection Callback");
        CallbackObject.GetComponent<ConnectionManager>().Callback_ServerConnect(CallbackBoolValue);
    }
}
