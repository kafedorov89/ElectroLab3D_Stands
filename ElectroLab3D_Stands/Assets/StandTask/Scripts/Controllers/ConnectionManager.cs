using UnityEngine;
using System.Collections;

public class ConnectionManager : MonoBehaviour {
    public GameObject ConnectButton;
    public GameObject DisconnectButton;
    
    public WebSocketManager webSocketManager;
    public MessageManager messageManager;
    public LoginManager loginManager;

    public bool is_connected = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void Callback_ServerConnect(bool connectionState)
    {
        if(connectionState)
            Debug.Log("ConnectionManager: Connected");
        else
            Debug.Log("ConnectionManager: Disconnected");

        is_connected = connectionState;

        DisconnectButton.SetActive(!connectionState);
        ConnectButton.SetActive(connectionState);

        loginManager.Callback_ServerLogIn(false);
    }

    public void ServerConnect()
    {
        webSocketManager.ConnectToWebSocket();
    }

    public void ServerDisconnect()
    {
        webSocketManager.DisconnectFromWebSocket();
        //Callback_ServerConnect(false);
        //webSocketManager.SendPackageToServer("CheckConnection");
    }
}
