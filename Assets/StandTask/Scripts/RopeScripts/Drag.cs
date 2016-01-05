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

    public bool KeepHorizontalOtherPoint;

    private Ray rayToPlug;
    private Ray rayToDragPlane;
    private RaycastHit[] hits;
    private RaycastHit hit;
    public GameObject plug;
    public bool isDropped = false;

    public bool DragOtherPoint;

    public RoleManager roleManager;

    public float doubleStep;
    public bool LeftPoint;
    public bool RightPoint;

    // Use this for initialization
    void Awake()
    {
        roleManager = FindObjectOfType<RoleManager>();
        isDropped = false;
        startDrag = false;
        isFix = false;
        isDraggedNow = false;
    }

    void Start()
    {
        //PointOrderZ = OtherPoint.transform.position.z - transform.position.z;
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

    void BadDistanceHandler(Vector3 prevPinDistVector)
    {
        if (DragOtherPoint && !OtherPoint.GetComponent<Drag>().isFix)
        {
            transform.position = hitPlane.point;
            OtherPoint.transform.position = hitPlane.point + prevPinDistVector;
            if (KeepHorizontalOtherPoint)
            {
                OtherPoint.transform.position = new Vector3(OtherPoint.transform.position.x, transform.position.y, OtherPoint.transform.position.z);
            }

            /*if (newPointOrderZ != PointOrderZ)
            {
                Vector3 newOtherPoint;
                OtherZ = OtherPoint.transform.position.z;
                newOtherPointPosition = new Vector3(OtherPoint.transform.position.x, OtherPoint.transform.position.y, OtherPoint.transform.position.);
                OtherPoint.transform.position
            }*/
        }
        else
        {
            isDraggedNow = false;
        }
    }

	void dragProcess(){
        if (!isFix && !roleManager.is_staff)
        {
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

                Vector3 prevPinDistVector = OtherPoint.transform.position - transform.position;

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
                        BadDistanceHandler(prevPinDistVector);
					}
					//transform.position = Vector3.Lerp(transform.position, hitPlane.point, 100f * Time.deltaTime);
					//print (transform.position);
				} else {
                    BadDistanceHandler(prevPinDistVector);
				}


                if ((LeftPoint && transform.position.z > OtherPoint.transform.position.z) || (RightPoint && transform.position.z < OtherPoint.transform.position.z))
                {
                    //doubleStep = Mathf.Abs(2.0f * Vector3.Magnitude(Vector3.ProjectOnPlane(prevPinDistVector, new Vector3(0.0f, 1.0f, 0.0f))));
                    doubleStep = Mathf.Abs(2.0f * Vector3.Magnitude(prevPinDistVector));

                    if (LeftPoint)
                    {
                        OtherPoint.transform.position = new Vector3(OtherPoint.transform.position.x, OtherPoint.transform.position.y, OtherPoint.transform.position.z + doubleStep);
                    }
                    else if (RightPoint)
                    {
                        OtherPoint.transform.position = new Vector3(OtherPoint.transform.position.x, OtherPoint.transform.position.y, OtherPoint.transform.position.z - doubleStep);
                    }
                }


				startDrag = false;
			}
			
			if (Input.GetMouseButtonUp (0) && isDraggedNow) {
                //print("Rope was dropped");
                isDraggedNow = false;
                GetComponent<Attracted>().ScanAttractors(false);
                
                isDropped = true;
				ropeManager.Dragging = false;
				ropeManager.DraggedPlug = null;
			}
		}
	}


}
