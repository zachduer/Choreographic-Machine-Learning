using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.Threading;
using TMPro;

public class kinectCallerScript : MonoBehaviour
{
    // Custom error logger
    public messageLog messageLogger;
    // File input text field
    public TMP_InputField fileInput;

    private Thread kinectThread; // thread opened to call kinect script.
    string applicationDataPath;
    string fileName;    

    private void Start()
    {
        applicationDataPath = Application.dataPath;
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

    public void buttonPress()
    {
        UnityEngine.Debug.Log("kinect script clicked.");
        messageLogger.messageReceived("kinect script clicked.");
        //All three of the following lines need to be called each time my program is run
        kinectThread = new Thread(new ThreadStart(startKinectScript));
        kinectThread.IsBackground = true;
        kinectThread.Start();
    }

    public void setFileName()
    {
        fileName = "test/" + fileInput.text;
    }

    void startKinectScript()
    {
        ProcessStartInfo start = new ProcessStartInfo();
        start.Arguments = fileName; //THIS IS WHAT YOU CHANGE        
        //UnityEngine.Debug.Log("dataPath : " + Application.dataPath);
        string executablePath = applicationDataPath + "/kinectProgram/azureTestProgram.exe"; //Change this for where you store .exe
        start.FileName = executablePath;
        start.WindowStyle = ProcessWindowStyle.Hidden;
        start.CreateNoWindow = true;
        int exitCode;

        using(Process proc = Process.Start(start))
        {
            proc.WaitForExit();
            exitCode = proc.ExitCode;
        }
    }
}
