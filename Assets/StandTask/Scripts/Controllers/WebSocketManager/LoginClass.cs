using UnityEngine;
using System.Collections;
using System;
using Newtonsoft.Json;

public class LoginClass {

    public string login { get; set; }
    public string password { get; set; }
    //public string session_id { get; set; }

    public LoginClass(string Login, string Password)//, string Session_id)
    {
        login = Login;
        password = Password;
        //session_id = Session_id;
    }

    public string GetJSON()
    {
        //Debug.Log("_Entered login = " + login);
        //Debug.Log("_Entered password = " + password);
        return JsonConvert.SerializeObject(this);
    }
}
