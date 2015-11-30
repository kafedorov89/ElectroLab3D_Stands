using UnityEngine;
using System.Collections;

public class RopeJSONClass{

    public RopeJSONClass(int ropeType, Vector3 posA, Vector3 posB)
    {
        RopeType = ropeType;
        PosA = posA;
        PosB = posB;
    }
    
    public int RopeType  { get; set; }
    public Vector3 PosA { get; set; }
    public Vector3 PosB { get; set; }
    
}
