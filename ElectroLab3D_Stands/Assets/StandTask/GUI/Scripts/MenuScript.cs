using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class MenuScript : MonoBehaviour {

    public GameObject MenuObject;
    public bool MenuEnabled = false;


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
        }
        else {
            MenuObject.SetActive(false);
            MenuEnabled = false;
        }
    }

    public void HideMenu() {
        MenuObject.SetActive(false);
        MenuEnabled = false;
    }
}
