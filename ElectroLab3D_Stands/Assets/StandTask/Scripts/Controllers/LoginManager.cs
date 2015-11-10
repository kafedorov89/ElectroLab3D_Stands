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

    public GameObject LoginButton;
    public GameObject LogoutButton;
    public GameObject LoginIcon;
    public GameObject LogoutIcon;

    //FIXME. Нужно доделать сохрание некорректно прерванной сессии и возможность продолжить работу после восстановления соединения
    public int session_id = -1;

    public bool is_logged = false;
    public WebSocketManager webSocketManager;
    public MessageManager messageManager;

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
        webSocketManager.SendPackageToServer("LogIn", loginClass.GetJSON());
    }

    public void ServerLogOut()
    {
        webSocketManager.SendPackageToServer("LogOut");
    }
}
