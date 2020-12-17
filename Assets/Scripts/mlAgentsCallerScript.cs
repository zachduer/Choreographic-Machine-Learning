using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.Threading;

public class mlAgentsCallerScript : MonoBehaviour
{
    string pythonFileName;
    string scriptFileName;
    string brainFileName;
    string settings;

    Thread mlAgentsThread; // thread opened to call kinect script.

    void Start()
    {
        pythonFileName = Application.persistentDataPath;        
        string[] parts = pythonFileName.Split('/');
        pythonFileName = parts[0] + "/" + parts[1] + "/" + parts[2] + "/" + parts[3] + "/";
        pythonFileName += "Local/Programs/Python/Python36/";
        scriptFileName = pythonFileName;
        pythonFileName += "python.exe";
        scriptFileName += "Lib/site-packages/mlagents/trainers/learn.py";

        brainFileName = Application.dataPath + "/BrainConfig/motionCaptureRNN.yaml";
        settings = "--run-id=sometestname --force --time-scale 1";
    }

    public void buttonPress()
    {
        UnityEngine.Debug.Log("mlagents script clicked.");
        mlAgentsThread = new Thread(new ThreadStart(startMlAgentsScript));
        mlAgentsThread.IsBackground = true;
        mlAgentsThread.Start();
    }

    void startMlAgentsScript()
    {
        ProcessStartInfo start = new ProcessStartInfo();
        start.FileName = pythonFileName;
        start.Arguments = string.Format("{0} {1} {2}", scriptFileName, brainFileName, settings); //mlagentsProgram brainYaml runSettings
        start.WindowStyle = ProcessWindowStyle.Hidden;
        start.CreateNoWindow = true;
        int exitCode;

        using (Process proc = Process.Start(start))
        {
            proc.WaitForExit();
            exitCode = proc.ExitCode;
        }
        UnityEngine.Debug.Log(exitCode);
    }
}
