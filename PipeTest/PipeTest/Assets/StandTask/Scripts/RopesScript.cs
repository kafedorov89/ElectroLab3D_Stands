using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class RopesScript : MonoBehaviour {

	public List<GameObject> CreatedRopes;
    public Dictionary<int, int> correctConnectionsList = new Dictionary<int, int>();
	public GameObject BlueRopePrefab;
	public GameObject DragPlane;
	public List<GameObject> RopeList;
	public List<GameObject> SocketList;
	public GameObject DraggedPlug;
	public bool Dragging;
    //public Vector3 ropeRespownOffset = new Vector3(-1.65f, 2.44f, 1.08f);
    public Vector3 ropeRespownOffset = new Vector3(-2.03f, -1.64f, 1.08f);
    public Vector3 ropeRespownPos = new Vector3();

	public int correctConnectionsCount = 0;
	public bool StandIsComplete = false;

    public GameObject CameraContainer;

    private WebSocket w;

	// Use this for initialization
	void Start () {
		setCorrectConnections ();
		resetSocketsColor ();

        //DEBUG StartCoroutine (ConnectToWebSocket());
        //Dragging = false;
	}

    IEnumerator ConnectToWebSocket(string ip_address = "192.168.1.206", string port = "8888", string controller = "/ws")
    {
        w = new WebSocket(new Uri("ws://" + ip_address + ":" + port + controller));
        yield return StartCoroutine(w.Connect());
        w.SendString("UnityTest");
        //int i = 0;
        while (true)
        {
            string reply = w.RecvString();
            if (reply != null)
            {
                Debug.Log("Received: " + reply); //Show received message from server
                //w.SendString("UnityTest " + i++); //Send something to server avter received message
            }
            if (w.Error != null)
            {
                Debug.LogError("Error: " + w.Error);
                //break;
            }
            yield return 0;
        }
        w.Close();
    }


    void RefreshRopeRespownPos()
    {
        ropeRespownPos = new Vector3(-2.03f, CameraContainer.transform.position.y + ropeRespownOffset.y, CameraContainer.transform.position.z + ropeRespownOffset.z);
    }

    // Update is called once per frame
	void Update () {
        RefreshRopeRespownPos();
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
            if (RopeList[i].GetComponent<RopeManager>().connectedClips.Count == 2) {
                int ID1 = RopeList[i].GetComponent<RopeManager>().connectedClips[0].gameObject.GetComponent<SocketScript>().ClipID;
                int ID2 = RopeList[i].GetComponent<RopeManager>().connectedClips[1].gameObject.GetComponent<SocketScript>().ClipID;


            }
        }
    }


	public void checkConnections(){
		resetSocketsColor ();
		StandIsComplete = false;
		correctConnectionsCount = 0;
		for (int i = 0; i < RopeList.Count; i++) {
			if(RopeList [i].GetComponent<RopeManager> ().connectedClips.Count == 2){
				int ID1 = RopeList [i].GetComponent<RopeManager> ().connectedClips[0].gameObject.GetComponent<SocketScript>().ClipID;
				int ID2 = RopeList [i].GetComponent<RopeManager> ().connectedClips[1].gameObject.GetComponent<SocketScript>().ClipID;

				print("ID1 = " + ID1);
				print("ID2 = " + ID2);

				int correctID1 = -1;
				int correctID2 = -1;

                if (correctConnectionsList.TryGetValue(ID1, out correctID2))
				{
					if(ID2 == correctID2){
						RopeList [i].GetComponent<RopeManager> ().connectedClips[0].gameObject.GetComponent<SocketScript>().setCorrectColor();
						RopeList [i].GetComponent<RopeManager> ().connectedClips[1].gameObject.GetComponent<SocketScript>().setCorrectColor();
						correctConnectionsCount++;
						print("AB is correct");
					}else{
						RopeList [i].GetComponent<RopeManager> ().connectedClips[0].gameObject.GetComponent<SocketScript>().setErrorColor();
						RopeList [i].GetComponent<RopeManager> ().connectedClips[1].gameObject.GetComponent<SocketScript>().setErrorColor();
						print("AB is NOT correct");
					}
				}
                else if (correctConnectionsList.TryGetValue(ID2, out correctID1))
				{
					if(ID1 == correctID1){
						RopeList [i].GetComponent<RopeManager> ().connectedClips[0].gameObject.GetComponent<SocketScript>().setCorrectColor();
						RopeList [i].GetComponent<RopeManager> ().connectedClips[1].gameObject.GetComponent<SocketScript>().setCorrectColor();
						correctConnectionsCount++;
						print("BA is correct");
					}else{
						RopeList [i].GetComponent<RopeManager> ().connectedClips[0].gameObject.GetComponent<SocketScript>().setErrorColor();
						RopeList [i].GetComponent<RopeManager> ().connectedClips[1].gameObject.GetComponent<SocketScript>().setErrorColor();
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
			if (objForDelete.GetComponent<RopeManager> ().pointA.gameObject.GetComponent<Select> ().isSelected ||
			    objForDelete.GetComponent<RopeManager> ().pointB.gameObject.GetComponent<Select> ().isSelected) {
				//GameObject objForDelete = RopeList[i].gameObject;
				RopeList.Remove(objForDelete);
				Destroy(objForDelete);
			}
		}*/
		List<GameObject> newRopeList = new List<GameObject>();

		for (int i = 0; i < RopeList.Count; i++) {
			if (RopeList[i].GetComponent<RopeManager> ().pointA.gameObject.GetComponent<Select> ().isSelected ||
			    RopeList[i].GetComponent<RopeManager> ().pointB.gameObject.GetComponent<Select> ().isSelected) {
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
			if (RopeList [i].GetComponent<RopeManager> ().pointA.gameObject.GetComponent<Select> ().isSelected){
				RopeList [i].GetComponent<RopeManager> ().pointA.gameObject.GetComponent<Drag> ().isFix = true;
				RopeList [i].GetComponent<RopeManager> ().pointA.gameObject.GetComponent<Select> ().isSelected = false;
			}
			
			if (RopeList [i].GetComponent<RopeManager> ().pointB.gameObject.GetComponent<Select> ().isSelected) {
				RopeList [i].GetComponent<RopeManager> ().pointB.gameObject.GetComponent<Drag> ().isFix = true;
				RopeList [i].GetComponent<RopeManager> ().pointB.gameObject.GetComponent<Select> ().isSelected = false;
			}

			/*if (RopeList [i].GetComponent<RopeManager> ().pointA.gameObject.GetComponent<Select> ().isSelected){
				if (!RopeList [i].GetComponent<RopeManager> ().pointA.gameObject.GetComponent<Drag> ().isFix){
					RopeList [i].GetComponent<RopeManager> ().pointA.gameObject.GetComponent<Drag> ().isFix = true;
				}else{
					RopeList [i].GetComponent<RopeManager> ().pointA.gameObject.GetComponent<Drag> ().isFix = false;
				}

				RopeList [i].GetComponent<RopeManager> ().pointA.gameObject.GetComponent<Select> ().isSelected = false;
			}

			if (RopeList [i].GetComponent<RopeManager> ().pointB.gameObject.GetComponent<Select> ().isSelected) {
				if (!RopeList [i].GetComponent<RopeManager> ().pointB.gameObject.GetComponent<Drag> ().isFix){
					RopeList [i].GetComponent<RopeManager> ().pointB.gameObject.GetComponent<Drag> ().isFix = true;
				}else{
					RopeList [i].GetComponent<RopeManager> ().pointB.gameObject.GetComponent<Drag> ().isFix = false;
				}

				RopeList [i].GetComponent<RopeManager> ().pointB.gameObject.GetComponent<Select> ().isSelected = false;
			}*/
		}
		//FindParent
		//Remove parent gameobject
	}

	public void FreeSelectedPlugs(){
		for (int i = 0; i < RopeList.Count; i++) {
			if (RopeList [i].GetComponent<RopeManager> ().pointA.gameObject.GetComponent<Select> ().isSelected){
				RopeList [i].GetComponent<RopeManager> ().pointA.gameObject.GetComponent<Drag> ().isFix = false;
				RopeList [i].GetComponent<RopeManager> ().pointA.gameObject.GetComponent<Select> ().isSelected = false;
			}
			
			if (RopeList [i].GetComponent<RopeManager> ().pointB.gameObject.GetComponent<Select> ().isSelected) {
				RopeList [i].GetComponent<RopeManager> ().pointB.gameObject.GetComponent<Drag> ().isFix = false;
				RopeList [i].GetComponent<RopeManager> ().pointB.gameObject.GetComponent<Select> ().isSelected = false;
			}
		}
		//FindParent
		//Remove parent gameobject
	}

	public void SelectFreePlugs(){
		UnselectAllPlugs ();
		for (int i = 0; i < RopeList.Count; i++) {
			if (!RopeList [i].GetComponent<RopeManager> ().pointA.gameObject.GetComponent<Drag> ().isFix) {
				RopeList [i].GetComponent<RopeManager> ().pointA.gameObject.GetComponent<Select> ().isSelected = true;
			}
			if (!RopeList [i].GetComponent<RopeManager> ().pointB.gameObject.GetComponent<Drag> ().isFix) {
				RopeList [i].GetComponent<RopeManager> ().pointB.gameObject.GetComponent<Select> ().isSelected = true;
			}
		}
	}

	public void SelectFixedPlugs(){
		UnselectAllPlugs ();
		for (int i = 0; i < RopeList.Count; i++) {
			if (RopeList [i].GetComponent<RopeManager> ().pointA.gameObject.GetComponent<Drag> ().isFix){
				RopeList [i].GetComponent<RopeManager> ().pointA.gameObject.GetComponent<Select> ().isSelected = true;
			}
			if (RopeList [i].GetComponent<RopeManager> ().pointB.gameObject.GetComponent<Drag> ().isFix){
				RopeList [i].GetComponent<RopeManager> ().pointB.gameObject.GetComponent<Select> ().isSelected = true;
			}
		}
	}

	public void UnselectAllPlugs(){
		for (int i = 0; i < RopeList.Count; i++) {
			RopeList [i].GetComponent<RopeManager> ().pointA.gameObject.GetComponent<Select> ().isSelected = false;
			RopeList [i].GetComponent<RopeManager> ().pointB.gameObject.GetComponent<Select> ().isSelected = false;
		}
	}

	public void SelectAllPlugs(){
		for (int i = 0; i < RopeList.Count; i++) {
			RopeList [i].GetComponent<RopeManager> ().pointA.gameObject.GetComponent<Select> ().isSelected = true;
			RopeList [i].GetComponent<RopeManager> ().pointB.gameObject.GetComponent<Select> ().isSelected = true;
		}
	}

	public void CreateNewRope(){
        
        
        GameObject newRope = Instantiate(BlueRopePrefab, ropeRespownPos, Quaternion.identity) as GameObject;
		//newRope.GetComponent<RopeManager> ().cable.GetComponent<UltimateRope> ().AfterImportedBonesObjectRespawn();

		//Add all sockets from stand to new rope
		for (int i = 0; i < SocketList.Count; i++) {
			newRope.GetComponent<RopeManager> ().availableClips.Add( SocketList[i].gameObject);
		}

		//Add DragPlane for new rope
		newRope.GetComponent<RopeManager> ().DragPlane = this.DragPlane;
		newRope.GetComponent<RopeManager> ().Ropes = this.gameObject;

		newRope.GetComponent<RopeManager> ().InitAfterAdd ();
		//Add new rope to RopeList
		RopeList.Add (newRope);

		//newRope.GetComponent<RopeManager> ().InitAfterAdd ();
		//CreatedRopes.Add (newRope);
	}

	public void MoveRopeToZero(){
		RopeList[0].gameObject.transform.position = new Vector3(0, 0, 0);
		RopeList [0].GetComponent<RopeManager> ().pointA.gameObject.transform.position = new Vector3 (-1f, -0.66f, 0f);
		RopeList [0].GetComponent<RopeManager> ().pointB.gameObject.transform.position = new Vector3 (-1f, 2.638f, 0f);
	}
}
