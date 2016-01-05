using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using System.IO;
using UnityEngine.UI;
using System.Linq;
public class RopeManager : MonoBehaviour {

    public int active_standtask_id; //id of activated standtask for student in main_standtask_state (dynamic) table on server
    //public int standtask_id; //id of activated standtask for student in main_standtask_data (static) table on server
    
    public List<GameObject> CreatedRopes;
    //public Dictionary<int, int> correctConnectionsList = new Dictionary<int, int>();
    public Dictionary<string, string> correctConnectionsList = new Dictionary<string, string>();
    //public List<RopeJSONConnectionClass> correctConnectionsList = new List<RopeJSONConnectionClass>();
	
    public GameObject RopePrefabBigBig;
    public GameObject RopePrefabBigSmall;
    public GameObject RopePrefabSmallSmall;
    public GameObject RopePrefabBridge;
	
    public GameObject DragPlane;
	public List<GameObject> RopeList;
	public List<GameObject> SocketList;
    public GameObject SocketsFolder;
	public GameObject DraggedPlug;
	public bool Dragging;
    //public Vector3 ropeRespownOffset = new Vector3(-1.65f, 2.44f, 1.08f);
    //public Vector3 ropeRespownOffset = new Vector3(-0.33f, 0.22f, 0.72f);
    public Vector3 ropeRespownOffset;// = new Vector3();
    public Vector3 ropeRespownPos;// = new Vector3();

	public bool StandIsComplete = false;

    public GameObject CameraContainer;

    public bool clearMode;
    private WebSocket w;
    private WebSocketManager webSocketManager;

    //public bool Need_InitNewSockets = false;

    // Use this for initialization

    void Awake()
    {
        webSocketManager = FindObjectOfType<WebSocketManager>();
    }
    
    void Start () {
		resetSocketsColor ();
	}

	void Update () {
        RefreshRopeRespownPos();
	}

    //Записать изменения о положении проводов в базу данных
    public void UpdateUserRopesToDatebase()
    {
        if (active_standtask_id > 0)
        {
            webSocketManager.SendPackageToServer("UploadStudentRopes", JsonConvert.SerializeObject(new string[] { JsonConvert.SerializeObject(EncodeAllRopesToJSON()), active_standtask_id.ToString() }));
        }
        else
        {
            //Debug.Log("Update Ropes is imposible. Standtask doesn't activated");
        }
    }

    public string ClearReceivedArray(string ReceivedStringArray)
    {
        Debug.Log("ReceivedStringArray[0] = " + ReceivedStringArray[0]);
        Debug.Log("ReceivedStringArray.Length = " + ReceivedStringArray.Length);

        if (ReceivedStringArray[0] != '[' || ReceivedStringArray.Length == 0) //Если строка пустая или правильно начинается то возвращаем ее без изменений
        {
            String workString = ReceivedStringArray;
            Debug.Log("workString.Length = " + workString.Length);

            while (workString.Length > 4)
            {
                workString = workString.Substring(1, workString.Length - 2).ToString();

                if (workString[0] == '[')
                    break;

                if (workString.Length > 0)
                    Debug.Log(workString);
            }

            Debug.Log("workString = " + workString.ToString());
            return workString.ToString();
        }
        else
        {
            Debug.Log("ReceivedStringArray = " + ReceivedStringArray);
            return ReceivedStringArray;
        }
    }

    public string EncodeAllRopesToJSON()
    {
        List<string> allRopesList = new List<string>();
        string allRopesInJSON = "";
        //Get info from each existing rope
        Debug.Log("RopeList.Count = " + RopeList.Count);

        for (int i = 0; i < RopeList.Count; i++)
        {
            int RopeType = RopeList[i].GetComponent<RopeClass>().RopeType;
            Vector3 PosA = RopeList[i].GetComponent<RopeClass>().pointA.transform.position;
            Vector3 PosB = RopeList[i].GetComponent<RopeClass>().pointB.transform.position;
            Color RopeColor = RopeList[i].GetComponent<RopeClass>().RopeColor;
            
            RopeJSONClass rope = new RopeJSONClass(RopeType, PosA, PosB, new Vector3(RopeColor.r, RopeColor.g, RopeColor.b));

            //Convert to JSON string
            string ropeInJSON = JsonConvert.SerializeObject(rope);
            allRopesList.Add(ropeInJSON);
        }
        
        allRopesInJSON = JsonConvert.SerializeObject(allRopesList);
        Debug.Log("allRopesInJSON = " + allRopesInJSON);

        return allRopesInJSON;
        //Create array of string and return as one JSON string
    }

    public void CreateRopesFromJSON(string JSONArrayWithRopes)//string JSONArrayWithRopes)
    {
        RemoveAllRopes();
        //string JSONArrayWithRopes = AllRopesFileReader("");
        //Debug.Log("JSONArrayWithRopes = " + JSONArrayWithRopes);
        JSONArrayWithRopes = ClearReceivedArray(JSONArrayWithRopes);
        //Parse JSON string to array of string
        //List<string>

        if (JSONArrayWithRopes.Length > 0 && JSONArrayWithRopes[0] == '[')
        {
            //JSONArrayWithRopes = JSONArrayWithRopes.Substring(1, JSONArrayWithRopes.Length - 2);
            //Debug.Log("JSONArrayWithRopes = " + JSONArrayWithRopes);

            List<string> ArrayWithRopes = JsonConvert.DeserializeObject<List<string>>(JSONArrayWithRopes);// as List<string>;
            
            //Debug.Log("ArrayWithRopes = " + ArrayWithRopes[0] + "; " );
            if (ArrayWithRopes != null)
            {
                //Debug.Log("ArrayWithRopes.Count = " + ArrayWithRopes.Count);

                for (int i = 0; i < ArrayWithRopes.Count; i++)
                {
                    //Debug.Log("ArrayWithRopes[" + i + "]" + ArrayWithRopes[i] + "\n");
                    RopeJSONClass rope = JsonConvert.DeserializeObject<RopeJSONClass>(ArrayWithRopes[i]);
                    //Debug.Log("i = " + i + "; PosA = " + rope.PosA + "; PosB = " + rope.PosB + "; Type = " + rope.RopeType + "\n");
                    //Parse each sub-string as JSON to RopeClass object

                    //RopeClass[] newRope = JSONArrayWithRopes["PosA"]

                    //Create new rope in need place
                    CreateNewRopeToPos(rope, true, false);
                }
            }

            UpdateUserRopesToDatebase();
        }
        else
        {
            Debug.Log("Received Empty Ropes List");
        }
    }

    public void SetCorrectConnectionsFromJSON(String JSONconnections)
    {
        correctConnectionsList.Clear();
        
        JSONconnections = ClearReceivedArray(JSONconnections);

        if (JSONconnections.Length > 0 && JSONconnections[0] == '[')
        {
            List<ConnJSONClass> connList = JsonConvert.DeserializeObject<List<ConnJSONClass>>(JSONconnections);
            //List<ConnJSONClass>[] connList = JsonConvert.DeserializeObject<List<ConnJSONClass>[]>(JSONconnections);
            //List<string> connList = JsonConvert.DeserializeObject<List<string>>(JSONconnections);
            //string[] connList = JsonConvert.DeserializeObject<string[]>(JSONconnections);
            //ConnJSONClass[] connList = JsonConvert.DeserializeObject<ConnJSONClass[]>(JSONconnections);

            /*foreach (string conn in connList)
            {
                print("conn: " + conn);
            }*/

            //correctConnectionsList.Clear();

            if (connList != null)
            {
                foreach (ConnJSONClass conn in connList)
                {
                    correctConnectionsList.Add(conn.A, conn.B);
                }
            }
            else
            {
                Debug.Log("Received Empty correct Connections List");
            }
        }
        else
        {
            Debug.Log("Received Empty correct Connections List");
        }
    }

    public string EncodeCurrentConnectionsToJSON()
    {
        List<ConnJSONClass> connectionsList = new List<ConnJSONClass>();
        string JSONconnectionList = "";

        for (int i = 0; i < RopeList.Count; i++)
        {
            if (RopeList[i].GetComponent<RopeClass>().connectedSocketList.Count == 2)
            {
                string ID1 = RopeList[i].GetComponent<RopeClass>().connectedSocketList[0].gameObject.GetComponent<SocketScript>().SocketID;
                string ID2 = RopeList[i].GetComponent<RopeClass>().connectedSocketList[1].gameObject.GetComponent<SocketScript>().SocketID;

                Debug.Log("Save connection");
                connectionsList.Add(new ConnJSONClass(ID1, ID2));
            }
            else
            {
                Debug.Log("Rope doesn't save becase haven't 2 connected sockets");
            }
        }

        //Convert all connections to JSON string
        JSONconnectionList = JsonConvert.SerializeObject(connectionsList);
        Debug.Log("JSONconnectionList = " + JSONconnectionList);

        return JSONconnectionList;

        //int standtaskNumber = 0;
        //int.TryParse(SaveStandtaskNumber.text, out standtaskNumber);

        //Save connections to file with standtaskID number
    }

    public void ClearSockets()
    {
        if (clearMode)
        {
            SocketList.Clear(); //Uncomment if doesn't want to save old SocketID numbers;
        }
    }

    public void InitNewSockets()
    {
        //SocketList.Clear(); //Uncomment if doesn't want to save old SocketID numbers;
        bool isOldSocket = false;
        SocketScript[] socketScriptList = SocketsFolder.GetComponentsInChildren<SocketScript>();
        for (int i = 0; i < socketScriptList.Length; i++)
        {
            // Check existing socket
            for (int k = 0; k < SocketList.Count; k++){
                SocketScript existingSocket = SocketList[k].GetComponentInChildren<SocketScript>() as SocketScript;
                
                if(socketScriptList[i].SocketID == existingSocket.SocketID){
                    isOldSocket = true;
                    break;
                }
            }

            // If it is realy new socket
            if (!isOldSocket)
            {
                print(socketScriptList[i].transform.name);
                //Add new UID to socket
                socketScriptList[i].SocketID = Guid.NewGuid().ToString();
                //Add new socket to SocketList
                SocketList.Add(socketScriptList[i].transform.gameObject);
            }
        }

        /*foreach (SocketScript socketScriptObj in socketScriptList)
        {
        }*/
    }

    void RefreshRopeRespownPos()
    {
        ropeRespownPos = new Vector3(ropeRespownOffset.x, CameraContainer.transform.position.y + ropeRespownOffset.y, CameraContainer.transform.position.z + ropeRespownOffset.z);
    }

    public void resetSocketsColor(){
		//correctConnectionsCount = 0;
		StandIsComplete = false;
		foreach (GameObject iSocket in SocketList) {
			iSocket.gameObject.GetComponent<SocketScript>().setNormalColor();
		}
	}

    public void markErrorRope(GameObject rope)
    {
        rope.GetComponent<RopeClass>().connectedSocketList[0].gameObject.GetComponent<SocketScript>().setErrorColor();
        rope.GetComponent<RopeClass>().connectedSocketList[1].gameObject.GetComponent<SocketScript>().setErrorColor();

        rope.GetComponent<RopeClass>().pointA.GetComponent<Select>().setErrorColor();
        rope.GetComponent<RopeClass>().pointB.GetComponent<Select>().setErrorColor();
    }

    public void markCorrectRope(GameObject rope)
    {
        rope.GetComponent<RopeClass>().connectedSocketList[0].gameObject.GetComponent<SocketScript>().setCorrectColor();
        rope.GetComponent<RopeClass>().connectedSocketList[1].gameObject.GetComponent<SocketScript>().setCorrectColor();

        rope.GetComponent<RopeClass>().pointA.GetComponent<Select>().setCorrectColor();
        rope.GetComponent<RopeClass>().pointB.GetComponent<Select>().setCorrectColor();
    }

	public bool CheckStandtaskConnections(bool TeacherMode){
		if(TeacherMode){
            resetSocketsColor ();
        }

        StandIsComplete = false;

		int correctConnectionsCount = 0;
        int errorConnectonsCount = 0;

        Debug.Log("RopeList.Count = " + RopeList.Count);
        Debug.Log("correctConnectionsList.Count = " + correctConnectionsList.Count);

		for (int i = 0; i < RopeList.Count; i++) {

            bool isCorrectRope = false;
            string correctID1 = "";
            string correctID2 = "";

            string ID1 = RopeList[i].GetComponent<RopeClass>().connectedSocketList[0].gameObject.GetComponent<SocketScript>().SocketID;
            string ID2 = RopeList[i].GetComponent<RopeClass>().connectedSocketList[1].gameObject.GetComponent<SocketScript>().SocketID;

			if(RopeList [i].GetComponent<RopeClass> ().connectedSocketList.Count == 2){

                
                //print("ID1 = " + ID1);
				//print("ID2 = " + ID2);
                if (correctConnectionsList.TryGetValue(ID1, out correctID2))
				{
					if(ID2 == correctID2){
                        if (TeacherMode) 
                        {
                            markCorrectRope(RopeList[i]);
                        }

						correctConnectionsCount++;
                        isCorrectRope = true;
						print("AB is correct");
					}
                    else
                    {
                        if (TeacherMode)
                        {
                            markErrorRope(RopeList[i]);
                        }

                        errorConnectonsCount++;
						print("AB is NOT correct");
					}
				}
                else if (correctConnectionsList.TryGetValue(ID2, out correctID1))
				{
					if(ID1 == correctID1){
                        if (TeacherMode)
                        {
                            markCorrectRope(RopeList[i]);
                        }

						correctConnectionsCount++;
                        isCorrectRope = true;
						print("BA is correct");
					}
                    else
                    {
                        if (TeacherMode)
                        {
                            markErrorRope(RopeList[i]);
                        }

                        errorConnectonsCount++;
						print("BA is NOT correct");
					}
                }

                if (TeacherMode)
                {
                    if (!isCorrectRope)
                    {
                        markErrorRope(RopeList[i]);
                    }

                }
			}

            if (TeacherMode)
            {
                List<string> usedSocketUIDList = correctConnectionsList.Keys.ToList();
                usedSocketUIDList.AddRange(correctConnectionsList.Values.ToList());

                foreach (string uid in usedSocketUIDList)
                {
                    foreach (GameObject socket in SocketList)
                    {
                        if (socket.GetComponent<SocketScript>().SocketID == uid && socket.GetComponent<SocketScript>().pluggedPinList.Count == 0)
                        {
                            socket.GetComponent<SocketScript>().setErrorColor();
                        }
                    }
                }
            }
		}

        print("Waiting Correct connections = " + correctConnectionsList.Count);
        print("User correct connections = " + correctConnectionsCount);
        print("Error connectons = " + errorConnectonsCount);

        //bool RopeCountCorrect = (RopeList.Count == correctConnectionsCount);
        bool ConnectionsCorrect = (correctConnectionsCount > 0) && (correctConnectionsCount == correctConnectionsList.Count) && (errorConnectonsCount == 0) && (RopeList.Count == correctConnectionsCount);

        
        if (ConnectionsCorrect)
        {
            StandIsComplete = true;
            print("Standtask was completed");
            correctConnectionsList.Clear();

            return true;
        }
        else
        {
            print("Standtask is not complete");
            return false;
        }
	}
	
	public void RemoveSelectedRopes(){
		/*foreach (GameObject objForDelete in RopeList) {
			if (objForDelete.GetComponent<RopeClass> ().pointA.gameObject.GetComponent<Select> ().isSelected ||
			    objForDelete.GetComponent<RopeClass> ().pointB.gameObject.GetComponent<Select> ().isSelected) {
				//GameObject objForDelete = RopeList[i].gameObject;
				RopeList.Remove(objForDelete);
				Destroy(objForDelete);
			}
		}*/
		List<GameObject> newRopeList = new List<GameObject>();
        List<GameObject> RemoveRopeList = new List<GameObject>();
        

		for (int i = 0; i < RopeList.Count; i++) {
			if (RopeList[i].GetComponent<RopeClass> ().pointA.gameObject.GetComponent<Select> ().isSelected ||
			    RopeList[i].GetComponent<RopeClass> ().pointB.gameObject.GetComponent<Select> ().isSelected) {
				//Add selected rope to remove list
				GameObject objForDelete = RopeList[i].gameObject;
                RemoveRopeList.Add(objForDelete);
			}else{
				//Add unselected rope to new rope list
                Debug.Log("Add unselected rope to new list");
                newRopeList.Add(RopeList[i]);
			}
		}

        for (int i = 0; i < RemoveRopeList.Count; i++)
        {
            RemoveRope(RemoveRopeList[i], false);
        }

		//Copy all unselected ropes from new to work list
		RopeList = newRopeList;
		//FindParent
		//Remove parent gameobject
        UpdateUserRopesToDatebase();
	}

    public void ReattractedAnotherPins(GameObject socket)
    {
        Debug.Log("Connected socket was found");
        List<GameObject> TempOtherPinList = socket.GetComponent<SocketScript>().pluggedPinList;
        socket.GetComponent<SocketScript>().pluggedPinList.Clear();
        foreach (GameObject otherpin in TempOtherPinList)
        {
            Debug.Log("Release attractor for another pin on realesed from removing pin socket");
            otherpin.GetComponent<Attracted>().ReleaseAttractor(false);
        }

        UpdateUserRopesToDatebase();
    }

	public void RemoveRope(GameObject Rope, bool update){
        GameObject objForDelete = Rope.gameObject;

        //Reattracting another pins which attracted to removing pin's socket 
        //ReattractedAnotherPins(objForDelete.GetComponent<RopeClass>().pointA.GetComponent<Attracted>().currentAttractor);
        //ReattractedAnotherPins(objForDelete.GetComponent<RopeClass>().pointB.GetComponent<Attracted>().currentAttractor);

        objForDelete.GetComponent<RopeClass>().DetachAllPins(false);
        RopeList.Remove(objForDelete);
		Destroy(objForDelete);

        if (update)
            UpdateUserRopesToDatebase();
	}

    public void RemoveAllRopes()
    {
        foreach (GameObject socketObject in SocketList)
        {
            socketObject.GetComponent<SocketScript>().pluggedPinList.Clear();
        }
        
        //Remove all ropes
        for (int i = 0; i < RopeList.Count; i++)
        {
            GameObject objForDelete = RopeList[i].gameObject;
            objForDelete.GetComponent<RopeClass>().DetachAllPins(false);
            Destroy(objForDelete);
        }

        //Clear list
        RopeList.Clear();// = new List<GameObject>();

        UpdateUserRopesToDatebase();
    }

	public void FixSelectedPlugs(){
		for (int i = 0; i < RopeList.Count; i++) {
			if (RopeList [i].GetComponent<RopeClass> ().pointA.gameObject.GetComponent<Select> ().isSelected){
				RopeList [i].GetComponent<RopeClass> ().pointA.gameObject.GetComponent<Drag> ().isFix = true;
				RopeList [i].GetComponent<RopeClass> ().pointA.gameObject.GetComponent<Select> ().SelectPin(false);
			}
			
			if (RopeList [i].GetComponent<RopeClass> ().pointB.gameObject.GetComponent<Select> ().isSelected) {
				RopeList [i].GetComponent<RopeClass> ().pointB.gameObject.GetComponent<Drag> ().isFix = true;
                RopeList[i].GetComponent<RopeClass>().pointB.gameObject.GetComponent<Select>().SelectPin(false);
			}
		}
	}

	public void FreeSelectedPlugs(){
		for (int i = 0; i < RopeList.Count; i++) {
			if (RopeList [i].GetComponent<RopeClass> ().pointA.gameObject.GetComponent<Select> ().isSelected){

                //ReattractedAnotherPins(RopeList[i].GetComponent<RopeClass>().pointA.GetComponent<Attracted>().currentAttractor);
                

                RopeList [i].GetComponent<RopeClass> ().pointA.gameObject.GetComponent<Drag> ().isFix = false;
                RopeList[i].GetComponent<RopeClass>().pointA.gameObject.GetComponent<Attracted>().ReleaseAttractor(false);
                RopeList[i].GetComponent<RopeClass>().pointA.gameObject.GetComponent<Select>().SelectPin(false);
			}
			
			if (RopeList [i].GetComponent<RopeClass> ().pointB.gameObject.GetComponent<Select> ().isSelected) {

                //ReattractedAnotherPins(RopeList[i].GetComponent<RopeClass>().pointB.GetComponent<Attracted>().currentAttractor);

                RopeList [i].GetComponent<RopeClass> ().pointB.gameObject.GetComponent<Drag> ().isFix = false;
                RopeList[i].GetComponent<RopeClass>().pointB.gameObject.GetComponent<Attracted>().ReleaseAttractor(false);
                RopeList[i].GetComponent<RopeClass>().pointB.gameObject.GetComponent<Select>().SelectPin(false);
			}
		}
		//FindParent
        //Remove parent gameobject

        UpdateUserRopesToDatebase();
    }

	public void SelectFreePlugs(){
		UnselectAllPlugs ();
        for (int i = 0; i < RopeList.Count; i++)
        {
            if (!RopeList[i].GetComponent<RopeClass>().pointA.gameObject.GetComponent<Drag>().isFix)
            {
                RopeList[i].GetComponent<RopeClass>().pointA.gameObject.GetComponent<Select>().SelectPin(true);
            }
            if (!RopeList[i].GetComponent<RopeClass>().pointB.gameObject.GetComponent<Drag>().isFix)
            {
                RopeList[i].GetComponent<RopeClass>().pointB.gameObject.GetComponent<Select>().SelectPin(true);
            }
        }
	}

	public void SelectFixedPlugs(){
		UnselectAllPlugs ();
		for (int i = 0; i < RopeList.Count; i++) {
			if (RopeList [i].GetComponent<RopeClass> ().pointA.gameObject.GetComponent<Drag> ().isFix){
				RopeList [i].GetComponent<RopeClass> ().pointA.gameObject.GetComponent<Select> ().SelectPin(true);
			}
			if (RopeList [i].GetComponent<RopeClass> ().pointB.gameObject.GetComponent<Drag> ().isFix){
				RopeList [i].GetComponent<RopeClass> ().pointB.gameObject.GetComponent<Select> ().SelectPin(true);
			}
		}
	}

	public void UnselectAllPlugs(){
		for (int i = 0; i < RopeList.Count; i++) {
			RopeList [i].GetComponent<RopeClass> ().pointA.gameObject.GetComponent<Select> ().SelectPin(false);
			RopeList [i].GetComponent<RopeClass> ().pointB.gameObject.GetComponent<Select> ().SelectPin(false);
		}
	}

	public void SelectAllPlugs(){
		for (int i = 0; i < RopeList.Count; i++) {
			RopeList [i].GetComponent<RopeClass> ().pointA.gameObject.GetComponent<Select> ().SelectPin(true);
			RopeList [i].GetComponent<RopeClass> ().pointB.gameObject.GetComponent<Select> ().SelectPin(true);
		}
	}

    public void CreateNewRopeToPos(RopeJSONClass rope, bool DropRope, bool update)
    {
        GameObject newRope = null;// = new GameObject();

        //Create rope with need type
        if (rope.RopeType == 0)
        {
            newRope = Instantiate(RopePrefabBigBig, new Vector3(), Quaternion.identity) as GameObject;
        }
        else if (rope.RopeType == 1)
        {
            newRope = Instantiate(RopePrefabBigSmall, new Vector3(), Quaternion.identity) as GameObject;
        }
        else if (rope.RopeType == 2)
        {
            newRope = Instantiate(RopePrefabSmallSmall, new Vector3(), Quaternion.identity) as GameObject;
        }
        else if (rope.RopeType == 3)
        {
            newRope = Instantiate(RopePrefabBridge, ropeRespownPos, Quaternion.identity) as GameObject;
        }

		newRope.SetActive (true);
        
        //Add all sockets from stand to new rope
        for (int i = 0; i < SocketList.Count; i++)
        {
            newRope.GetComponent<RopeClass>().availableSocketList.Add(SocketList[i].gameObject);
        }

        //Add DragPlane for new rope
        newRope.GetComponent<RopeClass>().DragPlane = this.DragPlane;
        newRope.GetComponent<RopeClass>().ropeManager = GetComponent<RopeManager>();
        
        //Init RopeClass links after create object rope prefab
		newRope.GetComponent<RopeClass>().InitAfterAdd(true, new Color(rope.RopeColor.x, rope.RopeColor.y, rope.RopeColor.z)); //Init rope with saved color from file or from database

        //Set rope pins to need positions
        newRope.GetComponent<RopeClass>().pointA.gameObject.transform.position = rope.PosA;
        newRope.GetComponent<RopeClass>().pointB.gameObject.transform.position = rope.PosB;

        //Drop all pins for fix to near sockets
        if(DropRope){
            if (!newRope.GetComponent<RopeClass>().pointA.GetComponent<Attracted>().AttractWithOther)
            {
                newRope.GetComponent<RopeClass>().pointA.GetComponent<Drag>().isFix = false;
                newRope.GetComponent<RopeClass>().pointA.GetComponent<Drag>().isDropped = true;
                newRope.GetComponent<RopeClass>().pointA.GetComponent<Attracted>().ScanAttractors(false, false);

                newRope.GetComponent<RopeClass>().pointB.GetComponent<Drag>().isFix = false;
                newRope.GetComponent<RopeClass>().pointB.GetComponent<Drag>().isDropped = true;
                newRope.GetComponent<RopeClass>().pointB.GetComponent<Attracted>().ScanAttractors(false, false);
            }
            else
            {
                newRope.GetComponent<RopeClass>().pointA.GetComponent<Drag>().isFix = false;
                newRope.GetComponent<RopeClass>().pointA.GetComponent<Drag>().isDropped = true;
                newRope.GetComponent<RopeClass>().pointA.GetComponent<Attracted>().ScanAttractors(false, false);
            }
        }
        //newRope.GetComponent<RopeClass>().pointA.GetComponent<Attracted>().ScanAttractors();
        //newRope.GetComponent<RopeClass>().pointB.GetComponent<Attracted>().ScanAttractors();
        
        //Add new rope to RopeList
        RopeList.Add(newRope);
        
        if(update)
            UpdateUserRopesToDatebase();
        //newRope.GetComponent<RopeClass> ().InitAfterAdd ();
        //CreatedRopes.Add (newRope);
    }

	public void CreateNewRope(int RopeType){

        GameObject newRope = null;
        
        if (RopeType == 0)
        {
            newRope = Instantiate(RopePrefabBigBig, ropeRespownPos, Quaternion.identity) as GameObject;//);  ;
        }
        else if (RopeType == 1)
        {
            newRope = Instantiate(RopePrefabBigSmall, ropeRespownPos, Quaternion.identity) as GameObject;
        }
        else if (RopeType == 2)
        {
            newRope = Instantiate(RopePrefabSmallSmall, ropeRespownPos, Quaternion.identity) as GameObject;
        }
        else if (RopeType == 3)
        {
            newRope = Instantiate(RopePrefabBridge, ropeRespownPos, Quaternion.identity) as GameObject;
        }

		newRope.SetActive (true);

		//newRope.GetComponent<RopeClass> ().cable.GetComponent<UltimateRope> ().AfterImportedBonesObjectRespawn();

		//Add all sockets from stand to new rope
		for (int i = 0; i < SocketList.Count; i++) {
			newRope.GetComponent<RopeClass> ().availableSocketList.Add( SocketList[i].gameObject);
		}

		//Add DragPlane for new rope
		newRope.GetComponent<RopeClass> ().DragPlane = this.DragPlane;
		newRope.GetComponent<RopeClass> ().ropeManager = GetComponent<RopeManager>();
        //Init RopeClass links after create object rope prefab
        newRope.GetComponent<RopeClass>().InitAfterAdd(false, Color.white); //Init new Rope with random color

		//Add new rope to RopeList
		RopeList.Add (newRope);
        
        UpdateUserRopesToDatebase();
		//newRope.GetComponent<RopeClass> ().InitAfterAdd ();
		//CreatedRopes.Add (newRope);
	}

	/*public void MoveRopeToZero(){
		RopeList[0].gameObject.transform.position = new Vector3(0, 0, 0);
		RopeList [0].GetComponent<RopeClass> ().pointA.gameObject.transform.position = new Vector3 (-1f, -0.66f, 0f);
		RopeList [0].GetComponent<RopeClass> ().pointB.gameObject.transform.position = new Vector3 (-1f, 2.638f, 0f);
	}*/
}
