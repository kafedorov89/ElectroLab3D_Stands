using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class StandtaskClass {

    public int standtask_id { get; set; }
    public string standtask_name { get; set; }
    public Dictionary<int, int> Connections { get; set; }

    public string GetJSON()
    {
        //Debug.Log("_Entered login = " + login);
        //Debug.Log("_Entered password = " + password);
        return JsonConvert.SerializeObject(this);
    }
}
