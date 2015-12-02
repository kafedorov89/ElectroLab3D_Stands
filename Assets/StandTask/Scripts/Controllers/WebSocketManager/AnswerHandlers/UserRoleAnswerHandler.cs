using UnityEngine;
using System.Collections;

public class UserRoleAnswerHandler : AnswerHandler
{
    public UserRoleAnswerHandler()
    {
        RequestTypeName = "UserRole";
    }

    public override void SendAnswerObject(AnswerClass answerObject)
    {
        CallbackIntValue = answerObject.int_value;
    }

    public override void ExecuteCallback()
    {
        print("UserRoleList Callback");
        CallbackObject.GetComponent<RoleManager>().Callback_UserRole(CallbackIntValue);
    }
}