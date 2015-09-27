using UnityEngine;
using System.Collections;

//скрипт объекта, переносимого мышью (ПКМ)
public class Dragged : MonoBehaviour {
	

	public int maxDistance = 100; //не трогаем слишком далекие объекты

	private Ray ray; 
	private RaycastHit hit; 
	private GameObject obj;

	public bool isDraggedNow; //взяли нас сейчас или нет
	//(!) не требует первоначальной инициализации в редакторе

	public Vector3 prevPosition; //предыдущая позиция объекта 
	//нужна, чтобы вернуться к ней в случае превышения длины веревки
	//(!) не требует первоначальной инициализации в редакторе 

	private RopeManager ropeManager;

	private Attracted attracted;

	// Use this for initialization
	void Start () 
	{
		obj = this.gameObject;
		prevPosition = obj.transform.position;
		isDraggedNow = false;
		ropeManager = transform.root.GetComponent<RopeManager>();
		attracted = gameObject.GetComponent<Attracted> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		//отслеживаем событие поднятия мышью
		if ((Input.GetMouseButton(1)) &&  //нажата ПКМ
		    (Physics.Raycast(ray, out hit, maxDistance)) && //луч поразил объект
		     (hit.collider.gameObject == obj)) //это наш объект
		{
				//запоминаем, что нас подняли
				isDraggedNow = true;
		}

		//если нас взяли - надо следовать за мышью
		if (isDraggedNow)
		{
			//сразу же отлипаем от аттрактора !
			//attracted.ReleaseAttractor();

			//сохраняем текущую позицию объекта, чтобы можно было к ней вернуться
			prevPosition = obj.transform.position;

			//применяем перемещение
			//Внимание! Координата X зафиксирована, перемещение только в плоскости YZ !
			float x = obj.transform.position.x; //фиксируем X

			Vector3 cameraTransform = Camera.main.transform.InverseTransformPoint(0, 0, 0);
			Vector3 screenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraTransform.z);
			Vector3 worldPoint = Camera.main.ScreenToWorldPoint(screenPoint);

			worldPoint.x = x; //восстанавливаем x
			obj.transform.position = worldPoint;

			//если с расстоянием будет плохо
			if (ropeManager.IsBadDistance())
				transform.position = prevPosition; //возвращаем все обратно

		}

		//отслеживаем факт того, что нас отпустили
		if (isDraggedNow && (Input.GetMouseButton (1) == false))
			isDraggedNow = false;
	}
}
