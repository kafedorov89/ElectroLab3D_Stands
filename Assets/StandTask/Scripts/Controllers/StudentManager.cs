using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using System.IO;
using UnityEngine.UI;

public class StudentManager : MonoBehaviour {

    public List<string> standtaskList;
    public WebSocketManager webSocketManager;
    public RopeManager ropeManager;
    public MessageManager messageManager;

    //GUI elements
    public Text CurStandtaskNumber;
    //public Text CurStandtaskName;
    public Toggle StandtaskCompleteFlag;

    private int current_db_string_id;

    //public CustomDropdown dropdownStandtaskList;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void UpdateRopesOnServer()
    {

    }

    public void SetCurrentStandtask(int standtask_id, string conn_json, string standtask_name = "")
    {
        StandtaskCompleteFlag.isOn = false;
        ropeManager.SetCorrectConnectionsFromJSON(conn_json);
        CurStandtaskNumber.text = standtask_id.ToString();
        //CurStandtaskName.text = standtask_name.ToString();
    }

    public void CheckStandTask(){
        if (ropeManager.CheckStandtaskConnections(false))
        {
            messageManager.ShowMessage("Схема собрана правильно. Приступайте к выполнению работы!");
            StandtaskCompleteFlag.isOn = true;

        }
        else
        {
            messageManager.ShowMessage("Схема собрана не правильно. Проверьте соединения!");
            StandtaskCompleteFlag.isOn = false;
        }
    }

    public void Callback_ActivateStantask(int db_string_id, int standtask_id, string conn_json, string standtask_name = "")
    {
        current_db_string_id = db_string_id;
        //Debug.Log("standtask_id = " + standtask_id);
        SetCurrentStandtask(standtask_id, conn_json, standtask_name);
        messageManager.ShowMessage("Схема №" + standtask_id + " готова к сборке. Добавьте необходимые соединения на стенды");
    }

    public void Callback_CheckCompleteStantask()
    {

    }
}
