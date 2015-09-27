using UnityEngine;
using System.Collections;

//скрипт для поворота при старте
public class AutoRotate : MonoBehaviour {

	//углы поворота (Эйлеровы углы)
	public float phi = 0.0f;
	public float theta = 0.0f;
	public float gamma = 0.0f;

	// Use this for initialization
	void Start ()
	{
		transform.Rotate (new Vector3(phi, theta, gamma));
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
