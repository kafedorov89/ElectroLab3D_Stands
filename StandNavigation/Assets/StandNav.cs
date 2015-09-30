using UnityEngine;
using System.Collections;

public class StandNav : MonoBehaviour {

	public float step = 0.16f;
	public float smoothing = 10f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(Input.GetKey(KeyCode.W)){
			// Create a postion the camera is aiming for based on 
			// the offset from the target.
			var camForward = transform.rotation * Vector3.up; // rotate vector forward 45 degrees around Y
			
			Vector3 newCamPos = transform.position + camForward * step;
			
			// Smoothly interpolate between the camera's current 
			// position and it's target position.
			transform.position = Vector3.Lerp(transform.position, 
			                                  newCamPos,   
			                                  smoothing * Time.deltaTime);
		}
		
		if(Input.GetKey(KeyCode.A)){
			// Create a postion the camera is aiming for based on 
			// the offset from the target.
			var camForward = transform.rotation * Vector3.left; // rotate vector forward 45 degrees around Y
			
			Vector3 newCamPos = transform.position + camForward * step;
			
			// Smoothly interpolate between the camera's current 
			// position and it's target position.
			transform.position = Vector3.Lerp(transform.position, 
			                                  newCamPos,   
			                                  smoothing * Time.deltaTime);
		}
		
		if(Input.GetKey(KeyCode.D)){
			// Create a postion the camera is aiming for based on 
			// the offset from the target.
			var camForward = transform.rotation * Vector3.right; // rotate vector forward 45 degrees around Y
			
			Vector3 newCamPos = transform.position + camForward * step;
			
			// Smoothly interpolate between the camera's current 
			// position and it's target position.
			transform.position = Vector3.Lerp(transform.position, 
			                                  newCamPos,   
			                                  smoothing * Time.deltaTime);
		}
		
		if(Input.GetKey(KeyCode.S)){
			// Create a postion the camera is aiming for based on 
			// the offset from the target.
			var camForward = transform.rotation * Vector3.down; // rotate vector forward 45 degrees around Y
			
			Vector3 newCamPos = transform.position + camForward * step;
			
			// Smoothly interpolate between the camera's current 
			// position and it's target position.
			transform.position = Vector3.Lerp(transform.position, 
			                                  newCamPos,   
			                                  smoothing * Time.deltaTime);
		}



	}
}
