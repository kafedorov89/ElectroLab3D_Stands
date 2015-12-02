using UnityEngine;
using System.Collections;

public class LogInAnswerHandler : AnswerHandler
{
    public LogInAnswerHandler()
    {
        RequestTypeName = "LogIn";
    }

    public override void SendAnswerObject(AnswerClass answerObject)
    {
        CallbackBoolValue = answerObject.bool_value;
    }

    public override void ExecuteCallback()
    {
        print ("LogIn Callback");
        CallbackObject.GetComponent<LoginManager>().Callback_ServerLogIn(CallbackBoolValue);
    }
}


