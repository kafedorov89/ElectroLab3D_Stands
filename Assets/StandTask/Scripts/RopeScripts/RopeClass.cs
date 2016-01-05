using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//скрипт - менеджер веревки
public class RopeClass : MonoBehaviour {

	//Ограничения
	public float minDistance;// = 0.6f; //минимальное расстояние между концами
    public float maxDistance;// = 2.6f; //максимальное расстояние между концами

	//Текущее расстояние между концами (только для отладки)
	public float currentDistance = 0.0f;
    public int RopeType;
    public Color RopeColor;

	//Клеммы
	public List<GameObject> availableSocketList; //доступные 
	public List<GameObject> connectedSocketList; //подсоединенные

	//Концы веревки
	public GameObject pointA;
	public GameObject pointB;

    public GameObject pointA2;
    public GameObject pointB2;
	public GameObject cable;

	private Drag draggedA;
	private Drag draggedB;
	//private BreakCotroller breakCtrl;

	public GameObject DragPlane;
    public RopeManager ropeManager;
    public float RandomMin;
    public float RandomMax;

    // Use this for initialization
    void Start () 
	{
		//cable.GetComponent<UltimateRope> ().BeforeImportedBonesObjectRespawn();
		//InitAfterAdd ();
	}

    // Update is called once per frame
    void Update()
    {
        //только для отладки - считаем текущее расстояние
        Vector3 distance = pointA.transform.position - pointB.transform.position;
        float R = distance.magnitude;
        currentDistance = R;
    }

    public void DetachAllPins(bool update)
    {
        pointA.GetComponent<Attracted>().ReleaseAttractor(update);
        pointB.GetComponent<Attracted>().ReleaseAttractor(update);
    }

    public void InitAfterAdd(bool setColor, Color ropeColor)
    {
        //Debug.Log("InitAfterAdd");
        connectedSocketList = new List<GameObject> ();
        availableSocketList = ropeManager.SocketList;

        //cable.GetComponent<UltimateRope> ().Regenerate (true);

        // Init Drag script
        //Add link to Drag script
        draggedA = pointA.GetComponent<Drag>();
		draggedB = pointB.GetComponent<Drag>();
		//breakCtrl = cable.GetComponent<BreakCotroller>();

        draggedA.ropeClass = GetComponent<RopeClass>();
        draggedA.attracted = pointA.GetComponent<Attracted>();
        draggedB.ropeClass = GetComponent<RopeClass>();
        draggedB.attracted = pointA.GetComponent<Attracted>();
        draggedA.plug = draggedA.gameObject;
        draggedB.plug = draggedB.gameObject;

		//Add drag plane to A and B points
        draggedA.DragPlane = DragPlane.gameObject;
		draggedB.DragPlane = DragPlane.gameObject;

		//Add (pointB link to pointA) and (pointA link to pointB) 
        draggedA.OtherPoint = pointB.gameObject;
		draggedB.OtherPoint = pointA.gameObject;

		//Add RopeClass object link to PointA and PointB 
        draggedA.ropeManager = ropeManager;
        draggedB.ropeManager = ropeManager;

        // Init Attracted script
        pointA.GetComponent<Attracted>().ropeManager = ropeManager;// = FindObjectOfType<RopeManager>(); //Testing
        //ссылка на родительский скрипт
        pointA.GetComponent<Attracted>().ropeClass = GetComponent<RopeClass>();
        //ссылка на перетаскивание
        pointA.GetComponent<Attracted>().drag = draggedA;
        //скачиваем объекты, доступные для прилипания
        pointA.GetComponent<Attracted>().availableAttractors = availableSocketList;
        
        pointB.GetComponent<Attracted>().ropeManager = ropeManager;// = FindObjectOfType<RopeManager>(); //Testing
        //ссылка на родительский скрипт
        pointB.GetComponent<Attracted>().ropeClass = GetComponent<RopeClass>();
        //ссылка на перетаскивание
        pointB.GetComponent<Attracted>().drag = draggedB;
        //скачиваем объекты, доступные для прилипания
        pointB.GetComponent<Attracted>().availableAttractors = availableSocketList;

		//breakCtrl.Ropes = Ropes.gameObject;
		//breakCtrl.thisRope = this.gameObject;

		//cable.GetComponent<UltimateRope>().DeleteRope();
		//cable.GetComponent<UltimateRope>().Regenerate(false);
		//pointA.GetComponent<AutoRotate>().setOrient();
		//pointB.GetComponent<AutoRotate>().setOrient();

        //Set random position for spline
        if (cable != null)
        {
            if (!setColor)
            {
                RopeColor = new Color(Random.value, Random.value, Random.value);
            }
            else
            {
                RopeColor = ropeColor;
            }

            cable.GetComponent<MeshRenderer>().materials[0].color = RopeColor;
        }

        //Set random color for cable
        pointA2.transform.localPosition = new Vector3(0, Random.Range(RandomMin, RandomMax), 0);
        pointB2.transform.localPosition = new Vector3(Random.Range(-RandomMin, -RandomMax), 0, 0);
        //print(pointA2.transform.localPosition);
        //print(pointB2.transform.localPosition);
        //pointA2.transform.localPosition = new Vector3(0.0f, 1.0f, 0.0f);
        //pointB2.transform.localPosition = new Vector3(-1.0f, 0.0f, 0.0f);

	}

	//определить, все ли хорошо с расстоянием между концами
	public bool IsBadDistance(Vector3 PosPointA, Vector3 PosPointB)
	{
		//считаем расстояние между концами веревки
		Vector3 distance = PosPointA - PosPointB;

        float R = distance.magnitude;
        //Debug.Log("distance = " + R);
		
        //если расстояние вышло за допустимые границы
        bool result = ((R < minDistance) || (R > maxDistance)); 
		//Debug.Log("result = " + result);

        return result;
	}

}