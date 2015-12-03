using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class StandtaskJSONClass {

    public int standtask_id { get; set; }
    public string standtask_name { get; set; }
    public string conn_json { get; set; }
    public string rope_json { get; set; }

    public StandtaskJSONClass(int Standtask_id, string Standtask_name, string Conn_json, string Rope_json)
    {
        standtask_id = Standtask_id;
        standtask_name = Standtask_name;
        conn_json = Conn_json;
        rope_json = Rope_json;
    }

    public string GetJSON()
    {
        return JsonConvert.SerializeObject(this);
    }

    public StandtaskJSONClass GetObject(string StandtaskJSON)
    {
        return JsonConvert.DeserializeObject<StandtaskJSONClass>(StandtaskJSON);
    }
}
