using UnityEngine;
using System.Collections;

public class LogOutAnswerHandlerClass : AnswerHandlerClass
{
    public LogOutAnswerHandlerClass()
    {
        RequestTypeName = "LogOut";
    }

    public override void ExecuteCallback()
    {
        print("LogOut Callback");
        CallbackObject.GetComponent<LoginManager>().Callback_ServerLogIn(!CallbackBoolValue);
    }
}
