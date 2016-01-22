using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GetStandNumberScript : MonoBehaviour {

    public RayCastManager rayCastManager;
    
    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if(rayCastManager.LookAtStandObject != null)
        {
            GetComponent<InputField>().text = rayCastManager.LookAtStandObject.Name;
        }
        else
        {
            GetComponent<InputField>().text = "";
        }
	}
}
