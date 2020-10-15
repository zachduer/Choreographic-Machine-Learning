using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class messageLog : MonoBehaviour
{
    public TextMeshProUGUI log1;
    public TextMeshProUGUI log2;
    public TextMeshProUGUI log3;
    public TextMeshProUGUI log4;
    public TextMeshProUGUI log5;

    string[] messageArray = new string[5];

    public void messageReceived(string message)
    {
        messageArray[4] = messageArray[3];
        messageArray[3] = messageArray[2];
        messageArray[2] = messageArray[1];
        messageArray[1] = messageArray[0];
        messageArray[0] = message;

        log1.text = messageArray[0];
        log2.text = messageArray[1];
        log3.text = messageArray[2];
        log4.text = messageArray[3];
        log5.text = messageArray[4];
    }

    // Start is called before the first frame update
    void Start()
    {
        messageArray[0] = "";
        messageArray[1] = "";
        messageArray[2] = "";
        messageArray[3] = "";
        messageArray[4] = "";
    }
}
