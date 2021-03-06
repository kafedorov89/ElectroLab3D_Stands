﻿using UnityEngine;
using System.Collections;
using System;

public class ClickStandPanelHandler : ClickClass
{

    //public TrainingManager trainingManager;
    //public ActionListManager actionListManager;
    public bool Enabled;
    private float StartTime;
    private float CurTime;
    private float DeltaTime;
    public float MovingTime;
    public float speed;

    // Use this for initialization
    void Start()
    {
        Enabled = false;
    }

    // Update is called once per frame
    // с Update не работает
    void FixedUpdate()
    {
        if (Enabled)
        {
            //CurTime = Time.time;
            //DeltaTime = CurTime - StartTime;

            //GetComponent<StandObject>().cameraObject.transform.position = Vector3.Lerp(GetComponent<StandObject>().cameraObject.transform.position, GetComponent<StandObject>().PresetCameraPosition, (float)Math.Round(DeltaTime / MovingTime, 2));
            GetComponent<StandObject>().cameraObject.transform.position = Vector3.Lerp(GetComponent<StandObject>().cameraObject.transform.position, GetComponent<StandObject>().PresetCameraPosition, Time.deltaTime * speed);

            /*if (Vector3.Distance(GetComponent<StandObject>().cameraObject.transform.position, GetComponent<StandObject>().PresetCameraPosition) == 0) { }
            {
                Debug.Log("Moving was stop");
                Enabled = false;
            }*/

            Debug.Log("Time.deltaTime * speed = " + Time.deltaTime * speed);

            double curDist = Vector3.Distance(GetComponent<StandObject>().cameraObject.transform.position, GetComponent<StandObject>().PresetCameraPosition);

            Debug.Log("curDist = " + curDist);
            //Debug.Log(GetComponent<StandObject>().Name + " " + curDist + " " + DeltaTime + " " + MovingTime);

            if (Math.Round(curDist, 2) == 0.0f)// || Mathf.Approximately(curDist, 0.0f))
            {
                Debug.Log("Moving was stop");
                Enabled = false;
            }
        }

        /*if (Input.GetMouseButtonDown(2))
        {
            Enabled = false;
        }*/
    }

    public override void DoClickAction()
    {
        Debug.Log("DoClickAction");

        StartTime = Time.time;
        Enabled = true;

        //Going to preset camera position
        //StartCoroutine(LerpMovingToPoint());  
    }
}
