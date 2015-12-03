using UnityEngine;
using System.Collections;

public class GetStudentStandtaskAnswerHandler : AnswerHandler
{
    public GetStudentStandtaskAnswerHandler()
    {
        RequestTypeName = "GetStudentStandtask";
    }

    public override void SendAnswerObject(AnswerClass answerObject)
    {
        CallbackStringList = answerObject.string_list; //User ropes and Correct connections
        CallbackIntValue = answerObject.int_value; //Standtask ID
        //CallbackStringValue = answerObject.string_value; // Full username
    }

    public override void ExecuteCallback()
    {
        print("GetStudentStandtask Callback");
        CallbackObject.GetComponent<TeacherManager>().Callback_GetStudentStandtask(CallbackStringList[0], CallbackStringList[1], CallbackStringList[2], CallbackStringList[3], CallbackIntValue);
    }
}
