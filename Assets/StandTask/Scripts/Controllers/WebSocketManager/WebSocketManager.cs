using UnityEngine;
using System.Collections;
using System.Security.Cryptography;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;

public class WebSocketManager : MonoBehaviour {

    public string IPAdress;
    public string PortNumber;
    public string ControllerName;
    public WebSocket websocketConnection = null;

    public AnswerHandler[] answerHandlerArray;
    private Dictionary<string, string> RequestPool;
    Coroutine WSConnectionCoroutine;
    Coroutine WSAnswerParserCoroutine;

    public string session_id;

    // Use this for initialization
    void Start () {
        RequestPool = new Dictionary<string, string>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    //Safe wrapper for websocketConnection.SendString() function
    public void SafeSendString(string StrForSend)
    {
        if (websocketConnection != null)
        {
            try
            {
                websocketConnection.SendString(StrForSend);
            }
            catch (NullReferenceException e)
            {
                Debug.Log("SafeSendString: Connection error. " + e); //Show error
            }
            catch (WebSocketSharp.WebSocketException e)
            {
                Debug.Log("SafeSendString: Connection error. " + e); //Show error
            }
        }
        else
        {
            Debug.Log("SafeSendString: Connection is null"); //Show error
        }
    }

    public void SendPackageToServer(string request_type, string Data)
    {
        string request_id = Guid.NewGuid().ToString();
        RequestPool.Add(request_id, request_type);

        SafeSendString(JsonConvert.SerializeObject(new string[] { request_id, request_type, Data }));
    }

    public void SendPackageToServer(string request_type)
    {
        string request_id = Guid.NewGuid().ToString();
        RequestPool.Add(request_id, request_type);

        SafeSendString(JsonConvert.SerializeObject(new string[] { request_id, request_type, "" }));
    }

    private IEnumerator AnswerParser(string answerMessage)
    {
        print("answerMessage in JSON: " + answerMessage);
        var answerObject = JsonConvert.DeserializeObject<AnswerClass>(answerMessage);

        //print(answerObject);
        //print(answerObject.request_type);
        //print(answerObject.bool_value);
        //print(answerObject.GetType());

        //Find answer handler for received package in AnswerHandlerArray
        for(int i = 0; i < answerHandlerArray.Length; i++)
        {
            print("i = " + i);
            if (answerHandlerArray[i].RequestTypeName == answerObject.request_type)
            {
                //Send all values from answer to Callback function
                answerHandlerArray[i].SendAnswerObject(answerObject);
                //Execute Callback function
                answerHandlerArray[i].ExecuteCallback();
            }
        }

    yield return 0;
    }

    public void DisconnectFromWebSocket()
    {
        Debug.Log("Disconnection from WebSocket"); //Show received message from server
        try
        {
            websocketConnection.Close(); //Close Websocket connection; //Close Websocket connection
            StopAllCoroutines();
            //StopCoroutine(WSConnectionCoroutine);
        }
        catch (NullReferenceException e)
        {
            Debug.Log("Connection was close with error: " + e); //Show error
        }
        catch (WebSocketSharp.WebSocketException e)
        {
            Debug.Log("Connection was close with error: " + e); //Show error
        }

        for (int i = 0; i < answerHandlerArray.Length; i++)
        {
            //print("i = " + i);
            if (answerHandlerArray[i].RequestTypeName == "CheckConnection")
            {
                answerHandlerArray[i].CallbackBoolValue = false;
                answerHandlerArray[i].ExecuteCallback();
            }
        }
    }

    public void ConnectToWebSocket(string controllerName)
    {
        //websocketConnection = null;
        StartCoroutine(ConnectToWebSocket_InThread(controllerName));

        /*for (int i = 0; i < answerHandlerArray.Length; i++)
        {
            print("i = " + i);
            if (answerHandlerArray[i].RequestTypeName == "CheckConnection")
            {
                answerHandlerArray[i].CallbackBoolValue = true;
                answerHandlerArray[i].ExecuteCallback();
            }
        }*/
        //return 0;
        //ConnectToWebSocket_InThread();
    }

    public IEnumerator ConnectToWebSocket_InThread(string controllerName)
    {
        Debug.Log("Connection to WebSocket"); //Show received message from server
        //websocketConnection = new WebSocket(new Uri("ws://" + IPAdress + ":" + PortNumber + ControllerName));
        websocketConnection = new WebSocket(new Uri("ws://" + IPAdress + ":" + PortNumber + controllerName));

        yield return StartCoroutine(websocketConnection.Connect());
        SendPackageToServer("CheckConnection");
        //StartCoroutine();

        while (true)
        {
            string answerMessage = websocketConnection.RecvString();
            if (answerMessage != null)
            {
                yield return StartCoroutine(AnswerParser(answerMessage)); //Debug
            }
            if (websocketConnection.Error != null)
            {
                Debug.LogError("Error: " + websocketConnection.Error);
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