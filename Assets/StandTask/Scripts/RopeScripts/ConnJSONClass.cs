using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConnJSONClass
{
    public ConnJSONClass(string a, string b)
    {
        A = a;
        B = b;
    }
    
    public string A { get; set; }
    public string B { get; set; }
    //public Dict<string,string> SocketPairDict  { get; set; }
    //public string StandtaskID { get; set; }
    //public string SocketB_ID  { get; set; }
}
