// Zach Duer - Institute of Creativity, Arts, and Technology, Virginia Polytechnic Institute and State University
// depends on Bespoke OSC library

// How to use:
// 1. Import this script into your Unity project that will be sending messages.
// 2. Attach this script to an object in your scene
// 3. In the inspector, find this script, and set the following:
// localPort - this is basically arbitrary.  this is the port out of which you will be sending data
// externalIP - this is the IP address of the device to which you will be sending data.  you must find this out by finding the IP on that device
// externalPort - this is the port on which the receiving device will be receiving data.  This is also essentially arbitrary, but the receiving script must be listening on this port, so it must be the same here as it on the receiving script.
// 4. To set the message that should be sent, you can either 
// a) change the examples in the Update method of this class
// b) remove the examples in the Update method of this class and instead use them as an example of how to call the AppendMessage function from another script
// in either situation, i generally recommend leaving the SendBundle call in the Update function of this script, and not calling it externally, so that you're not sending a multitude of packets.
// Generally speaking, this script is designed around sending relatively small amounts of data.  If you're going to be transmitting large arrays of data (65k plus values), you should bypass this example script and spend some more time getting familiar with both OSC (bespoke's implentation in this case) and the System.Net API for sending packets over UDP and TCP/IP 

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using Bespoke.Common;
using Bespoke.Common.Osc;

// uses the Bespoke OSC implementation, so that must be present in your Unity project for this to function correctly

public class OSCMessageSender : MonoBehaviour
{
    // Custom error logger
    public messageLog messageLogger;

    public static int localPort = 10025; // this could be any port number, 10025 is randomly chosen

    public string externalIP = "127.0.0.1"; // the IP of the machine to which you want to send messages
    public int externalPort = 7001;

    private IPEndPoint localEndPoint;
    private IPEndPoint externalEndPoint;

    private List<OscMessage> messagesThisFrame = new List<OscMessage>(); // a list of all the messages you Append during a frame, to be bundled together and sent at the end of the frame

    void Start()
    {
        // initialize EndPoints
        localEndPoint = new IPEndPoint(IPAddress.Loopback, localPort);
        externalEndPoint = new IPEndPoint(IPAddress.Parse(externalIP), externalPort);
    }

    /*
    //Replace this function
    private void OnMouseDown()
    {
        UnityEngine.Debug.Log("osc message sender clicked");
        stopRealtimeRecording(); //Call when user stops recording
    }
    */

    public void buttonPress()
    {
        UnityEngine.Debug.Log("osc message sender clicked.");
        messageLogger.messageReceived("osc message sender clicked.");
        stopRealtimeRecording(); //Call when user stops recording
    }

    public void stopRealtimeRecording()
    {
        List<object> stopRealtimeMessage = new List<object>();
        string testMessageAddress = "/End Recording/";
        AppendMessage(testMessageAddress, stopRealtimeMessage);

        SendBundle();
    }

    public void AppendMessage(string address, List<object> values)
    {
        OscMessage messageToSend = new OscMessage(localEndPoint, address);
        messageToSend.ClearData(); // do i need this?
        foreach (object message in values)
        {
            messageToSend.Append(message);
        }
        messagesThisFrame.Add(messageToSend);
    }

    //  sends the messages stored in messagesThisFrame as a bundle, then clears messagesThisFrame
    private void SendBundle()
    {
        OscBundle frameBundle = new OscBundle(localEndPoint);
        foreach (OscMessage message in messagesThisFrame)
        {
            frameBundle.Append(message);
        }
        //UnityEngine.Debug.Log("sending bundle");
        frameBundle.Send(externalEndPoint);

        messagesThisFrame.Clear();
    }
}
