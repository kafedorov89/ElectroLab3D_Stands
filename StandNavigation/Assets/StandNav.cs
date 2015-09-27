using UnityEngine;
using System.Collections;

public class StandNav : MonoBehaviour {

	public float step = 10f;
	public float smoothing = 10f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(Input.GetKeyDown(KeyCode.W)){
			// Create a postion the camera is aiming for based on 
			// the offset from the target.
			Vector3 newCamPos = transform.position + Vector3.forward * step;
			
			// Smoothly interpolate between the camera's current 
			// position and it's target position.
			transform.position = Vector3.Lerp(transform.position, 
			                                   newCamPos,   
			                                   smoothing * Time.deltaTime);
		}
	}
}
