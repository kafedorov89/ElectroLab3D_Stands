using UnityEngine;
using System.Collections;

//скрипт объекта, переносимого мышью (ПКМ)
public class Drag : MonoBehaviour {
	public int maxDistance = 100; //не трогаем слишком далекие объекты
	public float distLimitMax = 10f;
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

	private RopeScript ropeScript;
	private Attracted attracted;
	private bool startDrag = false;

	public GameObject Ropes;

	public bool isFix;

    private Ray rayToPlug;
    private Ray rayToDragPlane;
    private RaycastHit[] hits;
    private RaycastHit hit;
    private GameObject plug;
    public bool isDropped = false;

    // Use this for initialization
    void Start () 
	{
        isDropped = false;
        startDrag = false;
		isFix = false;
		isDraggedNow = false;
        //isPlug = false;
        ropeScript = transform.root.GetComponent<RopeScript>();
		attracted = gameObject.GetComponent<Attracted> ();

        plug = this.gameObject;
	}

	void dragProcess(){
		if (!isFix) {
			//if (Input.GetMouseButtonDown (2) && Ropes.GetComponent<RopesManager>().DraggedPlug == null) {
			if (Input.GetMouseButtonDown (0)) {// && !Ropes.GetComponent<RopesManager>().Dragging) {
                print("Try start drag");
                rayToPlug = Camera.main.ScreenPointToRay(Input.mousePosition);
                hits = Physics.RaycastAll(rayToPlug, maxDistance, 1 << LayerMask.NameToLayer ("Plugs"));
                for (int i = 0; i < hits.Length; i++) {
					RaycastHit iHit;
					iHit = hits [i];
					if (iHit.transform.gameObject == plug) {
						Ropes.GetComponent<RopesManager> ().Dragging = true;
						Ropes.GetComponent<RopesManager> ().DraggedPlug = plug;
						isDraggedNow = true;
						startDrag = true;
						print ("Start drag");
						//Ropes.GetComponent<RopesManager> ().Dragging = true;
					} else {
						//isDraggedNow = false;
						//Ropes.GetComponent<RopesManager>().DraggedPlug = null;
						//Ropes.GetComponent<RopesManager> ().Dragging = true;
					}
				}
			}
			
			//If mouse is down and plug was catched
			if (Input.GetMouseButton (0) && isDraggedNow && Ropes.GetComponent<RopesManager> ().DraggedPlug == plug.gameObject) {
                isDropped = false;
                print ("Dragging");
                prevPosition = plug.transform.position;
                rayToDragPlane = Camera.main.ScreenPointToRay(Input.mousePosition);
                Physics.Raycast(rayToDragPlane, out hit, maxDistance, 1 << LayerMask.NameToLayer("DragPlane"));
                
				
				if (hit.transform.gameObject == DragPlane) {
					hitPlane = hit;
				}
				
				//Check Distance limits
                if (!ropeScript.IsBadDistance (hitPlane.point, OtherPoint.transform.position)) {
					if((Vector3.Distance(prevPosition, hitPlane.point) < distLimitMax ) || startDrag){
						transform.position = hitPlane.point;
					}else{
						isDraggedNow = false;
					}
					//transform.position = Vector3.Lerp(transform.position, hitPlane.point, 100f * Time.deltaTime);
					//print (transform.position);
				} else {
					isDraggedNow = false;
					//Ropes.GetComponent<RopesManager> ().Dragging = false;
					//Ropes.GetComponent<RopesManager>().DraggedPlug = null;
					//print ("Bad Distance");
				}

				startDrag = false;
			}
			
			if (Input.GetMouseButtonUp (0) && isDraggedNow) {
                print("Rope was dropped");
                isDraggedNow = false;
                isDropped = true;
				Ropes.GetComponent<RopesManager> ().Dragging = false;
				Ropes.GetComponent<RopesManager> ().DraggedPlug = null;
			}
		}
	}

	void Update () 
	{
		dragProcess();
	}
}
