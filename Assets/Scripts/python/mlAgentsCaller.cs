using System.Collections;
using System.Collections.Generic;
using IronPython.Hosting; // ADD THIS IN
using UnityEngine;

public class mlAgentsCaller : MonoBehaviour
{
    private void Start()
    {
        var engine = Python.CreateEngine();

        ICollection<string> searchPaths = engine.GetSearchPaths();

        //Path to the folder of learner.py
        searchPaths.Add(Application.dataPath + @"\Plugins\Lib\site-packages\mlagents\trainers\");
        //Path to the Python standard library
        searchPaths.Add(Application.dataPath + @"\Plugins\Lib\");
        engine.SetSearchPaths(searchPaths);

        dynamic py = engine.ExecuteFile(Application.dataPath + @"\Plugins\Lib\site-packages\mlagents\trainers\learn.py");


        // NOT SURE HOW TO PROPERLY CONFIGURE THIS LINE BASED OFF learn.py
        // PythonExample.cs shows how to do it for example greeter.py script
        dynamic learner = py.main("BrainConfig/motionCaptureRnn.yaml --run-id=testName --force --time-scale 1");
    }



    public void callMlagents(string inputs)
    {
        var engine = Python.CreateEngine();

        ICollection<string> searchPaths = engine.GetSearchPaths();

        //Path to the folder of learner.py
        searchPaths.Add(Application.dataPath + @"\Plugins\Lib\site-packages\mlagents\trainers\");
        //Path to the Python standard library
        searchPaths.Add(Application.dataPath + @"\Plugins\Lib\");
        engine.SetSearchPaths(searchPaths);

        dynamic py = engine.ExecuteFile(Application.dataPath + @"\Plugins\Lib\site-packages\mlagents\trainers\learn.py");
        dynamic learner = py.main(inputs);
    }
}
