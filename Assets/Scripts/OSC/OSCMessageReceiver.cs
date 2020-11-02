// Zach Duer - Institute of Creativity, Arts, and Technology, Virginia Polytechnic Institute and State University
// depends on Bespoke OSC library

// How to use:
// 1. Import this script into your Unity project that will be receiving messages
// 2. Attach this script to an object in your scene
// 3. Receive!


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using Bespoke.Common;
using Bespoke.Common.Osc;

public class OSCMessageReceiver : MonoBehaviour
{
    // Custom error logger
    public messageLog messageLogger;

    //Port to receive data on local server machine - basically arbitrary, but the sender needs to know to send to this port
    private int localPort = 7000;

    private Thread receivingThread; // thread opened to receive data as it comes in
    private UdpClient receivingClient;
    private byte[] bytePacket;
    private IPEndPoint receivedEndPoint;
    private volatile bool dataReceived; // used in update every frame to determine if new data has been received since the last frame

    //Used to resend connect command to remote machine until a reply is received.
    //private bool connected = false;
    //private int frameCounter = 0;

    //private OscMessage messageToSend; // for creating and sending OSC messages.  the OSCMessage type is provided by the Bespoke library
    private IPEndPoint localEndPoint;
    private List<IPEndPoint> clientEndPoints = new List<IPEndPoint>();

    // Use this for initialization
    void Start()
    {
        dataReceived = false;

        receivingThread = new Thread(new ThreadStart(ReceiveData));
        receivingThread.IsBackground = true;
        receivingThread.Start();

        localEndPoint = new IPEndPoint(IPAddress.Any, localPort);
        OscPacket.UdpClient = new UdpClient();  // what does this actually do?
    }

    // Update is called once per frame
    void Update()
    {
        if (dataReceived)
        {
            dataReceived = false;
            FrameParser(OscPacket.FromByteArray(receivedEndPoint, bytePacket));
        }
    }

    // Use this for exiting
    void OnApplicationQuit()
    {
        try
        {
            //SendCommand ("StreamFrames Stop");
            //SendCommand ("Disconnect");
            if (receivingClient.Available == 1)
            {
                receivingClient.Close();
            }
            if (receivingThread != null)
            {
                receivingThread.Abort();
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            messageLogger.messageReceived(e.Message);
        }
    }

    // Receive thread
    private void ReceiveData()
    {
        receivingClient = new UdpClient(localPort);
        receivingClient.Client.ReceiveTimeout = 500;
        while (true)
        {
            try
            {
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                bytePacket = receivingClient.Receive(ref anyIP);
                receivedEndPoint = anyIP;
                dataReceived = true;
            }
            catch (Exception err)
            {
                SocketException sockErr = (SocketException)err; //CAUSES AN ERROR ON QUIT
                if (sockErr.ErrorCode != 10060)
                {
                    Debug.Log("Error receiving packet: " + sockErr.ToString());
                    messageLogger.messageReceived("Error receiving packet: " + sockErr.ToString());
                }
            }
        }
    }

    // Process Data Frame OscBundle
    private void FrameParser(OscPacket packet)
    {
        //LogPacket(packet); //Used for debugging to see all received packets
        if (packet.IsBundle)
        {
            foreach (OscMessage message in ((OscBundle)packet).Messages)
            {
                if (String.Compare(message.Address, "/Program Complete/") == 0)
                {
                    int conversionComplete = (int)message.Data[0];
                    string errorMessage = (string)message.Data[1];
                    if (conversionComplete == 1)
                    {
                        Debug.Log("Program succeeded."); //Replace 
                        //messageLogger.messageReceived("Program suceeded.");
                    }
                    else
                    {
                        Debug.Log("Program failed."); //Replace
                        //messageLogger.messageReceived("Program failed.");
                        Debug.Log(errorMessage);  
                        //messageLogger.messageReceived(errorMessage);
                    }
                }
                else if (String.Compare(message.Address, "/Recording Started/") == 0)
                {
                    Debug.Log("Recording Started.");//Put start recording function
                    //messageLogger.messageReceived("Recording Started.");
                }
            }
        }
        else
        { // if the packet is not a bundle and is just one message
            if (String.Compare(((OscMessage)packet).Address, "/Program Complete/") == 0)
            {
                int conversionComplete = (int)packet.Data[0];
                string errorMessage = (string)packet.Data[1];
                if (conversionComplete == 1)
                {
                    Debug.Log("Program succeeded."); //Replace
                    //messageLogger.messageReceived("Program suceeded.");
                }
                else
                {
                    Debug.Log("Program failed."); //Replace
                    //messageLogger.messageReceived("Program failed.");
                    Debug.Log(errorMessage);
                   // messageLogger.messageReceived(errorMessage);
                }
            }
            else if (String.Compare(((OscMessage)packet).Address, "/Recording Started/") == 0)
            {
                Debug.Log("Recording Started.");//Put start recording function
               // messageLogger.messageReceived("Recording Started.");
            }
        }
    }

    // Log OscMessage or OscBundle
    private static void LogPacket(OscPacket packet)
    {
        if (packet.IsBundle)
        {
            foreach (OscMessage message in ((OscBundle)packet).Messages)
            {
                LogMessage(message);
            }
        }
        else
        {
            LogMessage((OscMessage)packet);
        }
    }

    // Log OscMessage
    private static void LogMessage(OscMessage message)
    {        
        StringBuilder s = new StringBuilder();
        s.Append(message.Address);
        for (int i = 0; i < message.Data.Count; i++)
        {
            s.Append(" ");
            if (message.Data[i] == null)
            {
                s.Append("Nil");
            }
            else
            {
                s.Append(message.Data[i] is byte[] ? BitConverter.ToString((byte[])message.Data[i]) : message.Data[i].ToString());
            }
        }
        Debug.Log(s);
        Debug.Log("test test");
        //messageLogger.messageReceived("Program suceeded.");
    }
}
