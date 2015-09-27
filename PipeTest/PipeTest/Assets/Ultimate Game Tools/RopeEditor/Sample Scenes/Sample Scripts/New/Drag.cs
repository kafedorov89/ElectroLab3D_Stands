using UnityEngine;
using System.Collections;

//скрипт - управляет захватом и переносом (не используется)
public class Drag : MonoBehaviour {
	
	//public bool isDragEnabled; //разрешено перемещение или нет
	public int maxDistance = 100; //не трогаем слишком далекие объекты
	public GameObject[] objects; //объекты, которые можно перетаскивать
	private Ray ray; 
	private RaycastHit hit; 
	
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		foreach (GameObject obj in objects)
		{
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Input.GetMouseButton(1))
			{
				if (Physics.Raycast(ray, out hit, maxDistance) && hit.collider.gameObject == obj)
				{
					Vector3 cameraTransform = Camera.main.transform.InverseTransformPoint(0, 0, 0);
					Vector3 newPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraTransform.z);
					obj.transform.position = Camera.main.ScreenToWorldPoint(newPos);
					//Debug.Log("Pos: " + obj.transform.position);
				}
			}
		}
	}
}
