using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RopesScript : MonoBehaviour {

	List<GameObject> CreatedRopes;

	public GameObject BlueRopePrefab;
	public GameObject DragPlane;

	public List<GameObject> RopeList;
	public List<GameObject> SocketList;

	public GameObject DraggedPlug;
	public bool Dragging;

	// Use this for initialization
	void Start () {
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

	public bool CanDragging(GameObject testPlug){
		if (testPlug.gameObject == DraggedPlug.gameObject) {
			return true;
		} else {
			return false;
		}

		/*for (int i = 0; i < RopeList.Count; i++) {
			if (RopeList [i].gameObject.GetComponent<RopeManager> ().pointA.gameObject.GetComponent<Drag> ().isDraggedNow){
				print("Dragging = true");
				Dragging = true;
				//return false;
			}else{
				print("Dragging = false");
				Dragging = false;
				//return true;
			}
		}
		Dragging = false;
		//return false;*/
	}

	public void CreateNewRope(){
		GameObject newRope = Instantiate(BlueRopePrefab, Vector3.zero, Quaternion.identity) as GameObject;

		//Add all sockets from stand to new rope
		for (int i = 0; i < SocketList.Count; i++) {
			newRope.GetComponent<RopeManager> ().availableClips.Add( SocketList[i].gameObject);
		}

		//Add DragPlane for new rope
		newRope.GetComponent<RopeManager> ().DragPlane = this.DragPlane;

		newRope.GetComponent<RopeManager> ().InitAfterAdd ();
		//Add new rope to RopeList
		RopeList.Add (newRope);

		//newRope.GetComponent<RopeManager> ().InitAfterAdd ();
		//CreatedRopes.Add (newRope);
	}
}
