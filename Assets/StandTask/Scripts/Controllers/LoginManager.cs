﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Newtonsoft.Json;

public class LoginManager : MonoBehaviour {

    public string login;
    public string password;

    public MenuScript LoginMenu;

    public GameObject LoginField;
    public GameObject PasswordField;

    public GameObject LoginButton;
    public GameObject LogoutButton;
    public GameObject LoginIcon;
    public GameObject LogoutIcon;

    //FIXME. Нужно доделать сохрание некорректно прерванной сессии и возможность продолжить работу после восстановления соединения
    public string session_id;

    public bool is_logged;
    //public bool is_staff;
    //public bool is_superuser; 
    public WebSocketManager webSocketManager;
    public MessageManager messageManager;
    public RoleManager roleManager;
    public StudentManager studentManager;
    //public StudentManager studentManager;
    //public RopeManager ropeManager;


    void Awake()
    {
        webSocketManager = FindObjectOfType<WebSocketManager>();
        messageManager = FindObjectOfType<MessageManager>();
        roleManager = FindObjectOfType<RoleManager>();
        studentManager = FindObjectOfType<StudentManager>();
    }


    // Use this for initialization
    void Start()
    {
        is_logged = false;
        //is_staff = false;
        //is_superuser = false;

        //session_id = -1;
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void Callback_ServerLogIn(bool loginState)
    {
        if (loginState)
        {
            Debug.Log("LoginManager: Correct login and password");
            messageManager.MessageText.text = "Вход в учетную запись пользователя " + login + " выполнен";
            //Hide login and password fields automaticly
            LoginMenu.HideMenu();
        }
        else
        {
            Debug.Log("LoginManager: LogOut or Disconnect from server");
            roleManager.ResetAllRole();
            studentManager.ResetStandtask();
            LoginMenu.HideMenu();
            //messageManager.MessageText.text = "Выход из учетной записи пользователя выполнен";
        }
        
        is_logged = loginState;

        LoginButton.SetActive(!loginState);
        LogoutButton.SetActive(loginState);
        LoginIcon.SetActive(!loginState);
        LogoutIcon.SetActive(loginState);        
    }

    public void ServerLogIn()
    {
        InputField login_field = LoginField.GetComponent<InputField>();
        //Debug.Log("Entered login = " + login);
        InputField password_field = PasswordField.GetComponent<InputField>();
        //Debug.Log("Entered password = " + password);
        login = login_field.text;
        password = password_field.text;

        LoginClass loginClass = new LoginClass(login, password);
        //var serializesLoginClass = JsonConvert.SerializeObject(loginClass);
        //print(serializesLoginClass);
        webSocketManager.SendPackageToServer("LogIn", loginClass.GetJSON());
    }

    public void ServerLogOut()
    {
        webSocketManager.SendPackageToServer("LogOut");
    }
}
