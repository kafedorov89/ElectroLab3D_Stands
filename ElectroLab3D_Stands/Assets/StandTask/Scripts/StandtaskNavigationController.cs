using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StandtaskNavigationController : MonoBehaviour {

	public float step = 0.16f;
	public float smoothing = 10f;

    private Vector2 mouseOldPosition = new Vector2();
    private Vector2 mousePosition = new Vector2();
    private Vector2 mouseDeltaPosition = new Vector2();
    public bool KeyboardControlEnabled = false;

    public float leftPositionOffset;
    public float rightPositionOffset;
    public float upPositionOffset;
    public float downPositionOffset;

    public Vector3[] PresetCameraPosList;
    public int CurrentPreset;

    public float leftPosition;
    public float rightPosition;
    public float upPosition;
    public float downPosition;

    public float xMinLimit;
    public float xMaxLimit;

    void CalcCameraPositionsOffset()
    {
        leftPositionOffset = PresetCameraPosList[CurrentPreset].z + leftPosition;
        rightPositionOffset = PresetCameraPosList[CurrentPreset].z + rightPosition;
        upPositionOffset = PresetCameraPosList[CurrentPreset].y + upPosition;
        downPositionOffset = PresetCameraPosList[CurrentPreset].y + downPosition;
    }
    
    void ParallelMoving()
    {
        if (Input.GetMouseButtonDown(2))
        {
            Debug.Log("Start Moving...");
            mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            mouseOldPosition = mousePosition;
            mouseDeltaPosition = mousePosition - mouseOldPosition;
        }

        if (Input.GetMouseButtonUp(2))
        {
            Debug.Log("Stop Moving...");
            mousePosition = new Vector2();
            mouseOldPosition = new Vector2();
            mouseDeltaPosition = new Vector2();
        }

        if (Input.GetMouseButton(2))
        {
            //Debug.Log("Moving...");

            if (mouseDeltaPosition.magnitude >= step)
            {
                //var camForward = transform.rotation * Vector3.up; // rotate vector forward 45 degrees around Y
                //var camForward = transform.rotation * Vector3.left; // rotate vector forward 45 degrees around Y
                //var camForward = transform.rotation * Vector3.right; // rotate vector forward 45 degrees around Y
                //var camForward = transform.rotation * Vector3.right; // rotate vector forward 45 degrees around Y
                //mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                //Vector3 newCamPos = transform.position + camForward * step;

                // Smoothly interpolate between the camera's current 
                // position and it's target position.

                transform.position = Vector3.Lerp(transform.position,
                                                  new Vector3(transform.position.x, transform.position.y + 0.0001f * mouseDeltaPosition.y, transform.position.z - 0.0001f * mouseDeltaPosition.x),
                                                  smoothing * Time.deltaTime);

                Debug.Log("Moving...");
                mouseDeltaPosition = new Vector2();
            }
            else
            {
                mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                mouseDeltaPosition = mousePosition - mouseOldPosition;
            }
        }
    }

    // Use this for initialization
    void Start () {
        CalcCameraPositionsOffset();
        //xMinLimit = -12f;
        //xMaxLimit = -1.5f;
    }

	// Update is called once per frame
	void Update ()
	{
        //CalcCameraPositionsOffset();

        var camForwardZoom = transform.rotation * Vector3.forward; // rotate vector forward 45 degrees around Y
		float zoom = 0;
		float mouseScroll = Input.GetAxis ("Mouse ScrollWheel");
		if (mouseScroll > 0) {
			zoom = step*4;
		} else if (mouseScroll < 0) {
			zoom = -step*4;
		}

		Debug.Log("MouseScroll: " + mouseScroll);
		Debug.Log("Zoom: " + zoom);

		//float zoom = Input.GetAxis ("Mouse ScrollWheel") * smoothing;
		
		Vector3 newCamPosZoom = transform.position + camForwardZoom * zoom;
		if (newCamPosZoom.x > xMinLimit && newCamPosZoom.x < xMaxLimit) {
			transform.position = Vector3.Lerp (transform.position, 
			                                   newCamPosZoom,   
			                                   smoothing * Time.deltaTime);
		} else {
			Debug.Log("Limit xPos: " + newCamPosZoom.x);
		}

        ParallelMoving();

        if (KeyboardControlEnabled)
        {
            if (Input.GetKey(KeyCode.W))
            {
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

            if (Input.GetKey(KeyCode.A))
            {
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

            if (Input.GetKey(KeyCode.D))
            {
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

            if (Input.GetKey(KeyCode.S))
            {
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
}
