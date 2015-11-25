using UnityEngine;
using System.Collections;

public class Button3DScript : ControlClass {

    //public TwoState3DSwitcherScript switcher3DScript;
    
    public bool MouseLButtonDown;
    //public bool MouseRButtonDown;
    //public bool MouseMButtonDown;

    public bool fixMode;

    public string LButtonState0Name;
    public string LButtonState1Name;

    public ParamClass buttonState;

    
    // Use this for initialization
	void Start () {
        Reset();
	}
	
    public void Reset(){
        MouseLButtonDown = false;
        buttonState.ParamBoolValue = MouseLButtonDown;
        //MouseRButtonDown = false;
        //MouseMButtonDown = false;
    }

	// Update is called once per frame
	void Update () {
        if (rayCastManager.MouseButton3DObject == this)
        {
            if(!fixMode){
                MouseLButtonDown = false;
                buttonState.ParamBoolValue = MouseLButtonDown;
                //MouseRButtonDown = false;
                //MouseMButtonDown = false;
            }

            if (Enabled)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    MouseLButtonDown = !MouseLButtonDown;
                    buttonState.ParamBoolValue = MouseLButtonDown;
                } 
            }

            /*if (Input.GetMouseButtonDown(1))
            {
                MouseRButtonDown = !MouseRButtonDown;
            }

            if (Input.GetMouseButtonDown(2))
            {
                MouseMButtonDown = !MouseMButtonDown;
            }*/
        }

        
	}
}
