using UnityEngine;
using System.Collections;

public class LogOutAnswerHandler : AnswerHandler
{
    public LogOutAnswerHandler()
    {
        RequestTypeName = "LogOut";
    }

    public override void SendAnswerObject(AnswerClass answerObject)
    {
        CallbackBoolValue = answerObject.bool_value;
    }

    public override void ExecuteCallback()
    {
        print("LogOut Callback");
        CallbackObject.GetComponent<LoginManager>().Callback_ServerLogIn(!CallbackBoolValue);
    }
}
