using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using System.IO;
using UnityEngine.UI;

public class RopeManager : MonoBehaviour {

    public int active_standtask_id; //id of activated standtask for student in main_standtask_state (dynamic) table on server
    public int standtask_id; //id of activated standtask for student in main_standtask_data (static) table on server
    public List<GameObject> CreatedRopes;
    //public Dictionary<int, int> correctConnectionsList = new Dictionary<int, int>();
    public Dictionary<string, string> correctConnectionsList = new Dictionary<string, string>();
    //public List<RopeJSONConnectionClass> correctConnectionsList = new List<RopeJSONConnectionClass>();
	
    public GameObject RopePrefabBigBig;
    public GameObject RopePrefabBigSmall;
    public GameObject RopePrefabSmallSmall;
	
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

    public int correctConnectionsCount = 0;
	public bool StandIsComplete = false;

    public GameObject CameraContainer;

    public bool clearMode;
    private WebSocket w;

    //public bool Need_InitNewSockets = false;

    // Use this for initialization
    void Start () {
		resetSocketsColor ();
	}

	void Update () {
        RefreshRopeRespownPos();
	}

    public string EncodeAllRopesToJSON()
    {
        List<string> allRopesList = new List<string>();
        string allRopesInJSON = "";
        //Get info from each existing rope
        for (int i = 0; i < RopeList.Count; i++)
        {
            int RopeType = RopeList[i].GetComponent<RopeClass>().RopeType;
            Vector3 PosA = RopeList[i].GetComponent<RopeClass>().pointA.transform.position;
            Vector3 PosB = RopeList[i].GetComponent<RopeClass>().pointB.transform.position;
            
            RopeJSONClass rope = new RopeJSONClass(RopeType, PosA, PosB);
            
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
        //string JSONArrayWithRopes = AllRopesFileReader("");
        Debug.Log("JSONArrayWithRopes = " + JSONArrayWithRopes);

        //Parse JSON string to array of string
        //List<string> 
        List<string> ArrayWithRopes = JsonConvert.DeserializeObject<List<string>>(JSONArrayWithRopes) ;// as List<string>;
        //Debug.Log("ArrayWithRopes = " + ArrayWithRopes[0] + "; " );
        Debug.Log("ArrayWithRopes.Count = " + ArrayWithRopes.Count);

        for(int i = 0; i < ArrayWithRopes.Count; i++){
            Debug.Log("ArrayWithRopes[" + i + "]" + ArrayWithRopes[i] + "\n");
            RopeJSONClass rope = JsonConvert.DeserializeObject<RopeJSONClass>(ArrayWithRopes[i]);
            //Debug.Log("i = " + i + "; PosA = " + rope.PosA + "; PosB = " + rope.PosB + "; Type = " + rope.RopeType + "\n");
            //Parse each sub-string as JSON to RopeClass object
            
            //RopeClass[] newRope = JSONArrayWithRopes["PosA"]
        
            //Create new rope in need place
            CreateNewRopeToPos(rope, true);
        }
    }

    public void SetCorrectConnectionsFromJSON(string JSONconnections)
    {
        Debug.Log("JSONArrayWithRopes = " + JSONconnections);

        List<ConnJSONClass> connList = JsonConvert.DeserializeObject<List<ConnJSONClass>>(JSONconnections);

        correctConnectionsList.Clear();

        foreach (ConnJSONClass conn in connList)
        {
            correctConnectionsList.Add(conn.A, conn.B);
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

                connectionsList.Add(new ConnJSONClass(ID1, ID2));
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
		correctConnectionsCount = 0;
		StandIsComplete = false;
		foreach (GameObject iSocket in SocketList) {
			iSocket.gameObject.GetComponent<SocketScript>().setNormalColor();
		}
	}

	public bool CheckStandtaskConnections(bool TeacherMode){
		if(TeacherMode){
            resetSocketsColor ();
        }

        StandIsComplete = false;
		
        correctConnectionsCount = 0;
		for (int i = 0; i < RopeList.Count; i++) {
			if(RopeList [i].GetComponent<RopeClass> ().connectedSocketList.Count == 2){
				string ID1 = RopeList [i].GetComponent<RopeClass> ().connectedSocketList[0].gameObject.GetComponent<SocketScript>().SocketID;
                string ID2 = RopeList[i].GetComponent<RopeClass>().connectedSocketList[1].gameObject.GetComponent<SocketScript>().SocketID;

				print("ID1 = " + ID1);
				print("ID2 = " + ID2);

                string correctID1 = "";
                string correctID2 = "";

                if (correctConnectionsList.TryGetValue(ID1, out correctID2))
				{
					if(ID2 == correctID2){
						
                        if(TeacherMode){
                            RopeList [i].GetComponent<RopeClass> ().connectedSocketList[0].gameObject.GetComponent<SocketScript>().setCorrectColor();
						    RopeList [i].GetComponent<RopeClass> ().connectedSocketList[1].gameObject.GetComponent<SocketScript>().setCorrectColor();
                        }

						correctConnectionsCount++;
						print("AB is correct");
					}else{
                        if (TeacherMode)
                        {
                            RopeList[i].GetComponent<RopeClass>().connectedSocketList[0].gameObject.GetComponent<SocketScript>().setErrorColor();
                            RopeList[i].GetComponent<RopeClass>().connectedSocketList[1].gameObject.GetComponent<SocketScript>().setErrorColor();
                        }

						print("AB is NOT correct");
					}
				}
                else if (correctConnectionsList.TryGetValue(ID2, out correctID1))
				{
					if(ID1 == correctID1){
                        if (TeacherMode)
                        {
                            RopeList[i].GetComponent<RopeClass>().connectedSocketList[0].gameObject.GetComponent<SocketScript>().setCorrectColor();
                            RopeList[i].GetComponent<RopeClass>().connectedSocketList[1].gameObject.GetComponent<SocketScript>().setCorrectColor();
                        }

						correctConnectionsCount++;
						print("BA is correct");
					}else{
                        if (TeacherMode)
                        {
                            RopeList[i].GetComponent<RopeClass>().connectedSocketList[0].gameObject.GetComponent<SocketScript>().setErrorColor();
                            RopeList[i].GetComponent<RopeClass>().connectedSocketList[1].gameObject.GetComponent<SocketScript>().setErrorColor();
                        }

						print("BA is NOT correct");
					}
				}
			}
		}

        print("correctConnections.Count = " + correctConnectionsList.Count / 2);
        print("correctConnectionsCount = " + correctConnectionsCount);

        if (correctConnectionsCount == correctConnectionsList.Count / 2)
        {
            StandIsComplete = true;
            print("Standtask was completed");

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

		for (int i = 0; i < RopeList.Count; i++) {
			if (RopeList[i].GetComponent<RopeClass> ().pointA.gameObject.GetComponent<Select> ().isSelected ||
			    RopeList[i].GetComponent<RopeClass> ().pointB.gameObject.GetComponent<Select> ().isSelected) {
				//Remove selected rope
				GameObject objForDelete = RopeList[i].gameObject;
				Destroy(objForDelete);
			}else{
				//Add unselected rope to new rope list
				newRopeList.Add(RopeList[i]);
			}
		}

		//Copy all unselected ropes from new to work list
		RopeList = newRopeList;
		//FindParent
		//Remove parent gameobject
	}

	public void RemoveRope(GameObject Rope){
		GameObject objForDelete = Rope.gameObject;
        objForDelete.GetComponent<RopeClass>().DetachAllPins();
        RopeList.Remove(objForDelete);
		Destroy(objForDelete);
	}

    public void RemoveAllRopes()
    {
        //Remove all ropes
        for (int i = 0; i < RopeList.Count; i++)
        {
            GameObject objForDelete = RopeList[i].gameObject;
            objForDelete.GetComponent<RopeClass>().DetachAllPins();
            Destroy(objForDelete);
        }

        //Clear list
        RopeList.Clear();// = new List<GameObject>();
    }

	public void FixSelectedPlugs(){
		for (int i = 0; i < RopeList.Count; i++) {
			if (RopeList [i].GetComponent<RopeClass> ().pointA.gameObject.GetComponent<Select> ().isSelected){
				RopeList [i].GetComponent<RopeClass> ().pointA.gameObject.GetComponent<Drag> ().isFix = true;
				RopeList [i].GetComponent<RopeClass> ().pointA.gameObject.GetComponent<Select> ().isSelected = false;
			}
			
			if (RopeList [i].GetComponent<RopeClass> ().pointB.gameObject.GetComponent<Select> ().isSelected) {
				RopeList [i].GetComponent<RopeClass> ().pointB.gameObject.GetComponent<Drag> ().isFix = true;
				RopeList [i].GetComponent<RopeClass> ().pointB.gameObject.GetComponent<Select> ().isSelected = false;
			}

			/*if (RopeList [i].GetComponent<RopeClass> ().pointA.gameObject.GetComponent<Select> ().isSelected){
				if (!RopeList [i].GetComponent<RopeClass> ().pointA.gameObject.GetComponent<Drag> ().isFix){
					RopeList [i].GetComponent<RopeClass> ().pointA.gameObject.GetComponent<Drag> ().isFix = true;
				}else{
					RopeList [i].GetComponent<RopeClass> ().pointA.gameObject.GetComponent<Drag> ().isFix = false;
				}

				RopeList [i].GetComponent<RopeClass> ().pointA.gameObject.GetComponent<Select> ().isSelected = false;
			}

			if (RopeList [i].GetComponent<RopeClass> ().pointB.gameObject.GetComponent<Select> ().isSelected) {
				if (!RopeList [i].GetComponent<RopeClass> ().pointB.gameObject.GetComponent<Drag> ().isFix){
					RopeList [i].GetComponent<RopeClass> ().pointB.gameObject.GetComponent<Drag> ().isFix = true;
				}else{
					RopeList [i].GetComponent<RopeClass> ().pointB.gameObject.GetComponent<Drag> ().isFix = false;
				}

				RopeList [i].GetComponent<RopeClass> ().pointB.gameObject.GetComponent<Select> ().isSelected = false;
			}*/
		}
		//FindParent
		//Remove parent gameobject
	}

	public void FreeSelectedPlugs(){
		for (int i = 0; i < RopeList.Count; i++) {
			if (RopeList [i].GetComponent<RopeClass> ().pointA.gameObject.GetComponent<Select> ().isSelected){
				RopeList [i].GetComponent<RopeClass> ().pointA.gameObject.GetComponent<Drag> ().isFix = false;
                RopeList[i].GetComponent<RopeClass>().pointA.gameObject.GetComponent<Attracted>().ReleaseAttractor();
				RopeList [i].GetComponent<RopeClass> ().pointA.gameObject.GetComponent<Select> ().isSelected = false;
			}
			
			if (RopeList [i].GetComponent<RopeClass> ().pointB.gameObject.GetComponent<Select> ().isSelected) {
				RopeList [i].GetComponent<RopeClass> ().pointB.gameObject.GetComponent<Drag> ().isFix = false;
                RopeList[i].GetComponent<RopeClass>().pointB.gameObject.GetComponent<Attracted>().ReleaseAttractor();
				RopeList [i].GetComponent<RopeClass> ().pointB.gameObject.GetComponent<Select> ().isSelected = false;
			}
		}
		//FindParent
		//Remove parent gameobject
	}

	public void SelectFreePlugs(){
		UnselectAllPlugs ();
		for (int i = 0; i < RopeList.Count; i++) {
			if (!RopeList [i].GetComponent<RopeClass> ().pointA.gameObject.GetComponent<Drag> ().isFix) {
				RopeList [i].GetComponent<RopeClass> ().pointA.gameObject.GetComponent<Select> ().isSelected = true;
			}
			if (!RopeList [i].GetComponent<RopeClass> ().pointB.gameObject.GetComponent<Drag> ().isFix) {
				RopeList [i].GetComponent<RopeClass> ().pointB.gameObject.GetComponent<Select> ().isSelected = true;
			}
		}
	}

	public void SelectFixedPlugs(){
		UnselectAllPlugs ();
		for (int i = 0; i < RopeList.Count; i++) {
			if (RopeList [i].GetComponent<RopeClass> ().pointA.gameObject.GetComponent<Drag> ().isFix){
				RopeList [i].GetComponent<RopeClass> ().pointA.gameObject.GetComponent<Select> ().isSelected = true;
			}
			if (RopeList [i].GetComponent<RopeClass> ().pointB.gameObject.GetComponent<Drag> ().isFix){
				RopeList [i].GetComponent<RopeClass> ().pointB.gameObject.GetComponent<Select> ().isSelected = true;
			}
		}
	}

	public void UnselectAllPlugs(){
		for (int i = 0; i < RopeList.Count; i++) {
			RopeList [i].GetComponent<RopeClass> ().pointA.gameObject.GetComponent<Select> ().isSelected = false;
			RopeList [i].GetComponent<RopeClass> ().pointB.gameObject.GetComponent<Select> ().isSelected = false;
		}
	}

	public void SelectAllPlugs(){
		for (int i = 0; i < RopeList.Count; i++) {
			RopeList [i].GetComponent<RopeClass> ().pointA.gameObject.GetComponent<Select> ().isSelected = true;
			RopeList [i].GetComponent<RopeClass> ().pointB.gameObject.GetComponent<Select> ().isSelected = true;
		}
	}

    public void CreateNewRopeToPos(RopeJSONClass rope, bool DropRope)
    {
        GameObject newRope = new GameObject();

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
        
        //Add all sockets from stand to new rope
        for (int i = 0; i < SocketList.Count; i++)
        {
            newRope.GetComponent<RopeClass>().availableSocketList.Add(SocketList[i].gameObject);
        }

        //Add DragPlane for new rope
        newRope.GetComponent<RopeClass>().DragPlane = this.DragPlane;
        newRope.GetComponent<RopeClass>().ropeManager = GetComponent<RopeManager>();
        
        //Init RopeClass links after create object rope prefab
        newRope.GetComponent<RopeClass>().InitAfterAdd();

        //Set rope pins to need positions
        newRope.GetComponent<RopeClass>().pointA.gameObject.transform.position = rope.PosA;
        newRope.GetComponent<RopeClass>().pointB.gameObject.transform.position = rope.PosB;

        //Drop all pins for fix to near sockets
        if(DropRope){
            newRope.GetComponent<RopeClass>().pointA.GetComponent<Drag>().isFix = false;
            newRope.GetComponent<RopeClass>().pointA.GetComponent<Drag>().isDropped = true;
            newRope.GetComponent<RopeClass>().pointA.GetComponent<Attracted>().ScanAttractors();
            newRope.GetComponent<RopeClass>().pointB.GetComponent<Drag>().isFix = false;
            newRope.GetComponent<RopeClass>().pointB.GetComponent<Drag>().isDropped = true;
            newRope.GetComponent<RopeClass>().pointB.GetComponent<Attracted>().ScanAttractors();
        }
        //newRope.GetComponent<RopeClass>().pointA.GetComponent<Attracted>().ScanAttractors();
        //newRope.GetComponent<RopeClass>().pointB.GetComponent<Attracted>().ScanAttractors();
        
        //Add new rope to RopeList
        RopeList.Add(newRope);

        //newRope.GetComponent<RopeClass> ().InitAfterAdd ();
        //CreatedRopes.Add (newRope);
    }

	public void CreateNewRope(int RopeType){

        GameObject newRope = new GameObject();
        
        if (RopeType == 0)
        {
            newRope = Instantiate(RopePrefabBigBig, ropeRespownPos, Quaternion.identity) as GameObject;
        }
        else if (RopeType == 1)
        {
            newRope = Instantiate(RopePrefabBigSmall, ropeRespownPos, Quaternion.identity) as GameObject;
        }
        else if (RopeType == 2)
        {
            newRope = Instantiate(RopePrefabSmallSmall, ropeRespownPos, Quaternion.identity) as GameObject;
        }
		//newRope.GetComponent<RopeClass> ().cable.GetComponent<UltimateRope> ().AfterImportedBonesObjectRespawn();

		//Add all sockets from stand to new rope
		for (int i = 0; i < SocketList.Count; i++) {
			newRope.GetComponent<RopeClass> ().availableSocketList.Add( SocketList[i].gameObject);
		}

		//Add DragPlane for new rope
		newRope.GetComponent<RopeClass> ().DragPlane = this.DragPlane;
		newRope.GetComponent<RopeClass> ().ropeManager = GetComponent<RopeManager>();
        //Init RopeClass links after create object rope prefab
        newRope.GetComponent<RopeClass>().InitAfterAdd();

		//Add new rope to RopeList
		RopeList.Add (newRope);

		//newRope.GetComponent<RopeClass> ().InitAfterAdd ();
		//CreatedRopes.Add (newRope);
	}

	/*public void MoveRopeToZero(){
		RopeList[0].gameObject.transform.position = new Vector3(0, 0, 0);
		RopeList [0].GetComponent<RopeClass> ().pointA.gameObject.transform.position = new Vector3 (-1f, -0.66f, 0f);
		RopeList [0].GetComponent<RopeClass> ().pointB.gameObject.transform.position = new Vector3 (-1f, 2.638f, 0f);
	}*/
}
