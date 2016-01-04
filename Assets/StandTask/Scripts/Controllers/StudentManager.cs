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
    public RayCastManager rayCastManager;

    //GUI elements
    public Text CurStandtaskNumber;

    public Text CurStandNumber;
    //public Text CurStandtaskName;
    public Toggle StandtaskCompleteFlag;

    private int current_db_string_id;

    //public CustomDropdown dropdownStandtaskList;

    void Awake()
    {
        webSocketManager = FindObjectOfType<WebSocketManager>();
        messageManager = FindObjectOfType<MessageManager>();
        rayCastManager = FindObjectOfType<RayCastManager>();
    }

	// Use this for initialization
	void Start () {
        ResetStandtask();
	}
	
	// Update is called once per frame
	void Update () {
        if (rayCastManager.LookAtStandObject != null)
        {
            CurStandNumber.text = rayCastManager.LookAtStandObject.Name;
        }
	}

    public void SetCurrentStandtask(int standtask_id, string conn_json, string standtask_name = "")
    {
        StandtaskCompleteFlag.isOn = false;
        ropeManager.SetCorrectConnectionsFromJSON(conn_json);
        ropeManager.active_standtask_id = standtask_id;
        CurStandtaskNumber.text = standtask_id.ToString();
        ropeManager.UpdateUserRopesToDatebase();
        //CurStandtaskName.text = standtask_name.ToString();
    }

    public void ResetStandtask(){
        ropeManager.correctConnectionsList.Clear();
        //ropeManager.RemoveAllRopes();
        //ropeManager.resetSocketsColor();
        CurStandtaskNumber.text = "";
        ropeManager.active_standtask_id = -1;
        StandtaskCompleteFlag.isOn = false;
    }

    public void CheckStandTask(){
        if (ropeManager.active_standtask_id > 0)
        {
            if (ropeManager.CheckStandtaskConnections(false))
            //if (ropeManager.CheckStandtaskConnections(true)) //DEBUG. Заменить обратно, чтобы у студента не было видно подсветки проводов
            {
                //Send information about complete standtask to server
                webSocketManager.SendPackageToServer("SetStandtaskComplete");

                messageManager.ShowMessage("Схема собрана правильно. Приступайте к выполнению работы!");
                ResetStandtask();
                StandtaskCompleteFlag.isOn = true;

            }
            else
            {
                messageManager.ShowMessage("Схема собрана не правильно. Проверьте соединения!");
                StandtaskCompleteFlag.isOn = false;
            }
        }
        else
        {
            messageManager.ShowMessage("Схема не выбрана! Активируйте схему для сборки в лабораторной работе");
            StandtaskCompleteFlag.isOn = false;
        }

    }

    public void Callback_ActivateStantask(int db_string_id, int standtask_id, string conn_json, string standtask_name = "")
    {
        ResetStandtask();
        current_db_string_id = db_string_id;
        //Debug.Log("standtask_id = " + standtask_id);
        SetCurrentStandtask(standtask_id, conn_json, standtask_name);
        messageManager.ShowMessage("Схема №" + standtask_id + " активирована и готова к сборке. Добавьте необходимые соединения на стенды");
    }

    /*public void Callback_CheckCompleteStantask()
    {

    }*/
}
