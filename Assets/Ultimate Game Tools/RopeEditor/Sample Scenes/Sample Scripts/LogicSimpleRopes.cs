using UnityEngine;
using System.Collections;

public class LogicSimpleRopes : MonoBehaviour
{
    void OnGUI()
    {
        LogicGlobal.GlobalGUI();
        GUILayout.Label("Simple persistent rope test (procedural rope and linkedobjects rope)");
        GUILayout.Label("Move the mouse while holding down the LEFT button to move the camera");
		GUILayout.Label("Move the mouse while holding down the RIGHT button to move the Connectors");
		GUILayout.Label("Use the spacebar to shoot balls and aim for the ropes to test the physics");
    }
}
