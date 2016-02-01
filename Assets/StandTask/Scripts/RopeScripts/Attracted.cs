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
    public bool isCoaxialPin;
	
	public float minDistance = 0.2f; //при достижении этого расстояния объект прилипает
    public float curDistance = 0f;
    public float foundDistace;

    public float posXWhenAttract;// = -0.4f; //положение по X при прилипании
    public float plugSize; //размер пина для добавления поверх него еще нескольких и смещения их на уовень выше
	//private float prevPosX = 0; //положение по X до прилипания

    public bool AttractWithOther;

    void Awake()
    {
        Debug.Log("Attracted Awake");
        ropeManager = FindObjectOfType<RopeManager>();
        currentAttractor = null;
    }

	// Use this for initialization
	void Start ()
	{
		Debug.Log ("Attracted Start");
		//
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Debug.Log ("Updated currentAttractor = " + currentAttractor);
	} 

	public void ScanAttractors(bool slave, bool update, GameObject excludeAttractor)
	{
        foundDistace = float.MaxValue;

		GameObject foundAttractor = null;

		GameObject OtherPointAttractor = OtherPoint.GetComponent<Attracted>().currentAttractor;
		if(OtherPointAttractor != null){
			Debug.Log ("OtherPointAttractor = " + OtherPointAttractor.name);
		}else{
			Debug.Log ("slave = " + slave + ", OtherPointAttractor = null");
		}

        foreach (GameObject obj in availableAttractors)
		{
			if(excludeAttractor != obj){
				//вычисляем расстояние до аттрактора в 2D
				Vector3 distance = obj.transform.position - this.transform.position;
				Vector2 dist2d = new Vector2(distance.y, distance.z);// distance.z);
				//float R = distance.magnitude;
	            curDistance = dist2d.magnitude;

	            if (curDistance < foundDistace)
	            {
					foundDistace = curDistance;
	                foundAttractor = obj;
	            }
			}
			else
			{
				Debug.Log ("Founded Attractor used with another point of this rope");
			}
		}

        if (foundAttractor != null)
        {
            //если расстояние меньше заданного и нас НЕ удерживают мышью
            if (foundDistace < minDistance)
            {
                //проверяем соответсвие типоразмера сокета и пина
                if  ((!isCoaxialPin && !foundAttractor.GetComponent<SocketScript>().isCoaxialSocket) && 
                    ((isSmallPin && foundAttractor.GetComponent<SocketScript>().isSmallSocket) || (!isSmallPin && !foundAttractor.GetComponent<SocketScript>().isSmallSocket)) || 
                    (isCoaxialPin && foundAttractor.GetComponent<SocketScript>().isCoaxialSocket))
                {
                    CatchAttractor(foundAttractor, slave, update);
                }
            }
        }
        else
        {
            Debug.Log("Attractor was not found");
        }
    }

	//отслеживаем факт отлипания от текущего аттрактора
	public void CheckReleaseEvent(bool update)
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
				ReleaseAttractor(update);
			}
		}
	}

    //поймать аттрактор
	public void CatchAttractor(GameObject attr, bool slave, bool update)
	{
		Debug.Log ("CatchAttractor");

		//меняем текущее положение
        int pluggedLevel = attr.GetComponent<SocketScript>().pluggedPinList.Count;
		Vector3 attrPos = attr.transform.position;

        //устанавливаем пин в сокет первым или поверх остальных пинов
        Vector3 newPos = new Vector3(posXWhenAttract - plugSize * pluggedLevel, attrPos.y, attrPos.z);

		//проверяем, что с расстоянием будет все нормально
        if (!drag.isFix && AttractWithOther || (!drag.isFix && !ropeClass.IsBadDistance(newPos, OtherPoint.transform.position)))
		{
            //Если выбран режим совместного присоединения - присоединяем текущий штекер только если присоединился второй
			if (!slave && !OtherPoint.GetComponent<Drag>().isFix && AttractWithOther)
			{
				OtherPoint.GetComponent<Drag>().isDropped = true;

				//Запускаем поиск аттрактора для ведомого штекера штекера провода
				OtherPoint.GetComponent<Attracted>().ScanAttractors(true, update, attr.gameObject);
			}

			//Присоединяем текущий штекер если обычный провод или если штекер ведомый
            if ((slave && AttractWithOther) || (AttractWithOther && OtherPoint.GetComponent<Drag>().isFix) || (!AttractWithOther))
            {
                transform.position = newPos;
                
				//Запоминаем текущий аттрактор
                currentAttractor = attr.gameObject;

                ropeClass.connectedSocketList.Add(currentAttractor);

                currentAttractor.GetComponent<SocketScript>().pluggedPinList.Add(this.gameObject);
                drag.isFix = true;
            }
		}

        if(update)
            ropeManager.UpdateUserRopesToDatebase();
	}

	//отпустить текущий аттрактор
	public void ReleaseAttractor(bool update)
	{
        Debug.Log("Release attractor");
        ropeClass.connectedSocketList.Remove(currentAttractor);

        List<GameObject> tempPinList = null;
        
        //Перепривязать все привязанные к аттрактору пины
        if (currentAttractor != null)
        {
            Debug.Log("pluggedPinList.Count =" + currentAttractor.GetComponent<SocketScript>().pluggedPinList.Count);
            tempPinList = new List<GameObject>(currentAttractor.GetComponent<SocketScript>().pluggedPinList);
            currentAttractor.GetComponent<SocketScript>().pluggedPinList.Clear();

            Debug.Log("!!! Attractor was set to null !!!");
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
                        tempPinList[i].GetComponent<Attracted>().ScanAttractors(false, update, null);
                    }
                }
            }
        }

        if (update)
            ropeManager.UpdateUserRopesToDatebase();
	}
}
		                 