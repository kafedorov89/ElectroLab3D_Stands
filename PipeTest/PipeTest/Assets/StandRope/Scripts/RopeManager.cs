using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//скрипт - менеджер веревки
public class RopeManager : MonoBehaviour {

	//Ограничения
	public float minDistance = 0.6f; //минимальное расстояние между концами
	public float maxDistance = 2.6f; //максимальное расстояние между концами

	//Текущее расстояние между концами (только для отладки)
	public float currentDistance = 0.0f;

	//Клеммы
	public List<GameObject> availableClips; //доступные 
	public List<GameObject> connectedClips; //подсоединенные

	//Концы веревки
	public GameObject pointA;
	public GameObject pointB;
	public GameObject cable;

	private Drag draggedA;
	private Drag draggedB;

	public GameObject DragPlane;
	public GameObject Ropes;

	// Use this for initialization
	void Start () 
	{
		cable.GetComponent<UltimateRope> ().BeforeImportedBonesObjectRespawn();
		//InitAfterAdd ();
	}

	public void InitAfterAdd(){
		connectedClips = new List<GameObject> ();

		//cable.GetComponent<UltimateRope> ().Regenerate (true);
		
		draggedA = pointA.GetComponent<Drag>();
		draggedB = pointB.GetComponent<Drag>();

		draggedA.DragPlane = DragPlane.gameObject;
		draggedB.DragPlane = DragPlane.gameObject;

		draggedA.OtherPoint = pointB.gameObject;
		draggedB.OtherPoint = pointA.gameObject;

		draggedA.Ropes = Ropes.gameObject;
		draggedB.Ropes = Ropes.gameObject;

		//cable.GetComponent<UltimateRope>().DeleteRope();
		//cable.GetComponent<UltimateRope>().Regenerate(false);
		pointA.GetComponent<AutoRotate>().setOrient();
		pointB.GetComponent<AutoRotate>().setOrient();
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