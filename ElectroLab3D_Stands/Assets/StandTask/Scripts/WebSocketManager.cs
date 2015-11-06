using UnityEngine;
using System.Collections;
using System.Security.Cryptography;
using System.Collections.Generic;
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

    private Dictionary<string, string> RequestPool;


    // Use this for initialization
    void Start () {
        loginManager = (LoginManager)loginManagerObj.GetComponent<LoginManager>();
        ropesManager = (RopesManager)loginManagerObj.GetComponent<RopesManager>();
        messageManager = (MessageManager)loginManagerObj.GetComponent<MessageManager>();
        RequestPool = new Dictionary<string, string>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public string CreateJSONPackage(string request_id, string request_type, string Data)
    {
        return JsonConvert.SerializeObject(new string[] { request_id, request_type, Data });
    }

    //Safe wrapper for websocketConnection.SendString() function
    public void SafeWS_SendString(string StrForSend)
    {
        if (websocketConnection != null)
        {
            try
            {
                websocketConnection.SendString(StrForSend);
            }
            catch (NullReferenceException e)
            {
                Debug.Log("Connection was closed with error: " + e); //Show error
            }
            catch (WebSocketSharp.WebSocketException e)
            {
                Debug.Log("Connection was closed with error: " + e); //Show error
            }
        }
        else
        {
            Debug.Log("Connection is null"); //Show error
        }
    }

    public void LogIn(string LoginObject)
    {
        string request_id = Guid.NewGuid().ToString();
        string request_type = "LogIn";
        RequestPool.Add(request_id, request_type);

        SafeWS_SendString(CreateJSONPackage(request_id, request_type, LoginObject));
         
        //yield return 0;
    }

    public void LogOut()
    {
        string request_id = Guid.NewGuid().ToString();
        string request_type = "LogOut";

        SafeWS_SendString(CreateJSONPackage(request_id, request_type, "{}"));
        //yield return 0;
    }

    private IEnumerator AnswerParser(string answerMessage)
    {
        print("answerMessage in JSON: " + answerMessage);
        var answerObject = JsonConvert.DeserializeObject<AnswerClass>(answerMessage);

        print (answerObject);
        print(answerObject.request_type);
        print(answerObject.bool_value);
        print (answerObject.GetType());

        //Login operations
        //  Logged in on Server was comleted and Logged out on Server was comleted
        //bool login_status = false;
        //JSON parsing of answer Message


        if (answerObject.request_type == "LogIn")
        {
            if (answerObject.bool_value == true)
            {
                loginManager.Callback_ServerLogIn(true);
            }
        }

        if (answerObject.request_type == "LogOut")
        {
            if (answerObject.bool_value == true)
            {
                loginManager.Callback_ServerLogIn(false);
            }
        }

        //Config operations
        //  All local Standworks with connections was uploaded and added to database
        //  Was got List with Standwork's connections from database



        //Send callback to LoginManager
        //loginManager.Callback_ServerLogIn(login_status);

    //Standworks operations
    //  Was got Number of current standwork. Ready for work
    //  Current standwork comleted_flag was added to database

    yield return 0;
    }

    public void Change_ConnectButton_State(bool ConnectionState)
    {
        DisconnectedButton.SetActive(!ConnectionState);
        ConnectedButton.SetActive(ConnectionState);

        loginManager.Callback_ServerLogIn(false);
    }

    public void DisconnectFromWebSocket_Button()
    {
        DisconnectFromWebSocket();
    }

    public void DisconnectFromWebSocket()
    {
        Debug.Log("Disconnection from WebSocket"); //Show received message from server
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

    public void ConnectToWebSocket_Button()
    {
        StartCoroutine(ConnectToWebSocket());
    }

    public IEnumerator ConnectToWebSocket()
    {
        Debug.Log("Connection to WebSocket"); //Show received message from server
        websocketConnection = new WebSocket(new Uri("ws://" + IPAdress + ":" + PortNumber + ControllerName));
        
        yield return StartCoroutine(websocketConnection.Connect());
        Change_ConnectButton_State(true);
        //websocketConnection.SendString("UnityTest");

        while (true)
        {
            string answerMessage = websocketConnection.RecvString();
            if (answerMessage != null)
            {
                //Debug.Log("answerMessage: " + answerMessage); //Show received message from server
                yield return StartCoroutine(AnswerParser(answerMessage)); //Debug
                //w.SendString("UnityTest " + i++); //Send something to server avter received message
            }
            if (websocketConnection.Error != null)
            {
                Debug.LogError("Error: " + websocketConnection.Error);
                Change_ConnectButton_State(false);
                break;
            }
            yield return 0;
        }
    }

    void OnApplicationQuit()
    {
        DisconnectFromWebSocket();
    }
}
