using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class RopesManager : MonoBehaviour {

	public List<GameObject> CreatedRopes;
    public Dictionary<int, int> correctConnectionsList = new Dictionary<int, int>();
	
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

    private WebSocket w;
    //public bool Need_InitNewSockets = false;

    // Use this for initialization
    void Start () {
        /*if (Need_InitNewSockets) {
            InitNewSockets();
        }*/

        //ropeRespownOffset = new Vector3(-0.33f, 0.22f, 0.72f);
        setCorrectConnections ();
		resetSocketsColor ();

        //DEBUG StartCoroutine (ConnectToWebSocket());
        //Dragging = false;
	}

    // Update is called once per frame
	void Update () {
        RefreshRopeRespownPos();
	}

    public void InitNewSockets()
    {
        SocketList.Clear();
        SocketScript[] socketScriptList = SocketsFolder.GetComponentsInChildren<SocketScript>();
        for (int i = 0; i < socketScriptList.Length; i++)
        {
            print(socketScriptList[i].transform.name);
            socketScriptList[i].SocketID = i;
            SocketList.Add(socketScriptList[i].transform.gameObject);
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

    
    public void loadConnections(){

    }
    
    public void saveConnections() {
        for (int i = 0; i < RopeList.Count; i++) {
            if (RopeList[i].GetComponent<RopeScript>().connectedSocketList.Count == 2) {
                int ID1 = RopeList[i].GetComponent<RopeScript>().connectedSocketList[0].gameObject.GetComponent<SocketScript>().SocketID;
                int ID2 = RopeList[i].GetComponent<RopeScript>().connectedSocketList[1].gameObject.GetComponent<SocketScript>().SocketID;
            }
        }
    }


	public void checkConnections(){
		resetSocketsColor ();
		StandIsComplete = false;
		correctConnectionsCount = 0;
		for (int i = 0; i < RopeList.Count; i++) {
			if(RopeList [i].GetComponent<RopeScript> ().connectedSocketList.Count == 2){
				int ID1 = RopeList [i].GetComponent<RopeScript> ().connectedSocketList[0].gameObject.GetComponent<SocketScript>().SocketID;
				int ID2 = RopeList [i].GetComponent<RopeScript> ().connectedSocketList[1].gameObject.GetComponent<SocketScript>().SocketID;

				print("ID1 = " + ID1);
				print("ID2 = " + ID2);

				int correctID1 = -1;
				int correctID2 = -1;

                if (correctConnectionsList.TryGetValue(ID1, out correctID2))
				{
					if(ID2 == correctID2){
						RopeList [i].GetComponent<RopeScript> ().connectedSocketList[0].gameObject.GetComponent<SocketScript>().setCorrectColor();
						RopeList [i].GetComponent<RopeScript> ().connectedSocketList[1].gameObject.GetComponent<SocketScript>().setCorrectColor();
						correctConnectionsCount++;
						print("AB is correct");
					}else{
						RopeList [i].GetComponent<RopeScript> ().connectedSocketList[0].gameObject.GetComponent<SocketScript>().setErrorColor();
						RopeList [i].GetComponent<RopeScript> ().connectedSocketList[1].gameObject.GetComponent<SocketScript>().setErrorColor();
						print("AB is NOT correct");
					}
				}
                else if (correctConnectionsList.TryGetValue(ID2, out correctID1))
				{
					if(ID1 == correctID1){
						RopeList [i].GetComponent<RopeScript> ().connectedSocketList[0].gameObject.GetComponent<SocketScript>().setCorrectColor();
						RopeList [i].GetComponent<RopeScript> ().connectedSocketList[1].gameObject.GetComponent<SocketScript>().setCorrectColor();
						correctConnectionsCount++;
						print("BA is correct");
					}else{
						RopeList [i].GetComponent<RopeScript> ().connectedSocketList[0].gameObject.GetComponent<SocketScript>().setErrorColor();
						RopeList [i].GetComponent<RopeScript> ().connectedSocketList[1].gameObject.GetComponent<SocketScript>().setErrorColor();
						print("BA is NOT correct");
					}
				}
			}
		}

        print("correctConnections.Count = " + correctConnectionsList.Count / 2);
        print("correctConnectionsCount = " + correctConnectionsCount);


		if (correctConnectionsCount == correctConnectionsList.Count/2) {
			StandIsComplete = true;
			print("Stand is COMPLETE");
            w.SendString("Stand is COMPLETE");
		}
	}

	public void setCorrectConnections(){
		//Here should be parse information about correct connections for current stand from database

        correctConnectionsList.Add(0, 1);
        correctConnectionsList.Add(1, 0);

        correctConnectionsList.Add(2, 3);
        correctConnectionsList.Add(3, 2);

        correctConnectionsList.Add(4, 5);
        correctConnectionsList.Add(5, 4);

        correctConnectionsList.Add(6, 7);
        correctConnectionsList.Add(7, 6);

        correctConnectionsList.Add(8, 9);
        correctConnectionsList.Add(9, 8);
	}
	
	public void RemoveSelectedRopes(){
		/*foreach (GameObject objForDelete in RopeList) {
			if (objForDelete.GetComponent<RopeScript> ().pointA.gameObject.GetComponent<Select> ().isSelected ||
			    objForDelete.GetComponent<RopeScript> ().pointB.gameObject.GetComponent<Select> ().isSelected) {
				//GameObject objForDelete = RopeList[i].gameObject;
				RopeList.Remove(objForDelete);
				Destroy(objForDelete);
			}
		}*/
		List<GameObject> newRopeList = new List<GameObject>();

		for (int i = 0; i < RopeList.Count; i++) {
			if (RopeList[i].GetComponent<RopeScript> ().pointA.gameObject.GetComponent<Select> ().isSelected ||
			    RopeList[i].GetComponent<RopeScript> ().pointB.gameObject.GetComponent<Select> ().isSelected) {
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
		RopeList.Remove(objForDelete);
		Destroy(objForDelete);
	}

    public void RemoveAllRopes()
    {
        //Remove all ropes
        for (int i = 0; i < RopeList.Count; i++)
        {
            GameObject objForDelete = RopeList[i].gameObject;
            Destroy(objForDelete);
        }

        //Clear list
        RopeList.Clear();// = new List<GameObject>();
    }

	public void FixSelectedPlugs(){
		for (int i = 0; i < RopeList.Count; i++) {
			if (RopeList [i].GetComponent<RopeScript> ().pointA.gameObject.GetComponent<Select> ().isSelected){
				RopeList [i].GetComponent<RopeScript> ().pointA.gameObject.GetComponent<Drag> ().isFix = true;
				RopeList [i].GetComponent<RopeScript> ().pointA.gameObject.GetComponent<Select> ().isSelected = false;
			}
			
			if (RopeList [i].GetComponent<RopeScript> ().pointB.gameObject.GetComponent<Select> ().isSelected) {
				RopeList [i].GetComponent<RopeScript> ().pointB.gameObject.GetComponent<Drag> ().isFix = true;
				RopeList [i].GetComponent<RopeScript> ().pointB.gameObject.GetComponent<Select> ().isSelected = false;
			}

			/*if (RopeList [i].GetComponent<RopeScript> ().pointA.gameObject.GetComponent<Select> ().isSelected){
				if (!RopeList [i].GetComponent<RopeScript> ().pointA.gameObject.GetComponent<Drag> ().isFix){
					RopeList [i].GetComponent<RopeScript> ().pointA.gameObject.GetComponent<Drag> ().isFix = true;
				}else{
					RopeList [i].GetComponent<RopeScript> ().pointA.gameObject.GetComponent<Drag> ().isFix = false;
				}

				RopeList [i].GetComponent<RopeScript> ().pointA.gameObject.GetComponent<Select> ().isSelected = false;
			}

			if (RopeList [i].GetComponent<RopeScript> ().pointB.gameObject.GetComponent<Select> ().isSelected) {
				if (!RopeList [i].GetComponent<RopeScript> ().pointB.gameObject.GetComponent<Drag> ().isFix){
					RopeList [i].GetComponent<RopeScript> ().pointB.gameObject.GetComponent<Drag> ().isFix = true;
				}else{
					RopeList [i].GetComponent<RopeScript> ().pointB.gameObject.GetComponent<Drag> ().isFix = false;
				}

				RopeList [i].GetComponent<RopeScript> ().pointB.gameObject.GetComponent<Select> ().isSelected = false;
			}*/
		}
		//FindParent
		//Remove parent gameobject
	}

	public void FreeSelectedPlugs(){
		for (int i = 0; i < RopeList.Count; i++) {
			if (RopeList [i].GetComponent<RopeScript> ().pointA.gameObject.GetComponent<Select> ().isSelected){
				RopeList [i].GetComponent<RopeScript> ().pointA.gameObject.GetComponent<Drag> ().isFix = false;
                RopeList[i].GetComponent<RopeScript>().pointA.gameObject.GetComponent<Attracted>().ReleaseAttractor();
				RopeList [i].GetComponent<RopeScript> ().pointA.gameObject.GetComponent<Select> ().isSelected = false;
			}
			
			if (RopeList [i].GetComponent<RopeScript> ().pointB.gameObject.GetComponent<Select> ().isSelected) {
				RopeList [i].GetComponent<RopeScript> ().pointB.gameObject.GetComponent<Drag> ().isFix = false;
                RopeList[i].GetComponent<RopeScript>().pointB.gameObject.GetComponent<Attracted>().ReleaseAttractor();
				RopeList [i].GetComponent<RopeScript> ().pointB.gameObject.GetComponent<Select> ().isSelected = false;
			}
		}
		//FindParent
		//Remove parent gameobject
	}

	public void SelectFreePlugs(){
		UnselectAllPlugs ();
		for (int i = 0; i < RopeList.Count; i++) {
			if (!RopeList [i].GetComponent<RopeScript> ().pointA.gameObject.GetComponent<Drag> ().isFix) {
				RopeList [i].GetComponent<RopeScript> ().pointA.gameObject.GetComponent<Select> ().isSelected = true;
			}
			if (!RopeList [i].GetComponent<RopeScript> ().pointB.gameObject.GetComponent<Drag> ().isFix) {
				RopeList [i].GetComponent<RopeScript> ().pointB.gameObject.GetComponent<Select> ().isSelected = true;
			}
		}
	}

	public void SelectFixedPlugs(){
		UnselectAllPlugs ();
		for (int i = 0; i < RopeList.Count; i++) {
			if (RopeList [i].GetComponent<RopeScript> ().pointA.gameObject.GetComponent<Drag> ().isFix){
				RopeList [i].GetComponent<RopeScript> ().pointA.gameObject.GetComponent<Select> ().isSelected = true;
			}
			if (RopeList [i].GetComponent<RopeScript> ().pointB.gameObject.GetComponent<Drag> ().isFix){
				RopeList [i].GetComponent<RopeScript> ().pointB.gameObject.GetComponent<Select> ().isSelected = true;
			}
		}
	}

	public void UnselectAllPlugs(){
		for (int i = 0; i < RopeList.Count; i++) {
			RopeList [i].GetComponent<RopeScript> ().pointA.gameObject.GetComponent<Select> ().isSelected = false;
			RopeList [i].GetComponent<RopeScript> ().pointB.gameObject.GetComponent<Select> ().isSelected = false;
		}
	}

	public void SelectAllPlugs(){
		for (int i = 0; i < RopeList.Count; i++) {
			RopeList [i].GetComponent<RopeScript> ().pointA.gameObject.GetComponent<Select> ().isSelected = true;
			RopeList [i].GetComponent<RopeScript> ().pointB.gameObject.GetComponent<Select> ().isSelected = true;
		}
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
		//newRope.GetComponent<RopeScript> ().cable.GetComponent<UltimateRope> ().AfterImportedBonesObjectRespawn();

		//Add all sockets from stand to new rope
		for (int i = 0; i < SocketList.Count; i++) {
			newRope.GetComponent<RopeScript> ().availableSocketList.Add( SocketList[i].gameObject);
		}

		//Add DragPlane for new rope
		newRope.GetComponent<RopeScript> ().DragPlane = this.DragPlane;
		newRope.GetComponent<RopeScript> ().Ropes = this.gameObject;

		newRope.GetComponent<RopeScript> ().InitAfterAdd ();
		//Add new rope to RopeList
		RopeList.Add (newRope);

		//newRope.GetComponent<RopeScript> ().InitAfterAdd ();
		//CreatedRopes.Add (newRope);
	}

	public void MoveRopeToZero(){
		RopeList[0].gameObject.transform.position = new Vector3(0, 0, 0);
		RopeList [0].GetComponent<RopeScript> ().pointA.gameObject.transform.position = new Vector3 (-1f, -0.66f, 0f);
		RopeList [0].GetComponent<RopeScript> ().pointB.gameObject.transform.position = new Vector3 (-1f, 2.638f, 0f);
	}
}
