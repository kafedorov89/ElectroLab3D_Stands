using UnityEngine;
using System.Collections;

public class ActivateStandtaskAnswerHandler : AnswerHandler
{
    public ActivateStandtaskAnswerHandler()
    {
        RequestTypeName = "ActivateStandtask";
    }

    public override void SendAnswerObject(AnswerClass answerObject)
    {
        CallbackIntList = answerObject.int_list; //id of activated standtask in main_standtask_state
        CallbackStringValue = answerObject.string_value; //Standtask correct Connections //and standtask name Option
    }

    public override void ExecuteCallback()
    {
        print("GetStudentStandtask Callback");
        CallbackObject.GetComponent<StudentManager>().Callback_ActivateStantask(CallbackIntList[0], CallbackIntList[1], CallbackStringValue, "");//, CallbackStringList[3], CallbackIntValue);
    }
}
