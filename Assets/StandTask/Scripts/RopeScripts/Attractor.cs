using UnityEngine;
using System.Collections;

//скрипт - источник притяжения (не используется)
public class Attractor : MonoBehaviour {

	
	public float pullRadius = 2;
	public float pullForce = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		foreach (Collider collider in Physics.OverlapSphere(transform.position, pullRadius))
		{
			// calculate direction from target to me
			Vector3 forceDirection = transform.position - collider.transform.position;
			
			// apply force on target towards me
			//Rigidbody rb = collider.rigidbody;
			Rigidbody rb = collider.GetComponent<Rigidbody>();
			if (rb != null)
				rb.AddForce(forceDirection.normalized * pullForce * Time.fixedDeltaTime);
		}
		
	}
	public void FixedUpdate()
	{
		/*foreach (Collider collider in Physics.OverlapSphere(transform.position, pullRadius))
		{
			// calculate direction from target to me
			Vector3 forceDirection = transform.position - collider.transform.position;
			
			// apply force on target towards me
			//Rigidbody rb = collider.rigidbody;
			Rigidbody rb = collider.GetComponent<Rigidbody>();
			if (rb != null)
				rb.AddForce(forceDirection.normalized * pullForce * Time.fixedDeltaTime);
		}*/
	}
}
