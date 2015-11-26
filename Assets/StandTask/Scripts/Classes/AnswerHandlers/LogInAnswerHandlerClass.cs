using UnityEngine;
using System.Collections;

public class LogInAnswerHandlerClass : AnswerHandlerClass
{
    public LogInAnswerHandlerClass()
    {
        RequestTypeName = "LogIn";
    }
    
    public override void ExecuteCallback()
    {
        print ("LogIn Callback");
        CallbackObject.GetComponent<LoginManager>().Callback_ServerLogIn(CallbackBoolValue);
    }
}
