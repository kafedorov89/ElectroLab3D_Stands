using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Newtonsoft.Json;

public class LoginManager : MonoBehaviour {

    public string login;
    public string password;
    public GameObject LoginField;
    public GameObject PasswordField;

    public GameObject webSocketManagerObj;
    public GameObject messageObj;
    public GameObject LoginButton;
    public GameObject LogoutButton;
    public GameObject LoginIcon;
    public GameObject LogoutIcon;

    public int session_id = -1;
    public bool is_logged = false;
    private WebSocketManager webSocketManager;

    // Use this for initialization
    void Start () {
        webSocketManager = (WebSocketManager)webSocketManagerObj.GetComponent<WebSocketManager>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Callback_ServerLogIn(bool loginState)
    {
        Debug.Log("LoginManager: Correct login and password");
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
        var serializesLoginClass = JsonConvert.SerializeObject(loginClass);
        //print(serializesLoginClass);
        webSocketManager.LogIn(loginClass.GetJSON());
    }

    public void ServerLogOut()
    {
        webSocketManager.LogOut();
    }
}
