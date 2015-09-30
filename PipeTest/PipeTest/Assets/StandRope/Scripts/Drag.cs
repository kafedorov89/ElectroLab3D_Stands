using UnityEngine;
using System.Collections;

//скрипт объекта, переносимого мышью (ПКМ)
public class Drag : MonoBehaviour {
	public int maxDistance = 100; //не трогаем слишком далекие объекты
	public GameObject DragPlane;
	public GameObject OtherPoint;
	//public Vector3 newPlugPosition;
	RaycastHit hitPlane;
	
	public bool isDraggedNow; //взяли нас сейчас или нет
	//public bool isPlug; //взяли нас сейчас или нет
	//(!) не требует первоначальной инициализации в редакторе

	public Vector3 prevPosition; //предыдущая позиция объекта 
	//нужна, чтобы вернуться к ней в случае превышения длины веревки
	//(!) не требует первоначальной инициализации в редакторе 

	private RopeManager ropeManager;
	private Attracted attracted;

	public GameObject Ropes;

	public bool isFix;

	// Use this for initialization
	void Start () 
	{
		isFix = false;
		isDraggedNow = false;
		//isPlug = false;
		ropeManager = transform.root.GetComponent<RopeManager>();
		attracted = gameObject.GetComponent<Attracted> ();
	}
		
	void deleteParentRope(){
		//FindParent
		//Remove parent gameobject
	}

	void dragProcess(){
		if (!isFix) {
			Ray rayToPlug;
			Ray rayToDragPlane; 
			RaycastHit hit; 
			RaycastHit[] hits;
			GameObject plug;
			
			plug = this.gameObject;
			prevPosition = plug.transform.position;
			
			rayToPlug = Camera.main.ScreenPointToRay (Input.mousePosition);
			rayToDragPlane = Camera.main.ScreenPointToRay (Input.mousePosition);
			
			hits = Physics.RaycastAll (rayToPlug, maxDistance);
			
			//If object is catched and this is a plug
			print (Ropes.GetComponent<RopesScript> ().DraggedPlug);
			
			
			//if (Input.GetMouseButtonDown (2) && Ropes.GetComponent<RopesScript>().DraggedPlug == null) {
			if (Input.GetMouseButtonDown (2)) {// && !Ropes.GetComponent<RopesScript>().Dragging) {
				
				
				/*if(Physics.Raycast (rayToPlug, out hit, maxDistance) && hit.transform.gameObject == plug){
				print("Start Drag");
				isDraggedNow = true;
			}*/
				//Ropes.GetComponent<RopesScript> ().Dragging = true;
				for (int i = 0; i < hits.Length; i++) {
					RaycastHit iHit;
					iHit = hits [i];
					if (iHit.transform.gameObject == plug) {
						Ropes.GetComponent<RopesScript> ().Dragging = true;
						Ropes.GetComponent<RopesScript> ().DraggedPlug = plug;
						isDraggedNow = true;
						//Ropes.GetComponent<RopesScript> ().Dragging = true;
						break;
					} else {
						//isDraggedNow = false;
						//Ropes.GetComponent<RopesScript>().DraggedPlug = null;
						//Ropes.GetComponent<RopesScript> ().Dragging = true;
					}
				}
			}
			
			//If mouse is down and plug was catched
			if (Input.GetMouseButton (2) && isDraggedNow && Ropes.GetComponent<RopesScript> ().DraggedPlug == plug.gameObject) {
				print ("Dragging");
				prevPosition = plug.transform.position;
				
				for (int i = 0; i < hits.Length; i++) {
					RaycastHit iHit;
					iHit = hits [i];
					if (iHit.transform.gameObject == DragPlane) {
						hitPlane = iHit;
						break;
					}
					
					if (iHit.transform.gameObject == DragPlane) {
						hitPlane = iHit;
					}
				}
				
				//Check Distance limits
				if (!ropeManager.IsBadDistance (hitPlane.point, OtherPoint.transform.position)) {
					transform.position = hitPlane.point;
					//print (transform.position);
				} else {
					isDraggedNow = false;
					//Ropes.GetComponent<RopesScript> ().Dragging = false;
					//Ropes.GetComponent<RopesScript>().DraggedPlug = null;
					//print ("Bad Distance");
				}
			}
			
			if (Input.GetMouseButtonUp (2)) {
				isDraggedNow = false;
				Ropes.GetComponent<RopesScript> ().Dragging = false;
				Ropes.GetComponent<RopesScript> ().DraggedPlug = null;
			}
		}
	}

	void Update () 
	{
		dragProcess();
	}
}
