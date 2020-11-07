using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.Threading;
using TMPro;

public class KinectInterface : MonoBehaviour
{
    // Custom error logger
    //public messageLog messageLogger;
    // File input text field
    //public TMP_InputField fileInput;

    public OSCMessageSender oscMessageSender;

    private Thread kinectThread; // thread opened to call kinect script.
    string recordingFilename;
    string executablePath; //Change this for where you store .exe

    private void Start()
    {
        executablePath = Application.dataPath + "/kinectProgram/azureTestProgram.exe"; //Change this for where you store .exe
    }

    /*
    private void OnMouseDown()
    {
        UnityEngine.Debug.Log("kinect script clicked");
        //All three of the following lines need to be called each time my program is run
        kinectThread = new Thread(new ThreadStart(startKinectScript));
        kinectThread.IsBackground = true;
        kinectThread.Start();
    }
    */

    public void StartRecording(string filename)
    {
        UnityEngine.Debug.Log("starting recording");
        recordingFilename = "\"" + Application.persistentDataPath + "/" + filename + ".fbx\"";
        kinectThread = new Thread(new ThreadStart(StartKinect));
        kinectThread.IsBackground = true;
        kinectThread.Start();
    }

    public void StopRecording()
    {
        UnityEngine.Debug.Log("osc message sender clicked.");
        //messageLogger.messageReceived("osc message sender clicked.");
        oscMessageSender.stopRealtimeRecording(); //Call when user stops recording
    }

    void StartKinect()
    {
        ProcessStartInfo start = new ProcessStartInfo();        
        start.Arguments = recordingFilename; //THIS IS WHAT YOU CHANGE
        UnityEngine.Debug.Log(start.Arguments);      
        start.FileName = executablePath;
        start.WindowStyle = ProcessWindowStyle.Hidden;
        start.CreateNoWindow = true;
        int exitCode;

        using (Process proc = Process.Start(start))
        {
            proc.WaitForExit();
            exitCode = proc.ExitCode;
        }
    }

    public void buttonPress()
    {
        //UnityEngine.Debug.Log("kinect script clicked.");
        //messageLogger.messageReceived("kinect script clicked.");
        //All three of the following lines need to be called each time my program is run

    }

    //public void setFileName()
    //{
    //    fileName = "test/" + fileInput.text;
    //}


}
