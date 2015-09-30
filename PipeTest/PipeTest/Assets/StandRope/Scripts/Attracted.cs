using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//скрипт притягиваемого объекта
public class Attracted : MonoBehaviour {

	//объекты, к которым можно прилипнуть 
	//скачиваем у родителя при старте
	private GameObject[] availableAttractors;
	public GameObject OtherPoint;

	private RopeManager ropeManager; //родительский скрипт

	private Drag drag; //перетаскивание мышью

	//текущий объект, к которому прилипли (если есть, если нет - null)
	//закачиваем в родительский скрипт по факту прилипания
	private GameObject currentAttractor;
	
	public float minDistance = 0.2f; //при достижении этого расстояния объект прилипает

	public float posXWhenAttract;// = -0.4f; //положение по X при прилипании
	private float prevPosX = 0; //положение по X до прилипания


	// Use this for initialization
	void Start () 
	{
		//ссылка на родительский скрипт
		ropeManager = transform.root.GetComponent<RopeManager>();

		//ссылка на перетаскивание
		drag = gameObject.GetComponent<Drag> ();

		//скачиваем объекты, доступные для прилипания
		availableAttractors = ropeManager.availableClips;

		prevPosX = transform.position.x;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//сканируем аттракторы, может куда-нибудь прилипнем
		ScanAttractors();

		//отслеживаем факт отлипания от текущего аттрактора
		CheckReleaseEvent();
	}

	private void ScanAttractors()
	{
		foreach (GameObject obj in availableAttractors)
		{
			//пропускаем пустые объекты
			if (obj == null) continue;

			//вычисляем расстояние до аттрактора в 2D
			Vector3 distance = obj.transform.position - this.transform.position;
			Vector2 dist2d = new Vector2(distance.y, distance.z);
			//float R = distance.magnitude;
			float R = dist2d.magnitude;
			
			//если расстояние меньше заданного и нас НЕ удерживают мыщью
			if ((R < minDistance) && (drag.isDraggedNow == false))
			{
				//делаем попытку прилипнуть
				//TODO: должен притягиваться к ближайшему объекту, а не к первому попавшемуся
				if (currentAttractor == null)
				{
					CatchAttractor(obj);
					break;
				}
			}
		}
	}
	//отслеживаем факт отлипания от текущего аттрактора
	private void CheckReleaseEvent()
	{
		if (currentAttractor != null)  //если у нас был аттрактор
		{
			//смотрим положение наше и аттрактора в 2D
			Vector3 attrPos3 = currentAttractor.transform.position;
			Vector3 myPos3 = gameObject.transform.position;
			Vector2 attrPos2 = new Vector2(attrPos3.y, attrPos3.z);
			Vector2 myPos2 = new Vector2(myPos3.y, myPos3.z);

			//если мы отошли от него ИЛИ нас взяли
			if ((attrPos2 != myPos2) || (drag.isDraggedNow == true))
			{
				//у нас больше нет аттрактора
				ReleaseAttractor();
			}
		}
	}
	//поймать аттрактор
	public void CatchAttractor(GameObject attr)
	{
		//запоминаем положение
		Vector3 pos = transform.position;

		//запоминаем позицию по X
		//prevPosX = transform.position.x;

		//меняем текущее положение
		Vector3 attrPos = attr.transform.position;

		Vector3 newPos = new Vector3(posXWhenAttract, attrPos.y, attrPos.z);

		//проверяем, что с расстоянием будет все нормально
		if (!ropeManager.IsBadDistance (newPos, OtherPoint.transform.position))
		{
			transform.position = newPos;
		}
		else   
		{
			//копируем ссылку себе и родителю
			currentAttractor = attr;
			ropeManager.connectedClips.Add (currentAttractor);
		}
	}
	//отпустить текущий аттрактор
	public void ReleaseAttractor()
	{
		ropeManager.connectedClips.Remove(currentAttractor);
		currentAttractor = null;
		transform.position = new Vector3(prevPosX, transform.position.y, transform.position.z);
	}
}
		                 