using UnityEngine;
using System.Collections;

public class CheckConnectionAnswerHandlerClass : AnswerHandlerClass
{
    public CheckConnectionAnswerHandlerClass()
    {
        RequestTypeName = "CheckConnection";
    }

    public override void ExecuteCallback()
    {
        print("CheckConnection Callback");
        CallbackObject.GetComponent<ConnectionManager>().Callback_ServerConnect(CallbackBoolValue);
    }
}
