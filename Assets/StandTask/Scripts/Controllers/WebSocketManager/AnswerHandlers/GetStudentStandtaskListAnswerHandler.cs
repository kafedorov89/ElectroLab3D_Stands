using UnityEngine;
using System.Collections;

public class GetStudentStandtaskListAnswerHandler : AnswerHandler
{
    public GetStudentStandtaskListAnswerHandler()
    {
        RequestTypeName = "GetStudentStandtaskList";
    }

    public override void SendAnswerObject(AnswerClass answerObject)
    {
        CallbackIntList = answerObject.int_list; //User ropes and Correct connections
        CallbackStringList = answerObject.string_list; // Full standtask username
    }

    public override void ExecuteCallback()
    {
        print("GetStudentStandtaskList Callback");
        CallbackObject.GetComponent<TeacherManager>().Callback_GetStudentStandtaskList(CallbackIntList, CallbackStringList);
    }
}
