using UnityEngine;
using System.Collections;

public class SocketScript : MonoBehaviour {

	public GameObject socketMesh;

    public int SocketID = 0;
	public Material normalColor;
	public Material correctColor;
	public Material errorColor;

    public bool isSmallSocket;
	// Use this for initialization
	void Start () {
	
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
