using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

public class RopesScript : MonoBehaviour {

	public List<GameObject> CreatedRopes;

	public Dictionary<int, int> correctConnections = new Dictionary<int, int>();

	public GameObject BlueRopePrefab;
	public GameObject DragPlane;

	public List<GameObject> RopeList;
	public List<GameObject> SocketList;

	public GameObject DraggedPlug;
	public bool Dragging;

	public int correctConnectionsCount = 0;
	public bool StandIsComplete = false;

	// Use this for initialization
	void Start () {
		setCorrectConnections ();
		resetSocketsColor ();
		//Dragging = false;
	}
	
	// Update is called once per frame
	void Update () {
		/*for (int i = 0; i < RopeList.Count; i++) {
			if (RopeList [i].gameObject.GetComponent<RopeManager> ().pointA.gameObject.GetComponent<Drag> ().isDraggedNow){
				print("Dragging = true");
				Dragging = true;
				//return false;
			//}else{
			//	print("Dragging = false");
			//	Dragging = false;
				//return true;
			}
		}
		Dragging = false;*/
	}

	public void resetSocketsColor(){
		correctConnectionsCount = 0;
		StandIsComplete = false;
		foreach (GameObject iSocket in SocketList) {
			iSocket.gameObject.GetComponent<ClipScript>().setNormalColor();
		}
	}

	public void checkConnections(){
		resetSocketsColor ();
		StandIsComplete = false;
		correctConnectionsCount = 0;
		for (int i = 0; i < RopeList.Count; i++) {
			if(RopeList [i].GetComponent<RopeManager> ().connectedClips.Count == 2){
				int ID1 = RopeList [i].GetComponent<RopeManager> ().connectedClips[0].gameObject.GetComponent<ClipScript>().ClipID;
				int ID2 = RopeList [i].GetComponent<RopeManager> ().connectedClips[1].gameObject.GetComponent<ClipScript>().ClipID;

				print("ID1 = " + ID1);
				print("ID2 = " + ID2);

				int correctID1 = -1;
				int correctID2 = -1;

				if(correctConnections.TryGetValue(ID1, out correctID2))
				{
					if(ID2 == correctID2){
						RopeList [i].GetComponent<RopeManager> ().connectedClips[0].gameObject.GetComponent<ClipScript>().setCorrectColor();
						RopeList [i].GetComponent<RopeManager> ().connectedClips[1].gameObject.GetComponent<ClipScript>().setCorrectColor();
						correctConnectionsCount++;
						print("AB is correct");
					}else{
						RopeList [i].GetComponent<RopeManager> ().connectedClips[0].gameObject.GetComponent<ClipScript>().setErrorColor();
						RopeList [i].GetComponent<RopeManager> ().connectedClips[1].gameObject.GetComponent<ClipScript>().setErrorColor();
						print("AB is NOT correct");
					}
				}
				else if (correctConnections.TryGetValue(ID2, out correctID1))
				{
					if(ID1 == correctID1){
						RopeList [i].GetComponent<RopeManager> ().connectedClips[0].gameObject.GetComponent<ClipScript>().setCorrectColor();
						RopeList [i].GetComponent<RopeManager> ().connectedClips[1].gameObject.GetComponent<ClipScript>().setCorrectColor();
						correctConnectionsCount++;
						print("BA is correct");
					}else{
						RopeList [i].GetComponent<RopeManager> ().connectedClips[0].gameObject.GetComponent<ClipScript>().setErrorColor();
						RopeList [i].GetComponent<RopeManager> ().connectedClips[1].gameObject.GetComponent<ClipScript>().setErrorColor();
						print("BA is NOT correct");
					}
				}
			}
		}

		if (correctConnectionsCount == correctConnections.Count) {
			StandIsComplete = true;
			print("Stand is COMPLETE");
		}
	}

	public void setCorrectConnections(){
		//Here should be parse information about correct connections for current stand from database

		correctConnections.Add (0, 1);
		correctConnections.Add (1, 0);

		correctConnections.Add (2, 3);
		correctConnections.Add (3, 2);

		correctConnections.Add (4, 5);
		correctConnections.Add (5, 4);

		correctConnections.Add (6, 7);
		correctConnections.Add (7, 6);

		correctConnections.Add (8, 9);
		correctConnections.Add (9, 8);
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
		GameObject newRope = Instantiate(BlueRopePrefab, new Vector3(-1.210022f, 0f, 0f), Quaternion.identity) as GameObject;
		newRope.GetComponent<RopeManager> ().cable.GetComponent<UltimateRope> ().AfterImportedBonesObjectRespawn();

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
