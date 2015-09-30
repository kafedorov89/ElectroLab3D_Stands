using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RopesScript : MonoBehaviour {

	List<GameObject> CreatedRopes;

	public GameObject BlueRopePrefab;
	public GameObject DragPlane;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void CreateNewRope(){
		GameObject newRope = Instantiate(BlueRopePrefab, Vector3.zero, Quaternion.identity) as GameObject;
		newRope.GetComponent<RopeManager> ().DragPlane = this.DragPlane;
		newRope.GetComponent<RopeManager> ().InitAfterAdd ();
		//CreatedRopes.Add (newRope);
	}
}
