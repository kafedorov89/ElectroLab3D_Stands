using UnityEngine;
using System.Collections;
using System;
using Newtonsoft.Json;

public class AnswerClass {
    public string request_id { get; set; }
    public string request_type { get; set; }
    public string string_value { get; set; }
    public bool bool_value { get; set; }
    public int int_value { get; set; }
    public float float_value { get; set; }

    public AnswerClass()
    {
        request_id = "";
        request_type = "";
        string_value = "";
        bool_value = false;
        int_value = 0;
        float_value = 0f;
    }
}
