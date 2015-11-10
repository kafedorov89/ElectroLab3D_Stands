using UnityEngine;
using System.Collections;
using System;
using Newtonsoft.Json;

public class LoginClass {

    public string login { get; set; }
    public string password { get; set; }

    public LoginClass(string Login, string Password)
    {
        login = Login;
        password = Password;
    }

    public string GetJSON()
    {
        //Debug.Log("_Entered login = " + login);
        //Debug.Log("_Entered password = " + password);
        return JsonConvert.SerializeObject(this);
    }
}
