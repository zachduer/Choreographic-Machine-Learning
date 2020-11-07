using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TriLib;
using Unity.MLAgents.Policies;

public class RuntimeImporter : MonoBehaviour
{
    //public string csvLabelsFilepath;

    public string fbxFilesDirectory;

    public string jointListFilepath;

    public string behaviorName;

    public DataManager dataManager;

    //public int numJoints;  // 27 for Kinect // 21 for Axis with joints excluded

    GameObject _rootGameObject;
    private string[] jointNames;

    void Start()
    {
        //Change fbxFilesDirectory to persistentDataPath
        fbxFilesDirectory = Application.persistentDataPath;

        // The commented out line will work when we have a data structure for features working
        //FeatureLabels[] allLabels = ImportCSVLabels(csvLabelsFilepath);
        jointNames = ImportJointList(jointListFilepath);
        // The commented out line will work when we have a data structure for features working
        //ImportFBXDirectory(fbxFilesDirectory, allLabels, jointNames);
        //ImportFBXDirectory(fbxFilesDirectory, jointNames);
    }

    public void buttonPress()
    {
        //ImportFBXDirectory(fbxFilesDirectory, jointNames);
    }

    public void StartImportAndTraining(List<string> recordingNames, string modelName)
    {
        ImportFBXDirectory(recordingNames, modelName, fbxFilesDirectory, jointNames);
    }

    string[] ImportJointList(string filepath)
    {
        using (StringReader r = new StringReader(Resources.Load<TextAsset>(filepath).text))
        {
            List<string> jointNames = new List<string>();
            while (r.Peek() != -1)
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
    void ImportFBXDirectory(List<string> recordingNames, string modelName, string directory, string[] jointNames)
    {
        Debug.Log("importing fbx files");
        DirectoryInfo d = new DirectoryInfo(directory);
        FileInfo[] fbxFiles = d.GetFiles("*.fbx");

        for (int i = 0; i < fbxFiles.Length; i++)
        {
            // if this fbx file isn't in the list of recordings we want to train, then proceed to the next fbx file
            string fbxFileName = fbxFiles[i].Name.Replace(".fbx", ""); // strip the .fbx out of the filename
            if (!recordingNames.Contains(fbxFileName))
            {
                continue;
            }

            string recordingName = fbxFileName;
            // The commented out line will work when we have a data structure for features working
            ImportFile(directory, fbxFiles[i].Name, i, modelName, dataManager.GetLabelsFromRecording(recordingName), jointNames);
            //ImportFile(directory, fbxFiles[i].Name, i, jointNames);
        }
    }
    
    // The commented out line will work when we have a data structure for features working
    void ImportFile(string directory, string fileName, int fileIndex, string modelName, List<string> recordingLabels, string[] jointNames)
    //void ImportFile(string directory, string fileName, int fileIndex, string[] jointNames)
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
        _rootGameObject.GetComponent<BehaviorParameters>().BrainParameters.VectorObservationSize = jointNames.Length * 4 + 3; // number of joints times 4 quaternion values per joint, plus 3 position values for the hips
        _rootGameObject.GetComponent<BehaviorParameters>().BrainParameters.VectorActionSpaceType = SpaceType.Discrete;

        // The commented out line will work when we have a data structure for features working
        List<string> modelLabels = dataManager.GetLabelsFromModel(modelName);
        int numLabelsInModel = modelLabels.Count;
        _rootGameObject.GetComponent<BehaviorParameters>().BrainParameters.VectorActionSize = new int[numLabelsInModel];
        //_rootGameObject.GetComponent<BehaviorParameters>().BrainParameters.VectorActionSize = new int[2]; // this will need to be changed to match the features list

        // For now, we're using a vectoractionsize value of 2 for each vector action.
        // Because we're using binary labels that are either true or false.  Ie.It either is labeled "twisting" or it's not.
        for (int i = 0; i < _rootGameObject.GetComponent<BehaviorParameters>().BrainParameters.VectorActionSize.Length; i++)
        {
            _rootGameObject.GetComponent<BehaviorParameters>().BrainParameters.VectorActionSize[i] = 2;
        }

        int[] recordingVectorActionValues = new int[numLabelsInModel];
        for (int i = 0; i < recordingVectorActionValues.Length; i++)
        {
            // We need to make the list of labels for this recording conform to array of all the possible labels for the model
            // where the value at each index corresponds to whether that label is present for this model.
            // For example, if the possible labels are:  Arc, Twist, Bend
            // and this recording has the labels: Arc, Bend
            // then we need an array like {1, 0, 1}
            if (recordingLabels.Contains(modelLabels[i])) // it should be safe to use i here because the size of the recordingVectorActionValues arary should be the same length as the modelFeatures array, because it was set above
            {
                recordingVectorActionValues[i] = 1;
                //_rootGameObject.GetComponent<BehaviorParameters>().BrainParameters.VectorActionSize[i] = 1;
            }
            else
            {
                recordingVectorActionValues[i] = 0;
                // _rootGameObject.GetComponent<BehaviorParameters>().BrainParameters.VectorActionSize[i] = 0;
            }
        }



        //Debug.Log("adding agent component");
        _rootGameObject.AddComponent<MocapTrainerAgent_TimedEpisode_RuntimeVersion>();
        //Debug.Log("calling init on agent component");
        _rootGameObject.GetComponent<MocapTrainerAgent_TimedEpisode_RuntimeVersion>().Init(
            recordingName,
            jointNames,
            behaviorName,
            recordingVectorActionValues
            );
               
    }
}
