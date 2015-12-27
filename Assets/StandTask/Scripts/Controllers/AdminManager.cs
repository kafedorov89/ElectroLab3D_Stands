using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using System.IO;
using UnityEngine.UI;

//using System.Runtime.InteropServices;

public class AdminManager : MonoBehaviour {

    /*[DllImport("user32.dll")]
    private static extern void SaveFileDialog(); //in your case : OpenFileDialog
    private static extern void OpenFileDialog(); //in your case : OpenFileDialog*/

    public InputField SaveStandtaskID;
    public InputField CurrentStandtaskID;
    public InputField LoadStandtaskID;
    public RopeManager ropeManager;
    public WebSocketManager webSocketManager;
    public string FolderName;

    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void UploadAllStandtasksToDatabase(string folderName = "")
    {
        Debug.Log("UploadAllStandtasksToDatabase");
        string folderPath = "";
        string fileName = LoadStandtaskID.text;
        List<string> StandtaskList = new List<string>();
        //string JSONArrayWithRopes = "";

        //Debug.Log("Directory.GetCurrentDirectory" + Directory.GetCurrentDirectory());
        if (FolderName != "")
        {
            folderPath = Application.dataPath + "/../" + FolderName + "/";
        }
        else
        {
            folderPath = Application.dataPath + "/../";
        }
        
        DirectoryInfo info = new DirectoryInfo(folderPath);
        FileInfo[] fileInfo = info.GetFiles();
        foreach(FileInfo file in fileInfo){
            Debug.Log("file.FullName = " + file.FullName);
            //Add one standtask from file in json format to standtask list
            StandtaskList.Add(File.ReadAllText(file.FullName));
            //print (file);
        }

        //Send standtask list to server
        webSocketManager.SendPackageToServer("UploadAllSchemas", JsonConvert.SerializeObject(StandtaskList));
    }

    public void SaveFullStandtask(string folderName = "")
    {
        int standtask_id = 0;
        int.TryParse(SaveStandtaskID.text, out standtask_id);
        StandtaskJSONClass standtaskJSON = new StandtaskJSONClass(standtask_id, "", ropeManager.EncodeCurrentConnectionsToJSON(), ropeManager.EncodeAllRopesToJSON());

        string FullStantaskJSON = standtaskJSON.GetJSON();

        string fileName = "standtask_" + SaveStandtaskID.text + ".json";
        string folderPath;
        //Debug.Log("Directory.GetCurrentDirectory" + Directory.GetCurrentDirectory());
        if (FolderName != "")
        {
            folderPath = Application.dataPath + "/../" + FolderName + "/";
        }
        else
        {
            folderPath = Application.dataPath + "/../";
        }
        Debug.Log("folderPath: " + folderPath);
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
            Debug.Log("folder doesn't exist");
            Directory.CreateDirectory(folderName);
            Debug.Log("folder was created");
        }
        else
        {
            Debug.Log("folder alredy exist");
        }

        //Directory.CreateDirectory(folderName);

        if (File.Exists(fileName))
        {
            Debug.Log(fileName + " already exists.");
            return;
        }

        StreamWriter sr = File.CreateText(folderPath + fileName);

        sr.Write(FullStantaskJSON);
        //sr.WriteLine("This is my file.");
        //sr.WriteLine("I can write ints {0} or floats {1}, and so on.", 1, 4.2);
        sr.Close();
    }

    public void LoadFullStandtask(string folderName = "")
    {
        string folderPath = "";
        string fileName = "standtask_" + LoadStandtaskID.text + ".json";
        //string JSONArrayWithRopes = "";

        //Debug.Log("Directory.GetCurrentDirectory" + Directory.GetCurrentDirectory());
        if (FolderName != "")
        {
            folderPath = Application.dataPath + "/../" + FolderName + "/";
        }
        else
        {
            folderPath = Application.dataPath + "/../";
        }

        StandtaskJSONClass standtask = new StandtaskJSONClass(File.ReadAllText(folderPath + fileName));

        ropeManager.CreateRopesFromJSON(standtask.rope_json);
    }

    //public void SaveFullStandtask(string conn_json, string rope_json, int standtask_id, string folderName = "")

    /*
    public void SaveTempStandtask()
    {
        AllRopesTempFileWriter(ropeManager.EncodeAllRopesToJSON());
    }

    public void AllRopesTempFileWriter(string allRopesJSONContent, string folderName = "")
    {
        string folderPath = "";
        string fileName = "standtask_temp.json";
        //Debug.Log("Directory.GetCurrentDirectory" + Directory.GetCurrentDirectory());
        if (folderName != "")
        {
            folderPath = Application.dataPath + "/../" + folderName + "/";
        }
        else
        {
            folderPath = Application.dataPath + "/../";
        }

        Debug.Log("folderPath: " + folderPath);
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
            Debug.Log("folder doesn't exist");
            Directory.CreateDirectory(folderName);
            Debug.Log("folder was created");
        }
        else
        {
            Debug.Log("folder alredy exist");
        }

        //Directory.CreateDirectory(folderName);

        if (File.Exists(fileName))
        {

            Debug.Log(fileName + " already exists and will be removed");
            File.Delete(fileName);
            //return;
        }

        StreamWriter sr = File.CreateText(folderPath + fileName);
        sr.Write(allRopesJSONContent);
        //sr.WriteLine("This is my file.");
        //sr.WriteLine("I can write ints {0} or floats {1}, and so on.", 1, 4.2);
        sr.Close();
    }

    public void LoadTempStandTask(){
        ropeManager.CreateRopesFromJSON(AllRopesTempFileReader());
    }

    public string AllRopesTempFileReader(string folderName = "")
    {
        string folderPath = "";
        string fileName = "standtask_temp.json";
        string JSONArrayWithRopes = "";

        //Debug.Log("Directory.GetCurrentDirectory" + Directory.GetCurrentDirectory());
        if (folderName != "")
        {
            folderPath = Application.dataPath + "/../" + folderName + "/";
        }
        else
        {
            folderPath = Application.dataPath + "/../";
        }

        JSONArrayWithRopes = File.ReadAllText(folderPath + fileName);

        return JSONArrayWithRopes;
    }*/
    
}
