using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RayCastManager : MonoBehaviour {
	
	//public Object CurrentDoor;
    public GameObject LookAtIntersectedObject;
    public string LookAtIntersectedObjectID;
    
    public GameObject MouseIntersectedObject;
    public string MouseIntersectedObjectID;

	//public List<GameObject> doors;
	//public List<GameObject> tools;
	//public List<GameObject> users;

    public Button3DScript MouseButton3DObject;

	// Use this for initialization
	void Start () {
	
	}

	//GameObject HitGameObject;

	// Update is called once per frame

    public void GetLookAtObject()
    {
        LookAtIntersectedObject = null;
        LookAtTrainingObject = null;
        LookAtIntersectedObjectID = "";
        //FsmVariables.GlobalVariables.GetFsmGameObject("IntersectedObject").Value = null;

        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, transform.forward, 4.0F);

        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];

            //Find ObjectClass script inside intersected object
            TrainingObjectClass iObject = hit.transform.GetComponentInChildren<TrainingObjectClass>();

            if (iObject != null)
            {
                LookAtIntersectedObject = hit.transform.gameObject;
                LookAtTrainingObject = LookAtIntersectedObject.GetComponentInChildren<TrainingObjectClass>();
                //FsmVariables.GlobalVariables.GetFsmGameObject("IntersectedObject").Value = IntersectedObject;

            }
        }
    }

    public void GetMouse3DButton()
    {
        MouseIntersectedObject = null;
        MouseButton3DObject = null;

        RaycastHit[] hits;
        Ray MouseRay = this.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

        hits = Physics.RaycastAll(MouseRay, 10.0F);

        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];

            //Find ObjectClass script inside intersected object
            Button3DScript iObject = hit.transform.GetComponentInChildren<Button3DScript>();

            if (iObject != null)
            {
                MouseIntersectedObject = hit.transform.gameObject;
                MouseButton3DObject = MouseIntersectedObject.GetComponentInChildren<Button3DScript>();
                //FsmVariables.GlobalVariables.GetFsmGameObject("IntersectedObject").Value = IntersectedObject;
            }
        }
    }

    public void GetMouseObject()
    {
        MouseIntersectedObject = null;
        MouseTrainingObject = null;
        MouseIntersectedObjectID = "";
        //FsmVariables.GlobalVariables.GetFsmGameObject("IntersectedObject").Value = null;

        RaycastHit[] hits;
        Ray MouseRay = this.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        
        hits = Physics.RaycastAll(MouseRay, 10.0F);

        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];

            //Find ObjectClass script inside intersected object
            TrainingObjectClass iObject = hit.transform.GetComponentInChildren<TrainingObjectClass>();

            if (iObject != null)
            {
                MouseIntersectedObject = hit.transform.gameObject;
                MouseTrainingObject = MouseIntersectedObject.GetComponentInChildren<TrainingObjectClass>();
                //FsmVariables.GlobalVariables.GetFsmGameObject("IntersectedObject").Value = IntersectedObject;

            }
        }
    }
    
    
    void Update () {
        GetLookAtObject();
        GetMouseObject();
        GetMouse3DButton();
        //doors.Clear();
		//tools.Clear();
		//users.Clear();
		
	}
}
