using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//скрипт притягиваемого объекта
public class Attracted : MonoBehaviour {

	//объекты, к которым можно прилипнуть 
	//скачиваем у родителя при старте
	private List<GameObject> availableAttractors;
	public GameObject OtherPoint;
    public RopesManager ropesManager;

	private RopeScript ropeScript; //родительский скрипт
	private Drag drag; //перетаскивание мышью

	//текущий объект, к которому прилипли (если есть, если нет - null)
	//закачиваем в родительский скрипт по факту прилипания
	public GameObject currentAttractor;

    public bool isSmallPin;
	
	public float minDistance = 0.2f; //при достижении этого расстояния объект прилипает
    public float curDistance = 0f;
    public float foundDistace;

    public float posXWhenAttract;// = -0.4f; //положение по X при прилипании
    public float plugSize; //размер пина для добавления поверх него еще нескольких и смещения их на уовень выше
	private float prevPosX = 0; //положение по X до прилипания


	// Use this for initialization
	void Start () 
	{
		//ссылка на родительский скрипт
		ropeScript = transform.root.GetComponent<RopeScript>();
		//ссылка на перетаскивание
		drag = gameObject.GetComponent<Drag> ();
		//скачиваем объекты, доступные для прилипания
		availableAttractors = ropeScript.availableSocketList;
		prevPosX = transform.position.x;
	}
	
	// Update is called once per frame
	void Update () 
	{
        if (drag.isDropped)
        {
            drag.isDropped = false;
            //сканируем аттракторы, может куда-нибудь прилипнем
            ScanAttractors();
            //отслеживаем факт отлипания от текущего аттрактора
            CheckReleaseEvent();
        }
	}

	private void ScanAttractors()
	{
        //int i = 0;
        foundDistace = float.MaxValue;
        //float Distace
        GameObject foundAttractor = null;

        foreach (GameObject obj in availableAttractors)
		{
			//пропускаем пустые объекты
			//if (obj == null) continue;

			//вычисляем расстояние до аттрактора в 2D
			Vector3 distance = obj.transform.position - this.transform.position;
			Vector2 dist2d = new Vector2(distance.y, distance.z);// distance.z);
			//float R = distance.magnitude;
            curDistance = dist2d.magnitude;
            //curDistance = distance.magnitude;
            //print(R);

            if (curDistance < foundDistace)
            {
                foundDistace = curDistance;
                foundAttractor = obj;
            }
		}

        //если расстояние меньше заданного и нас НЕ удерживают мыщью
        if (foundDistace < minDistance)
        {
            //проверяем соответсвие типоразмера сокета и пина
            if ((isSmallPin && foundAttractor.GetComponent<SocketScript>().isSmallSocket) || (!isSmallPin && !foundAttractor.GetComponent<SocketScript>().isSmallSocket))
            {
                //смотрим сколько пинов уже подключено к сокету
                Debug.Log("Bad Socket size");
                CatchAttractor(foundAttractor);
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
		//Vector3 pos = transform.position;

		//запоминаем позицию по X
		//prevPosX = transform.position.x;

		//меняем текущее положение
        int pluggedLevel = attr.GetComponent<SocketScript>().pluggedPinList.Count;
		Vector3 attrPos = attr.transform.position;

        Vector3 newPos = new Vector3(posXWhenAttract - plugSize * pluggedLevel, attrPos.y, attrPos.z);

		//проверяем, что с расстоянием будет все нормально
        if (!ropeScript.IsBadDistance (newPos, OtherPoint.transform.position))
		{
			transform.position = newPos;
			//копируем ссылку себе и родителю
			currentAttractor = attr;
			ropeScript.connectedSocketList.Add (currentAttractor);
            
            currentAttractor.GetComponent<SocketScript>().pluggedPinList.Add(this.gameObject);

			drag.isFix = true;
		}
	}
	//отпустить текущий аттрактор
	public void ReleaseAttractor()
	{
        Debug.Log("Release attractor");
        ropeScript.connectedSocketList.Remove(currentAttractor);
        //Перепривязать все привязанные к аттрактору пины
        List<GameObject> tempPinList = new List<GameObject>(currentAttractor.GetComponent<SocketScript>().pluggedPinList);
        currentAttractor.GetComponent<SocketScript>().pluggedPinList.Clear();
		
        currentAttractor = null;
        transform.position = new Vector3(ropesManager.ropeRespownOffset.x, transform.position.y, transform.position.z);

        Debug.Log(tempPinList.Count);
        for (int i = 0; i < tempPinList.Count; i++)
        {
            if (tempPinList[i].gameObject != this.gameObject)
            {
                Debug.Log("Autofree plug");
                tempPinList[i].GetComponent<Drag>().isFix = false;
                tempPinList[i].GetComponent<Drag>().isDropped = true;

                //tempPinList[i].GetComponent<Attracted>().ReleaseAttractor();
                //tempPinList[i].GetComponent<Attracted>().ScanAttractors();
            }
        }
	}
}
		                 