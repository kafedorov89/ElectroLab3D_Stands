using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MessageManager : MonoBehaviour {
    // Use this for initialization

    public Text MessageText;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowMessage(string message)
    {
        MessageText.text = message;
    }
}
