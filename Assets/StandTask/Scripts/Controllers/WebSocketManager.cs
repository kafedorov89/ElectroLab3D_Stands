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

    public AnswerHandlerClass[] answerHandlerArray;
    private Dictionary<string, string> RequestPool;
    Coroutine WSConnectionCoroutine;
    Coroutine WSAnswerParserCoroutine;

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
                answerHandlerArray[i].CallbackBoolValue = answerObject.bool_value;
                answerHandlerArray[i].CallbackIntValue = answerObject.int_value;
                answerHandlerArray[i].CallbackFloatValue = answerObject.float_value;
                answerHandlerArray[i].CallbackStringValue = answerObject.string_value;
                answerHandlerArray[i].CallbackVector3Value = answerObject.vector3_value;
                answerHandlerArray[i].CallbackVector3Value = answerObject.vector2_value;

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

    public void ConnectToWebSocket()
    {
        //websocketConnection = null;
        StartCoroutine(ConnectToWebSocket_InThread());

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

    public IEnumerator ConnectToWebSocket_InThread()
    {
        Debug.Log("Connection to WebSocket"); //Show received message from server
        websocketConnection = new WebSocket(new Uri("ws://" + IPAdress + ":" + PortNumber + ControllerName));

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