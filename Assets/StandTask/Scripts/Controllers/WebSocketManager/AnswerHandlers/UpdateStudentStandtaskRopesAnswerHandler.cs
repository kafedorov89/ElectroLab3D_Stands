using UnityEngine;
using System.Collections;

public class UpdateStudentStandtaskRopesAnswerHandler : AnswerHandler
{
    public UpdateStudentStandtaskRopesAnswerHandler()
    {
        RequestTypeName = "UpdateStudentStandtaskRopes";
    }

    public override void SendAnswerObject(AnswerClass answerObject)
    {
        CallbackStringValue = answerObject.string_value; //User ropes and Correct connections
    }

    public override void ExecuteCallback()
    {
        print("UpdateStudentStandtaskRopes Callback");
        CallbackObject.GetComponent<TeacherManager>().Callback_UpdateStudentStandtaskRopes(CallbackStringValue);
    }
}