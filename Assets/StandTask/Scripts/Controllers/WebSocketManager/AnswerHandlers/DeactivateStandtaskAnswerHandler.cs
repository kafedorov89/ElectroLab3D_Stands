using UnityEngine;
using System.Collections;

public class DeactivateStandtaskAnswerHandler : AnswerHandler
{
    public DeactivateStandtaskAnswerHandler()
    {
        RequestTypeName = "DeactivateStandtask";
    }

    public override void SendAnswerObject(AnswerClass answerObject)
    {
        CallbackIntList = answerObject.int_list; //id of activated standtask in main_standtask_state
        CallbackStringValue = answerObject.string_value; //Standtask correct Connections //and standtask name Option
    }

    public override void ExecuteCallback()
    {
        print("DeactivateStandtask Callback");
        CallbackObject.GetComponent<StudentManager>().Callback_DectivateStantask();//, CallbackStringList[3], CallbackIntValue);
        //CallbackObject.GetComponent<TeacherManager>().Callback_DectivateStantask();//, CallbackStringList[3], CallbackIntValue);
    }
}