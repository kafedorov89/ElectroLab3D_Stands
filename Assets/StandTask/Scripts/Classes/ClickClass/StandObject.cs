using UnityEngine;
using System.Collections;
//using System;

//[System.Serializable]
public class StandObject : MonoBehaviour {

    public string Name;

    //[SerializeField]
    public Vector3 PresetCameraPosition;
    public GameObject cameraObject;
    
    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void GetCamPos()
    {
        PresetCameraPosition = cameraObject.transform.position;
    }
}
