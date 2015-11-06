using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//скрипт - менеджер веревки
public class RopeScript : MonoBehaviour {

	//Ограничения
	public float minDistance = 0.6f; //минимальное расстояние между концами
	public float maxDistance = 2.6f; //максимальное расстояние между концами

	//Текущее расстояние между концами (только для отладки)
	public float currentDistance = 0.0f;

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
	private BreakCotroller breakCtrl; 

	public GameObject DragPlane;
	public GameObject Ropes;
    public float RandomMin;
    public float RandomMax;

    // Use this for initialization
    void Start () 
	{
		//cable.GetComponent<UltimateRope> ().BeforeImportedBonesObjectRespawn();
		//InitAfterAdd ();
	}

	public void InitAfterAdd(){
		connectedSocketList = new List<GameObject> ();
        availableSocketList = Ropes.GetComponent<RopesManager>().SocketList;

        //cable.GetComponent<UltimateRope> ().Regenerate (true);

        //Add link to Drag script
        draggedA = pointA.GetComponent<Drag>();
		draggedB = pointB.GetComponent<Drag>();
		//breakCtrl = cable.GetComponent<BreakCotroller>();

		//Add drag plane to A and B points
        draggedA.DragPlane = DragPlane.gameObject;
		draggedB.DragPlane = DragPlane.gameObject;

		//Add (pointB link to pointA) and (pointA link to pointB) 
        draggedA.OtherPoint = pointB.gameObject;
		draggedB.OtherPoint = pointA.gameObject;

		//Add RopeScript object link to PointA and PointB 
        draggedA.Ropes = Ropes.gameObject;
		draggedB.Ropes = Ropes.gameObject;

		//breakCtrl.Ropes = Ropes.gameObject;
		//breakCtrl.thisRope = this.gameObject;

		//cable.GetComponent<UltimateRope>().DeleteRope();
		//cable.GetComponent<UltimateRope>().Regenerate(false);
		//pointA.GetComponent<AutoRotate>().setOrient();
		//pointB.GetComponent<AutoRotate>().setOrient();

        //Set random position for spline
        cable.GetComponent<MeshRenderer>().materials[0].color = new Color(Random.value, Random.value, Random.value);

        //Set random color for cable
        pointA2.transform.localPosition = new Vector3(0, Random.Range(RandomMin, RandomMax), 0);
        pointB2.transform.localPosition = new Vector3(Random.Range(-RandomMin, -RandomMax), 0, 0);
        //print(pointA2.transform.localPosition);
        //print(pointB2.transform.localPosition);
        //pointA2.transform.localPosition = new Vector3(0.0f, 1.0f, 0.0f);
        //pointB2.transform.localPosition = new Vector3(-1.0f, 0.0f, 0.0f);

	}

	// Update is called once per frame
	void Update ()
	{
		//только для отладки - считаем текущее расстояние
		Vector3 distance = pointA.transform.position - pointB.transform.position;
		float R = distance.magnitude;
		currentDistance = R;
	}
	//определить, все ли хорошо с расстоянием между концами
	public bool IsBadDistance(Vector3 PosPointA, Vector3 PosPointB)
	{
		//считаем расстояние между концами веревки
		Vector3 distance = PosPointA - PosPointB;
		float R = distance.magnitude;
		
		//если расстояние вышло за допустимые границы
		return ((R < minDistance) || (R > maxDistance)); 
	}
}