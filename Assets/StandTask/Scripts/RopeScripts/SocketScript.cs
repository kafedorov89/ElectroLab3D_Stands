using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SocketScript : MonoBehaviour {

	public GameObject socketMesh;

    public string SocketID;
	public Material normalColor;
	public Material correctColor;
	public Material errorColor;

    public bool isSmallSocket;

    public List<GameObject> pluggedPinList;
	// Use this for initialization
	void Start () {
        pluggedPinList.Clear();
	}

	public void setNormalColor(){
		socketMesh.GetComponent<Renderer>().material = normalColor;
		
	}

	public void setCorrectColor(){
		socketMesh.GetComponent<Renderer>().material = correctColor;
		
	}

	public void setErrorColor(){
		socketMesh.GetComponent<Renderer>().material = errorColor;
		
	}

	// Update is called once per frame
	void Update () {
	
	}
}
