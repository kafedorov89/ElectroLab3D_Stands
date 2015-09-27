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
	public GameObject[] availableClips; //доступные 
	public List<GameObject> connectedClips; //подсоединенные

	//Концы веревки
	private GameObject pointA;
	private GameObject pointB;

	private Dragged draggedA;
	private Dragged draggedB;

	// Use this for initialization
	void Start () 
	{
		connectedClips = new List<GameObject> ();

		//получаем ссылку на объекты концов веревки
		pointA = (transform.Find ("PointA")).gameObject;
		pointB = (transform.Find ("PointB")).gameObject;

		draggedA = pointA.GetComponent<Dragged>();
		draggedB = pointB.GetComponent<Dragged>();
	}

	// Update is called once per frame
	void Update ()
	{
		//только для отладки - считаем текущее расстояние
		Vector3 distance = pointA.transform.position - pointB.transform.position;
		float R = distance.magnitude;
		currentDistance = R;

		//анти-торсион (???)
		/*if (draggedA.isDraggedNow || draggedB.isDraggedNow)
		{
			//вычисляем угол вектора AB с осью Z
			Vector3 ab3 = pointB.transform.position - pointA.transform.position;
			Vector3 ab2 = new Vector3(0, ab3.y, ab3.z);
			Vector3 OY = new Vector3(0, 1, 0);
			float scalar = Vector2.Dot(ab2, OY);
			float arc_rad = Mathf.Acos(scalar);
			float arc_grad = arc_rad / Mathf.PI * 180.0f;

			//Quaternion rot = Quaternion.FromToRotation(ab2, OY);
			if (draggedA.isDraggedNow) pointA.transform.rotation = rot;
			if (draggedB.isDraggedNow) pointB.transform.rotation = rot;
		}*/
	}
	//определить, все ли хорошо с расстоянием между концами
	public bool IsBadDistance()
	{
		//считаем расстояние между концами веревки
		Vector3 distance = pointA.transform.position - pointB.transform.position;
		float R = distance.magnitude;
		//Debug.Log(R); //вывод длины
		
		//если расстояние вышло за допустимые границы
		return ((R < minDistance) || (R > maxDistance)); 
	}
}