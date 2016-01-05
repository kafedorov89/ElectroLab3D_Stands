using UnityEngine;
using System.Collections;

public class RopeJSONClass{

    /*public RopeJSONClass(int ropeType, Vector3 posA, Vector3 posB, Color ropeColor)
    {
        RopeType = ropeType;
        PosA = posA;
        PosB = posB;
        RopeColor = new Vector3(ropeColor.r, ropeColor.g, ropeColor.b);
    }*/

	public RopeJSONClass(int ropeType, Vector3 posA, Vector3 posB, Vector3 ropeColor)
	{
		RopeType = ropeType;
		PosA = posA;
		PosB = posB;
		RopeColor = ropeColor;
	}
	
	public int RopeType  { get; set; }
    public Vector3 PosA { get; set; }
    public Vector3 PosB { get; set; }
    public Vector3 RopeColor { get; set; }
    
}
