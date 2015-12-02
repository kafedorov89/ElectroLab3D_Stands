using UnityEngine;
using System.Collections;

public class ClickStandtaskListHandler : ClickClass
{

    //public TrainingManager trainingManager;
    //public ActionListManager actionListManager;
    public TeacherManager teacherManager;

    public override void DoClickAction(int i)
    {
        Debug.Log("DoClickAction");
        Debug.Log("Dropdown Value = " + i);

        teacherManager.DownloadSelectedStandTask(i);
    }
}
