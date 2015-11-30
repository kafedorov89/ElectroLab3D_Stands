using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BreakCotroller : MonoBehaviour {

	public List<GameObject> RopeNodes;
	public bool Inited = false;
	public GameObject pointA;
	public GameObject pointB;
	public GameObject Ropes;
	public GameObject thisRope;

	public Vector3[] oldRopeState;
	public float distLimitMax = 50f;

	void SaveOldRopeState(){
		int i = 0;
		foreach (GameObject ropenode in RopeNodes) {
			oldRopeState[i] = ropenode.transform.position;
			i++;
		}
	}

	// Use this for initialization
	void Start () {
		if (!Inited) {
			foreach (Transform b in this.transform) {
				if (b != null && b.gameObject != null)
					RopeNodes.Add(b.gameObject);
			}

			oldRopeState = new Vector3[RopeNodes.Count];
			SaveOldRopeState();
			Inited = true;
		}
	}
	
	// Update is called once per frame
	void Update () {

        //Old function for BreakController
		/*
        int i = 0;
		foreach (GameObject ropenode in RopeNodes) {
			float dist = Vector3.Distance(oldRopeState[i], ropenode.transform.position);
			if (dist > distLimitMax){
				//pointA.gameObject.GetComponent<Drag>().isDraggedNow = false;
				//pointB.gameObject.GetComponent<Drag>().isDraggedNow = false;

				Ropes.GetComponent<RopeManager>().RemoveRope(thisRope);

				print ("Dangerous plug moving speed!!!");
			}
		}

		SaveOldRopeState ();
        */
	}
}
