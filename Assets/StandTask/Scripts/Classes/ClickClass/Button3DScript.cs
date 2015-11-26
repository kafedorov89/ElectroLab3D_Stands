using UnityEngine;
using System.Collections;

public class Button3DScript : MonoBehaviour
{
    public RayCastManager rayCastManager;
    public bool Enabled;
    public bool MouseLButtonDown;
    //public ClickClass clickHandler;
    public int buttonN;
    //public ClickClass clickClass;

    // Use this for initialization
    void Start()
    {
        Reset();
        firstPressed = false;
    }

    public void Reset()
    {
        MouseLButtonDown = false;
    }


    public bool firstPressed;
    public float firstPressedTime;
    public bool DoubleClick(int buttonn){
        if (!firstPressed)
        {
            firstPressed = true;
            firstPressedTime = Time.time;
            return false;
        }
        if (firstPressed)
        {
            if (Time.time - firstPressedTime < 1.0f)
            {
                firstPressed = false;
                return true;
            }
            else
            {
                firstPressed = false;
                return false;
            }
        }

        return false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Enabled)
        {
            if (Input.GetMouseButtonDown(buttonN))
            {
                //DoubleClick(buttonN);
                MouseLButtonDown = true;

                if (DoubleClick(buttonN) && rayCastManager.MouseStandObject == GetComponent<StandObject>())
                {
                    Debug.Log(GetComponent<StandObject>().Name);
                    
                    //clickHandler.DoClickAction();
                    GetComponent<ClickClass>().DoClickAction();
                }
            }
            else
            {
                Reset();
            }
        }

    }
}
