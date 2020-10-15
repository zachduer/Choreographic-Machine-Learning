using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TriLib;
using Unity.MLAgents.Policies;

/*
public class FeatureLabels{
    public string recordingName;

    public int[] featureRanges; // the number of possible integers in each of the columns of data
    public Dictionary<string, int> labeledFeatures = new Dictionary<string, int>();
    public int[] unlabeledFeatures;
}
*/

public class RuntimeImporter : MonoBehaviour
{
    //public string csvLabelsFilepath;

    public string fbxFilesDirectory;

    public string jointListFilepath;

    public string behaviorName;

    //public int numJoints;  // 27 for Kinect // 21 for Axis with joints excluded

    GameObject _rootGameObject;
    private string[] jointNames;

    void Start()
    {
        // The commented out line will work when we have a data structure for features working
        //FeatureLabels[] allLabels = ImportCSVLabels(csvLabelsFilepath);

        jointNames = ImportJointList(jointListFilepath);

        // The commented out line will work when we have a data structure for features working
        //ImportFBXDirectory(fbxFilesDirectory, allLabels, jointNames);
        //ImportFBXDirectory(fbxFilesDirectory, jointNames);
    }

    public void buttonPress()
    {
        ImportFBXDirectory(fbxFilesDirectory, jointNames);
    }

    string[] ImportJointList(string filepath)
    {
        using (StreamReader r = new StreamReader(filepath))
        {
            List<string> jointNames = new List<string>();
            while (!r.EndOfStream)
            {
                var line = r.ReadLine();
                jointNames.Add(line);
                //var values = line.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None); // split on new line
            }
            return jointNames.ToArray();
        }
    }

    // The commented out line will work when we have a data structure for features working
    //void ImportFBXDirectory(string directory, FeatureLabels[] allLabels, string[] jointNames)
    void ImportFBXDirectory(string directory, string[] jointNames)
    {
        Debug.Log("importing fbx files");
        DirectoryInfo d = new DirectoryInfo(directory);
        FileInfo[] fbxFiles = d.GetFiles("*.fbx");

        for (int i = 0; i < fbxFiles.Length; i++)
        {
            // The commented out line will work when we have a data structure for features working
            //ImportFile(directory, fbxFiles[i].Name, i, allLabels[i], jointNames);
            ImportFile(directory, fbxFiles[i].Name, i, jointNames);
        }
    }
    
    // The commented out line will work when we have a data structure for features working
    //void ImportFile(string directory, string fileName, int fileIndex, FeatureLabels featureLabels, string[] jointNames)
    void ImportFile(string directory, string fileName, int fileIndex, string[] jointNames)
    {
        string filePath = directory + "/" + fileName;
        var assetLoader = new AssetLoader();
        var assetLoaderOptions = AssetLoaderOptions.CreateInstance();
        assetLoaderOptions.AutoPlayAnimations = true;
        assetLoaderOptions.AnimationWrapMode = WrapMode.Loop;
        assetLoaderOptions.UseOriginalPositionRotationAndScale = true;

        //Debug.Log("filename: " + fileName);
        string recordingName = fileName.Replace(".fbx", "");
        //Debug.Log("recordingName: " + recordingName);

        _rootGameObject = assetLoader.LoadFromFileWithTextures(filePath, assetLoaderOptions);
        _rootGameObject.name = recordingName;
        _rootGameObject.transform.localScale = new Vector3(0.005f, 0.005f, 0.005f);
        _rootGameObject.transform.parent = transform;


        _rootGameObject.AddComponent<JointVisualizer>();

        _rootGameObject.AddComponent<BehaviorParameters>();
        // all the behavior parameters has to be here and not in the Agent Init in order for it to work for some reason. i haven't been able to figure out why that might be, but who cares?
        _rootGameObject.GetComponent<BehaviorParameters>().BehaviorName = "MotionCaptureRNN";
        _rootGameObject.GetComponent<BehaviorParameters>().BrainParameters.VectorObservationSize = jointNames.Length * 4 + 3; 
        _rootGameObject.GetComponent<BehaviorParameters>().BrainParameters.VectorActionSpaceType = SpaceType.Discrete;

        // The commented out line will work when we have a data structure for features working
        //  _rootGameObject.GetComponent<BehaviorParameters>().BrainParameters.VectorActionSize = new int[featureLabels.labeledFeatures.Count];
        _rootGameObject.GetComponent<BehaviorParameters>().BrainParameters.VectorActionSize = new int[2]; // this will need to be changed to match the features list

        for (int i = 0; i < _rootGameObject.GetComponent<BehaviorParameters>().BrainParameters.VectorActionSize.Length; i++)
        {
            _rootGameObject.GetComponent<BehaviorParameters>().BrainParameters.VectorActionSize[i] = 0; // this will need to be changed to match the features list
        }


        //Debug.Log("adding agent component");
        _rootGameObject.AddComponent<MocapTrainerAgent_TimedEpisode_RuntimeVersion>();
        //Debug.Log("calling init on agent component");
        _rootGameObject.GetComponent<MocapTrainerAgent_TimedEpisode_RuntimeVersion>().Init(
            recordingName,
            jointNames,
            behaviorName
            //featureLabels         // The commented out line will work when we have a data structure for features working
            );
               
    }
}
