using UnityEngine;
using System.Collections;

public class RoleManager : MonoBehaviour {

    public bool is_student;
    public bool is_staff;// = isStaff;
    public bool is_superuser;// = isSuperuser;
    public GUIManager guiManager;

    public void Callback_UserRole(int userRoleID)
    {
        if (userRoleID == 0) //Student
        {
            is_student = true;
            is_staff = false;
            is_superuser = false;
            guiManager.GoToStudentGUI();
        }
        else if (userRoleID == 1) //Teacher
        {
            is_student = false;
            is_staff = true;
            is_superuser = false;
            guiManager.GoToTeacherGUI();
        }
        else if (userRoleID == 2) //Admin
        {
            is_student = false;
            is_staff = false;
            is_superuser = true;
            guiManager.GoToAdminGUI();
        }
    }

    public void ResetAllRole()
    {
        is_student = false;
        is_staff = false;
        is_superuser = false;
        guiManager.HideAllGUI();
    }

    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
