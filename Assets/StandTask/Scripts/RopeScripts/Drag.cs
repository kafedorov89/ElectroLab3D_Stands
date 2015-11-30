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

	public RopeClass ropeClass;
	public Attracted attracted;
	private bool startDrag = false;

    public RopeManager ropeManager;

	public bool isFix;

    private Ray rayToPlug;
    private Ray rayToDragPlane;
    private RaycastHit[] hits;
    private RaycastHit hit;
    public GameObject plug;
    public bool isDropped = false;

    // Use this for initialization
    void Awake()
    {
        isDropped = false;
        startDrag = false;
        isFix = false;
        isDraggedNow = false;
    }

    void Start()
    {
        /*isDropped = false;
        startDrag = false;
		isFix = false;
		isDraggedNow = false;

        plug = this.gameObject;*/
    }

    void Update()
    {
        dragProcess();
    }

	void dragProcess(){
		if (!isFix) {
			//if (Input.GetMouseButtonDown (2) && Ropes.GetComponent<RopeManager>().DraggedPlug == null) {
			if (Input.GetMouseButtonDown (0)) {// && !Ropes.GetComponent<RopeManager>().Dragging) {
                //print("Try start drag");
                rayToPlug = Camera.main.ScreenPointToRay(Input.mousePosition);
                hits = Physics.RaycastAll(rayToPlug, maxDistance, 1 << LayerMask.NameToLayer ("Plugs"));
                for (int i = 0; i < hits.Length; i++) {
					RaycastHit iHit;
					iHit = hits [i];
					if (iHit.transform.gameObject == plug) {
						ropeManager.Dragging = true;
						ropeManager.DraggedPlug = plug;
						isDraggedNow = true;
						startDrag = true;
                        GetComponent<Attracted>().CheckReleaseEvent();
						print ("Start drag");
						//ropeManager.Dragging = true;
					} else {
						//isDraggedNow = false;
						//Ropes.GetComponent<RopeManager>().DraggedPlug = null;
						//ropeManager.Dragging = true;
					}
				}
			}
			
			//If mouse is down and plug was catched
			if (Input.GetMouseButton (0) && isDraggedNow && ropeManager.DraggedPlug == plug.gameObject) {
                isDropped = false;
                print ("Dragging");
                prevPosition = plug.transform.position;
                rayToDragPlane = Camera.main.ScreenPointToRay(Input.mousePosition);
                Physics.Raycast(rayToDragPlane, out hit, maxDistance, 1 << LayerMask.NameToLayer("DragPlane"));
                
				
				if (hit.transform.gameObject == DragPlane) {
					hitPlane = hit;
				}
				
				//Check Distance limits
                if (!ropeClass.IsBadDistance (hitPlane.point, OtherPoint.transform.position)) {
					if((Vector3.Distance(prevPosition, hitPlane.point) < distLimitMax ) || startDrag){
						transform.position = hitPlane.point;
					}else{
						isDraggedNow = false;
					}
					//transform.position = Vector3.Lerp(transform.position, hitPlane.point, 100f * Time.deltaTime);
					//print (transform.position);
				} else {
					isDraggedNow = false;
					//ropeManager.Dragging = false;
					//Ropes.GetComponent<RopeManager>().DraggedPlug = null;
					//print ("Bad Distance");
				}

				startDrag = false;
			}
			
			if (Input.GetMouseButtonUp (0) && isDraggedNow) {
                //print("Rope was dropped");
                isDraggedNow = false;
                GetComponent<Attracted>().ScanAttractors();
                
                isDropped = true;
				ropeManager.Dragging = false;
				ropeManager.DraggedPlug = null;
			}
		}
	}


}
