using UnityEngine;
using System.Collections;

public class ConnectionManager : MonoBehaviour {
    public GameObject ConnectButton;
    public GameObject DisconnectButton;
    
    public WebSocketManager webSocketManager;
    public MessageManager messageManager;
    private LoginManager loginManager;
    private TeacherManager teacherManager;

    public bool is_connected = false;

    // Use this for initialization
    void Start()
    {
        loginManager = GetComponent<LoginManager>();
        teacherManager = FindObjectOfType<TeacherManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void Callback_ServerConnect(bool connectionState)
    {
        if (connectionState)
        {
            Debug.Log("ConnectionManager: Connected");
            messageManager.ShowMessage("Соединение с сервером установлено");
        }
        else
        {
            Debug.Log("ConnectionManager: Disconnected");
			teacherManager.ResetStandtask();
        }

        is_connected = connectionState;

        DisconnectButton.SetActive(!connectionState);
        ConnectButton.SetActive(connectionState);

        loginManager.Callback_ServerLogIn(false);
    }

    public void ServerConnect()
    {
        webSocketManager.ConnectToWebSocket(webSocketManager.ControllerName);
    }

    public void ServerDisconnect()
    {
        webSocketManager.DisconnectFromWebSocket();
        //Callback_ServerConnect(false);
        //webSocketManager.SendPackageToServer("CheckConnection");
    }
}
