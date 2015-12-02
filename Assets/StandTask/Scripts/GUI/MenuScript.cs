using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class MenuScript : MonoBehaviour {

    public GameObject MenuObject;
    public bool MenuEnabled = false;

    public bool navControl;
    public StandtaskNavController nav; //FIXME. Replace StandtaskNavController to the abstract NavManager


    // Use this for initialization
	void Start () {
        HideMenu();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void ShowMenu() {
        if (!MenuEnabled) {
            MenuObject.SetActive(true);
            MenuEnabled = true;
            if (navControl)
                nav.Enable(false);
        }
        else {
            MenuObject.SetActive(false);
            MenuEnabled = false;
            if (navControl) 
                nav.Enable(true);
        }
    }

    public void HideMenu() {
        MenuObject.SetActive(false);
        MenuEnabled = false;
    }
}
