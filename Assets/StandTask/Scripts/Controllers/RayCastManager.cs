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

    public StandObject LookAtStandObject;
    public StandObject MouseStandObject;

    //public Button3DScript MouseButton3DObject;

	// Use this for initialization
	void Start () {
	
	}

	//GameObject HitGameObject;

	// Update is called once per frame

    public void GetLookAtObject()
    {
        LookAtIntersectedObject = null;
        LookAtStandObject = null;
        LookAtIntersectedObjectID = "";
        //FsmVariables.GlobalVariables.GetFsmGameObject("IntersectedObject").Value = null;

        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, transform.forward, 4.0F);

        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];

            //Find ObjectClass script inside intersected object
            StandObject iObject = hit.transform.GetComponentInChildren<StandObject>();

            if (iObject != null)
            {
                LookAtIntersectedObject = hit.transform.gameObject;
                LookAtStandObject = LookAtIntersectedObject.GetComponentInChildren<StandObject>();
                //FsmVariables.GlobalVariables.GetFsmGameObject("IntersectedObject").Value = IntersectedObject;

            }
        }
    }

    public void GetMouseObject()
    {
        MouseIntersectedObject = null;
        MouseStandObject = null;
        MouseIntersectedObjectID = "";
        //FsmVariables.GlobalVariables.GetFsmGameObject("IntersectedObject").Value = null;

        RaycastHit[] hits;
        Ray MouseRay = this.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        
        hits = Physics.RaycastAll(MouseRay, 10.0F);

        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];

            //Find ObjectClass script inside intersected object
            StandObject iObject = hit.transform.GetComponentInChildren<StandObject>();

            if (iObject != null)
            {
                MouseIntersectedObject = hit.transform.gameObject;
                MouseStandObject = MouseIntersectedObject.GetComponentInChildren<StandObject>();
                //FsmVariables.GlobalVariables.GetFsmGameObject("IntersectedObject").Value = IntersectedObject;

            }
        }
    }
    
    
    void Update () {
        GetLookAtObject();
        //GetMouseObject();
        //GetMouseObject();
        //GetMouse3DButton();
        //doors.Clear();
		//tools.Clear();
		//users.Clear();
		
	}
}
