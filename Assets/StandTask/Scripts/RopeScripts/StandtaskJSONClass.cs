using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class StandtaskJSONClass {

    public int standtask_id { get; set; }
    public string standtask_name { get; set; }
    public string conn_json { get; set; }
    public string rope_json { get; set; }

    public StandtaskJSONClass()
    {
        standtask_id = 0;
        standtask_name = "";
        conn_json = "";
        rope_json = "";
    }

    public StandtaskJSONClass(StandtaskJSONClass standtaskObject)
    {
        standtask_id = standtaskObject.standtask_id;
        standtask_name = standtaskObject.standtask_name;
        conn_json = standtaskObject.conn_json;
        rope_json = standtaskObject.rope_json;
    }
    
    public StandtaskJSONClass(int Standtask_id, string Standtask_name, string Conn_json, string Rope_json)
    {
        standtask_id = Standtask_id;
        standtask_name = Standtask_name;
        conn_json = Conn_json;
        rope_json = Rope_json;
    }

    public StandtaskJSONClass(string JSONstandtask)
    {
        StandtaskJSONClass new_standtask = GetObject(JSONstandtask);
        standtask_id = new_standtask.standtask_id;
        standtask_name = new_standtask.standtask_name;
        conn_json = new_standtask.conn_json;
        rope_json = new_standtask.rope_json;
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
