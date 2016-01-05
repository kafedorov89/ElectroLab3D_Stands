using UnityEngine;
using System.Collections;

public class StandtaskCompleteAnswerHandler : AnswerHandler
{
	public StandtaskCompleteAnswerHandler()
	{
		RequestTypeName = "StandtaskComplete";
	}
	
	public override void SendAnswerObject(AnswerClass answerObject)
	{
	}
	
	public override void ExecuteCallback()
	{
		print("StandtaskComplete Callback");
		CallbackObject.GetComponent<TeacherManager>().Callback_StandtaskComplete();//, CallbackStringList[3], CallbackIntValue);
	}
}
