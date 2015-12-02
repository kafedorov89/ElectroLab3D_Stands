using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour {

    public GameObject allGUI;
    public GameObject studentGUI;
    public GameObject staffGUI;
    public GameObject adminGUI;

    public void GoToTeacherGUI()
    {
        studentGUI.SetActive(false);
        staffGUI.SetActive(true);
        adminGUI.SetActive(false);
    }

    public void GoToStudentGUI()
    {
        studentGUI.SetActive(true);
        staffGUI.SetActive(false);
        adminGUI.SetActive(false);
    }

    public void GoToAdminGUI()
    {
        studentGUI.SetActive(false);
        staffGUI.SetActive(false);
        adminGUI.SetActive(true);
    }

    public void HideAllGUI()
    {
        studentGUI.SetActive(false);
        staffGUI.SetActive(false);
        adminGUI.SetActive(false);
    }


	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
