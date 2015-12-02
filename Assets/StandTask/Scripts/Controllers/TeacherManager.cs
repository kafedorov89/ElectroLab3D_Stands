using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class TeacherManager : MonoBehaviour {

    public List<string> standtaskList;
    public WebSocketManager webSocketManager;
    private RopeManager ropeManager;
    public CustomDropdown dropdownStandtaskList;
    
    // Use this for initialization
	void Start () {
        //webSocketManager = GetComponent<WebSocketManager>();
        ropeManager = GetComponent<RopeManager>();
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void UpdateStandtaskList(){ //Execute when dropdown list enabling by MenuScript by GUI button (second action in button)
        //Send request to server for list with user's names, who doing standtasks
        webSocketManager.SendPackageToServer("GetStudentStandtaskList");
    }

    public void Callback_GetStudentStandtaskList(List<string> standtask_id_full_username_list, List<int> db_string_id)
    {
        //Get list with active standtask from server
        //standtaskList = standtask_list;
        if(standtask_id_full_username_list.Count == db_string_id.Count){
            for(int i = 0; i < db_string_id.Count; i++){
                //dropdownStandtaskList.AddItem(db_string_id[i], standtask_id_full_username_list[i], true);
            }
        }
        //Update standtaskList
    }

    public void ShowStandtaskList()
    {
        if (dropdownStandtaskList.gameObject.activeSelf)
        {
            //dropdownStandtaskList.RemoveItems();
            dropdownStandtaskList.HideAndClearListObject();
        }
        else
        {
            dropdownStandtaskList.gameObject.SetActive(true);
            TestUpdateDropdownList();
        }

    }

    public void TestUpdateDropdownList()
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
    }

    public void Callback_GetStudentStandtask(string user_rope_json, string conn_json, int standtask_id, string full_username)
    {
        //Get user_rope_json for selected standtask from server
        //Add ropes from json package
        ropeManager.CreateRopesFromJSON(user_rope_json);
        
        //Get conn_json for selected stantask from server
        //Set current standtask number to GUI
        ropeManager.SetCurrentStandtask(standtask_id, conn_json);

        //Set current user's first_name and last_name to GUI
        
    }

    public void DownloadSelectedStandTask(int db_string_id){
        //Request standtask by order db_string_id, saved in standtaskList (i)
        webSocketManager.SendPackageToServer("GetStudentStandtask", JsonConvert.SerializeObject(db_string_id));
    }

    public void CheckStandtask()
    {
        //Get
    }
}
