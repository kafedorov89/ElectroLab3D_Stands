using UnityEngine;
using System.Collections;

public class Select : MonoBehaviour {

	public bool isSelected;
	public int maxDistance = 100; //не трогаем слишком далекие объекты

	public GameObject PlugMesh;

	public Texture FreePlug;
	public Texture SelectedPlug;
	public Texture ErrorPlug;

	// Use this for initialization
	void Start () {
		isSelected = false;
	}

	void setSelectedTexture(){
		if(isSelected){
			PlugMesh.GetComponent<Renderer>().material.mainTexture = SelectedPlug;
		}else{
			PlugMesh.GetComponent<Renderer>().material.mainTexture = FreePlug;
		}
	}

	// Update is called once per frame
	void Update () {
		setSelectedTexture();

		Ray rayToPlug;
		Ray rayToDragPlane; 
		RaycastHit[] hits;
		GameObject plug;

		plug = this.gameObject;

		rayToPlug = Camera.main.ScreenPointToRay (Input.mousePosition);
		hits = Physics.RaycastAll (rayToPlug, maxDistance);

		if (Input.GetMouseButtonDown (1)) {
			for (int i = 0; i < hits.Length; i++) {
				RaycastHit iHit;
				iHit = hits [i];
				if (iHit.transform.gameObject == plug) {
					if(!isSelected){
						isSelected = true;
					}else{
						isSelected = false;
					}

					//Ropes.GetComponent<RopesScript> ().Dragging = true;
					break;
				} else {
					//isDraggedNow = false;
					//Ropes.GetComponent<RopesScript>().DraggedPlug = null;
					//Ropes.GetComponent<RopesScript> ().Dragging = true;
				}
			}
		}
	}
}
