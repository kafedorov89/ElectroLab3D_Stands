using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//скрипт притягиваемого объекта
public class Attracted : MonoBehaviour {

	//объекты, к которым можно прилипнуть 
	//скачиваем у родителя при старте
	public List<GameObject> availableAttractors;
	public GameObject OtherPoint;
    public RopeManager ropeManager;

	public RopeClass ropeClass; //родительский скрипт
	public Drag drag; //перетаскивание мышью

	//текущий объект, к которому прилипли (если есть, если нет - null)
	//закачиваем в родительский скрипт по факту прилипания
	public GameObject currentAttractor;

    public bool isSmallPin;
	
	public float minDistance = 0.2f; //при достижении этого расстояния объект прилипает
    public float curDistance = 0f;
    public float foundDistace;

    public float posXWhenAttract;// = -0.4f; //положение по X при прилипании
    public float plugSize; //размер пина для добавления поверх него еще нескольких и смещения их на уовень выше
	//private float prevPosX = 0; //положение по X до прилипания

    void Awake()
    {
        ropeManager = FindObjectOfType<RopeManager>();
    }


	// Use this for initialization
	void Start ()
	{
        currentAttractor = null;
	}
	
	// Update is called once per frame
	void Update () 
	{
        
	} 

	public void ScanAttractors()
	{
        ropeManager.UpdateUserRopesToDatebase();
        Debug.Log("ScanAttractors");
        //int i = 0;
        foundDistace = float.MaxValue;
        //float Distace
        GameObject foundAttractor = null;

        foreach (GameObject obj in availableAttractors)
		{
            //Debug.Log("Scan availableAttractors");
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

        //если расстояние меньше заданного и нас НЕ удерживают мышью
        if (foundDistace < minDistance)
        {
            //проверяем соответсвие типоразмера сокета и пина
            if ((isSmallPin && foundAttractor.GetComponent<SocketScript>().isSmallSocket) || (!isSmallPin && !foundAttractor.GetComponent<SocketScript>().isSmallSocket))
            {
                CatchAttractor(foundAttractor);
            }
        }
    }
	//отслеживаем факт отлипания от текущего аттрактора
	public void CheckReleaseEvent()
	{
        Debug.Log("CheckReleaseEvent");
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
        ropeManager.UpdateUserRopesToDatebase();
        
        Debug.Log("CatchAttractor");
        //запоминаем положение
		//Vector3 pos = transform.position;

		//запоминаем позицию по X
		//prevPosX = transform.position.x;

		//меняем текущее положение
        int pluggedLevel = attr.GetComponent<SocketScript>().pluggedPinList.Count;
		Vector3 attrPos = attr.transform.position;

        //устанавливаем пин в сокет первым или поверх остальных пинов
        Vector3 newPos = new Vector3(posXWhenAttract - plugSize * pluggedLevel, attrPos.y, attrPos.z);

		//проверяем, что с расстоянием будет все нормально
        if (!ropeClass.IsBadDistance (newPos, OtherPoint.transform.position))
		{
			transform.position = newPos;
			//копируем ссылку себе и родителю
			currentAttractor = attr;
			ropeClass.connectedSocketList.Add (currentAttractor);
            
            currentAttractor.GetComponent<SocketScript>().pluggedPinList.Add(this.gameObject);

			drag.isFix = true;
		}
	}

	//отпустить текущий аттрактор
	public void ReleaseAttractor()
	{
        ropeManager.UpdateUserRopesToDatebase();
        
        Debug.Log("Release attractor");
        ropeClass.connectedSocketList.Remove(currentAttractor);

        List<GameObject> tempPinList = null;
        
        //Перепривязать все привязанные к аттрактору пины
        if (currentAttractor != null)
        {
            Debug.Log("pluggedPinList.Count =" + currentAttractor.GetComponent<SocketScript>().pluggedPinList.Count);
            tempPinList = new List<GameObject>(currentAttractor.GetComponent<SocketScript>().pluggedPinList);
            currentAttractor.GetComponent<SocketScript>().pluggedPinList.Clear();
            currentAttractor = null;
        }

        //Return pin to free position
        transform.position = new Vector3(ropeManager.ropeRespownOffset.x, transform.position.y, transform.position.z);

        if (tempPinList != null)
        {
            Debug.Log("tempPinList.Count =" + tempPinList.Count);
            if (tempPinList.Count > 0)
            {
                for (int i = 0; i < tempPinList.Count; i++)
                {
                    Debug.Log("Autofree plug i = " + i);
                    if (tempPinList[i].gameObject != this.gameObject)
                    {
                        Debug.Log("Autofree plug");
                        tempPinList[i].GetComponent<Drag>().isFix = false;
                        tempPinList[i].GetComponent<Drag>().isDropped = true;

                        //tempPinList[i].GetComponent<Attracted>().ReleaseAttractor();
                        tempPinList[i].GetComponent<Attracted>().ScanAttractors();
                    }
                }
            }
        }
	}
}
		                 