using UnityEngine;
using System.Collections;
using System.Security.Cryptography;
using System;
using Newtonsoft.Json;

public class WebSocketManager : MonoBehaviour {

    public string IPAdress = "192.168.1.206";
    public string PortNumber = "8888";
    public string ControllerName = "/ws";
    public WebSocket websocketConnection = null;
    public bool is_connected = false;

    public GameObject ConnectedButton;
    public GameObject DisconnectedButton;

    public GameObject loginManagerObj;
    private LoginManager loginManager;
    public GameObject ropesManagerObj;
    private RopesManager ropesManager;
    public GameObject messageObj;
    private MessageManager messageManager;


    // Use this for initialization
    void Start () {
        loginManager = (LoginManager)loginManagerObj.GetComponent<LoginManager>();
        ropesManager = (RopesManager)loginManagerObj.GetComponent<RopesManager>();
        messageManager = (MessageManager)loginManagerObj.GetComponent<MessageManager>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public string CreateJSONPackage(string request_id, string request_type, string Data)
    {
        return JsonConvert.SerializeObject(new string[] { request_id, request_type, Data });
    }


    public void DisconnectFromWebSocket_Button()
    {
        DisconnectFromWebSocket();
    }

    public void DisconnectFromWebSocket()
    {
        try
        {
            websocketConnection.Close(); //Close Websocket connection; //Close Websocket connection
            Change_ConnectButton_State(false);

        }
        catch (NullReferenceException e)
        {
            Debug.Log("Connection was close with error: " + e); //Show error
        }
        catch (WebSocketSharp.WebSocketException e)
        {
            Debug.Log("Connection was close with error: " + e); //Show error
        }
    }

    void OnApplicationQuit()
    {
        DisconnectFromWebSocket();
    }

    public void LogIn(string LoginObject)
    {
        string request_id = Guid.NewGuid().ToString();
        string request_type = "LogIn";

        if (websocketConnection != null)
        {
            try {
                websocketConnection.SendString(CreateJSONPackage(request_id, request_type, LoginObject));
            }
            catch (NullReferenceException e)
            {
                Debug.Log("Connection was close with error: " + e); //Show error
            }
            catch (WebSocketSharp.WebSocketException e)
            {
                Debug.Log("Connection was close with error: " + e); //Show error
            }

        }
        //yield return 0;
    }

    public void LogOut()
    {
        string requestMessage = "JSON";

        websocketConnection.SendString(requestMessage);
        //yield return 0;
    }

    private IEnumerator AnswerParser(string answerMessage)
    {
        print("answerMessage in JSON: " + answerMessage);

        //Config operations
        //  All local Standworks with connections was uploaded and added to database
        //  Was got List with Standwork's connections from database

        //Login operations
        //  Logged in on Server was comleted and Logged out on Server was comleted
        bool login_status = false;
        //JSON parsing of answer Message

        //Send callback to LoginManager
        loginManager.Callback_ServerLogIn(login_status);

    //Standworks operations
    //  Was got Number of current standwork. Ready for work
    //  Current standwork comleted_flag was added to database

    yield return 0;
    }

    public void ConnectToWebSocket_Button()
    {
        StartCoroutine(ConnectToWebSocket());
    }

    public void Change_ConnectButton_State(bool state)
    {
        if (state)
        {
            DisconnectedButton.SetActive(false);
            ConnectedButton.SetActive(true);
        }
        else
        {
            DisconnectedButton.SetActive(true);
            ConnectedButton.SetActive(false);
        }
    }

    public IEnumerator ConnectToWebSocket()
    {
        Debug.Log("Connection to WebSocket"); //Show received message from server
        websocketConnection = new WebSocket(new Uri("ws://" + IPAdress + ":" + PortNumber + ControllerName));
        
        yield return StartCoroutine(websocketConnection.Connect());
        //websocketConnection.SendString("UnityTest");
        
        while (true)
        {
            string answerMessage = websocketConnection.RecvString();
            if (answerMessage != null)
            {
                Debug.Log("answerMessage: " + answerMessage); //Show received message from server


                //yield return StartCoroutine(AnswerParser(answerMessage)); //Debug
                //w.SendString("UnityTest " + i++); //Send something to server avter received message
            }
            if (websocketConnection.Error != null)
            {
                Debug.LogError("Error: " + websocketConnection.Error);
                Change_ConnectButton_State(false);
                break;
            }
            else
            {
                Change_ConnectButton_State(true);
            }
            yield return 0;
        }
    }
}
