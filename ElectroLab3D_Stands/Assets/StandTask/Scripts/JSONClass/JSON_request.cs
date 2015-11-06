using UnityEngine;
using System.Collections;
using System;
using Newtonsoft.Json;

public class JSON_LogIn_request : MonoBehaviour {

    Guid request_id;
    string request_type = "";
    string login = "";
    string password = "";

    public JSON_LogIn_request(string Login, string Password)
    {
        this.request_id = Guid.NewGuid();
        this.request_type = "LogIn";
        this.login = Login;
        this.password = Password;
    }

    public string EncodeToJSON()
    {
        string json_package = "";
        var serialized = JsonConvert.SerializeObject(this);
        return json_package;
    }
}
