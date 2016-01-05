using UnityEngine;
using System.Collections;

public class Select : MonoBehaviour {

	public bool isSelected;
	public int maxDistance = 100; //не трогаем слишком далекие объекты

	public GameObject PlugMesh;

	public Texture FreePlug;
	public Texture SelectedPlug;
	public Texture ErrorPlug;
    public Texture CorrectPlug;

    public RoleManager roleManager;

    // Use this for initialization
    void Awake()
    {
        roleManager = FindObjectOfType<RoleManager>();
    }

	// Use this for initialization
	void Start () {
		isSelected = false;
	}

    public void SelectPin(bool select)
    {
        isSelected = select;
        setSelectedTexture();
    }

	void setSelectedTexture(){
		if(isSelected){
			PlugMesh.GetComponent<Renderer>().material.mainTexture = SelectedPlug;
		}else{
			PlugMesh.GetComponent<Renderer>().material.mainTexture = FreePlug;
		}
	}

    public void setCorrectColor()
    {
        PlugMesh.GetComponent<Renderer>().material.mainTexture = CorrectPlug;
    }

    public void setNormalColor()
    {
        PlugMesh.GetComponent<Renderer>().material.mainTexture = SelectedPlug;
    }

    public void setErrorColor()
    {
        PlugMesh.GetComponent<Renderer>().material.mainTexture = ErrorPlug;
    }

	// Update is called once per frame
	void Update () {
		Ray rayToPlug;
		Ray rayToDragPlane; 
		RaycastHit[] hits;
		GameObject plug;

		plug = this.gameObject;

		rayToPlug = Camera.main.ScreenPointToRay (Input.mousePosition);
		hits = Physics.RaycastAll (rayToPlug, maxDistance);

        if (Input.GetMouseButtonDown(1) && !roleManager.is_staff)
        {
			for (int i = 0; i < hits.Length; i++) {
				RaycastHit iHit;
				iHit = hits [i];
				if (iHit.transform.gameObject == plug) {
					if(!isSelected){
						isSelected = true;
					}else{
						isSelected = false;
					}

                    setSelectedTexture();

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
