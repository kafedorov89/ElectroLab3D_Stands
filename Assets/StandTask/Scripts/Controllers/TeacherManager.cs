using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Newtonsoft.Json;

public class TeacherManager : MonoBehaviour {

    public List<string> standtaskList;
    public WebSocketManager webSocketManager;
    private RopeManager ropeManager;
    public CustomDropdown dropdownStandtaskList;
    public MessageManager messageManager;

    //GUI elements
    public Text StandtaskIDText;
    public Text StudentFullNameText;
    //public InputField StandtaskNameText;
    public Toggle StandtaskCompleteFlag;

    // Use this for initialization
	void Start () {
        //webSocketManager = GetComponent<WebSocketManager>();
        ropeManager = GetComponent<RopeManager>();
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void ResetFields()
    {
        StandtaskIDText.text = "";
        StudentFullNameText.text = "";
    }

    public void UpdateStandtaskList(){ //Execute when dropdown list enabling by MenuScript by GUI button (second action in button)
        Debug.Log("UpdateStandtaskList");
        //Send request to server for list with user's names, who doing standtasks
        webSocketManager.SendPackageToServer("GetStudentStandtaskList");
    }

    public void Callback_GetStudentStandtaskList(List<int> db_string_id, List<string> standtask_id_full_username_list)
    {
        foreach(int i in db_string_id) {
            Debug.Log("db_string_id[i] = " + i);
        }

        foreach (string s in standtask_id_full_username_list)
        {
            Debug.Log("standtask_id_full_username_list[i] = " + s);
        }
        
        Debug.Log("Callback_GetStudentStandtaskList");
        //Get list with active standtask from server
        //standtaskList = standtask_list;


        if (standtask_id_full_username_list.Count == db_string_id.Count)
        {
            dropdownStandtaskList.SetItemList(db_string_id, standtask_id_full_username_list);
        }
            //for(int i = 0; i < db_string_id.Count; i++){
                //dropdownStandtaskList.AddItem(db_string_id[i], standtask_id_full_username_list[i], true);
            //}
        //}
        //Update standtaskList
    }

    public void ShowStandtaskList()
    {
        Debug.Log("ShowStandtaskList");
        if (dropdownStandtaskList.gameObject.activeSelf)
        {
            //dropdownStandtaskList.RemoveItems();
            dropdownStandtaskList.HideAndClearListObject();
        }
        else
        {
            dropdownStandtaskList.gameObject.SetActive(true);
            UpdateStandtaskList();
            //TestUpdateDropdownList();
        }
    }

    public void Callback_GetStudentStandtask(
        string user_rope_json, 
        string conn_json, 
        string user_full_name,
        string standtask_name,                                     
        int standtask_id)
    {
        Debug.Log("Callback_GetStudentStandtask");
        
        ropeManager.CreateRopesFromJSON(user_rope_json);
        
        ropeManager.SetCorrectConnectionsFromJSON(conn_json);
        StandtaskIDText.text = standtask_id.ToString();
        StudentFullNameText.text = user_full_name;
    }

    public void DownloadSelectedStandTask(int db_string_id){
        Debug.Log("DownloadSelectedStandTask");
        
        //Request standtask by order db_string_id, saved in standtaskList (i)
        webSocketManager.SendPackageToServer("GetStudentStandtask", JsonConvert.SerializeObject(db_string_id));
    }

    public void CheckStandTask()
    {
        if (ropeManager.CheckStandtaskConnections(false))
        {
            messageManager.ShowMessage("Схема собрана правильно. Студент может приступать к выполнению работы!");
            StandtaskCompleteFlag.isOn = true;

        }
        else
        {
            messageManager.ShowMessage("Схема собрана не правильно. Студенту необходимо проверить соединения!");
            StandtaskCompleteFlag.isOn = false;
        }
    }

    /*public void TestUpdateDropdownList()
    {
        dropdownStandtaskList.RemoveItems();
        
        List<int> key_list = new List<int>();
        List<string> name_list = new List<string>();
        List<bool> enable_list = new List<bool>();

        //key_list.Clear();
        //name_list.Clear();
        //enable_list.Clear();
        
        for (int i = 0; i < 100; i++)
        {
            key_list.Add(i);
            name_list.Add("Лабораторная работа №" + i + " Студент по имени Олень");
            enable_list.Add(true);
            //dropdownStandtaskList.AddItem(i, "Лабораторная работа №" + i + " Студент по имени Олень", true);
        }

        //dropdownStandtaskList.
        dropdownStandtaskList.SetItemList(key_list, name_list, enable_list);
    }*/

}
