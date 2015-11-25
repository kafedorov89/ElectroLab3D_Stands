using UnityEngine;
using System.Collections;

public class ClickActionList : ClickClass {

    //public TrainingManager trainingManager;
    public ActionListManager actionListManager;
    
    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void DoClickAction(int i)
    {
        Debug.Log("DoClickAction");
        Debug.Log("Dropdown Value = " + i);

        //Do selected action from TrainingObject's list
        if (actionListManager.savedTrainingObject.ActionList.Length > 0)
        {
            if ((i <= actionListManager.savedTrainingObject.ActionList.Length) && (i >= 0))
            {
                actionListManager.savedTrainingObject.ActionList[i].StartAction = true;

                //Hide action list and go to WalkMode navigation
                if (actionListManager.customDropdown.HideListOnClick)
                {
                    // Hide Menu
                    actionListManager.customDropdown.HideListObject();

                    // Enable WalkMode
                    actionListManager.navManager.WalkMode = true;

                    // Switch off GUI
                    actionListManager.guiManager.EnableGUI(false);
                }
            }
        }
    }
}
