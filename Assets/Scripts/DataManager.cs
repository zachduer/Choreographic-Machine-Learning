using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Model
{
    public Model(string _modelName, List<string> _features, List<string> _labels)
    {
        modelName = _modelName;
        features = _features;
        labels = _labels;
    }

    public string modelName;

    public List<string> features;
    public List<string> labels;
}

[Serializable]
public class Recording
{
    public Recording(string _recordingName, string _modelName, List<string> _labels)
    {
        recordingName = _recordingName;
        modelName = _modelName;
        labels = _labels;
    }

    public string recordingName;

    public string modelName;
    public List<string> labels;
}

//public class FeatureLabels
//{
//    public string recordingName;

//    public int[] featureRanges; // the number of possible integers in each of the columns of data
//    public Dictionary<string, int> labeledFeatures = new Dictionary<string, int>();
//    public int[] unlabeledFeatures;
//} 


public class DataManager : MonoBehaviour
{
    Dictionary<string, Model> savedModelsByName = new Dictionary<string, Model>();
    Dictionary<string, Recording> savedRecordingsByName = new Dictionary<string, Recording>();

    public void Awake()
    {
        LoadData();
    }

    public void SaveModel(string modelName, List<string> features, List<string> labels)
    {
        Model newModel = new Model(modelName, features, labels);

        savedModelsByName.Add(modelName, newModel);
    }

    public List<string> GetModelNames()
    {
        List<string> modelNames = new List<string>();

        foreach (KeyValuePair<string, Model> savedModel in savedModelsByName)
        {
            modelNames.Add(savedModel.Key);
        }

        return modelNames;
    }

    public List<string> GetLabelsFromModel(string modelName)
    {
        if (savedModelsByName.TryGetValue(modelName, out Model model))
        {
            return model.labels;
        }
        else
        {
            return null;
        }
    }

    public List<string> GetFeaturesFromModel(string modelName)
    {
        if (savedModelsByName.TryGetValue(modelName, out Model model))
        {
            return model.features;
        }
        else
        {
            return null;
        }
    }

    public List<string> GetRecordingNames()
    {
        List<string> recordingNames = new List<string>();

        foreach (KeyValuePair<string, Recording> savedRecording in savedRecordingsByName)
        {
            recordingNames.Add(savedRecording.Key);
        }

        return recordingNames;
    }

    public string GetModelFromRecording(string recordingName)
    {
        if (savedRecordingsByName.TryGetValue(recordingName, out Recording recording))
        {
            return recording.modelName;
        }
        else
        {
            return null;
        }
    }

    public List<string> GetLabelsFromRecording(string recordingName)
    {
        if (savedRecordingsByName.TryGetValue(recordingName, out Recording recording))
        {
            return recording.labels;
        }
        else
        {
            return null;
        }
    }

    public void DeleteModel(string modelName)
    {
        savedModelsByName.Remove(modelName);
    }

    public void SaveRecording(string recordingName, string modelName, List<string> labels)
    {
        Recording newRecording = new Recording(recordingName, modelName, labels);
        Debug.Log(recordingName);
        Debug.Log(newRecording.recordingName);
        Debug.Log(savedRecordingsByName);
        savedRecordingsByName.Add(recordingName, newRecording);
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }

    void SaveData()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath
                     + "/ChoreographicMachineLearning_SaveData.dat");
        SaveData data = new SaveData();
        data.savedModelsByName = savedModelsByName;
        data.savedRecordingsByName = savedRecordingsByName;
        bf.Serialize(file, data);
        file.Close();
        Debug.Log("Data saved to diskette!");
    }

    void LoadData()
    {
        if (File.Exists(Application.persistentDataPath
                   + "/ChoreographicMachineLearning_SaveData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file =
                       File.Open(Application.persistentDataPath
                       + "/ChoreographicMachineLearning_SaveData.dat", FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);
            file.Close();
            if (data.savedModelsByName != null)
            {
                savedModelsByName = data.savedModelsByName;
            }
            else
            {
                savedModelsByName = new Dictionary<string, Model>();
            }
            if (data.savedRecordingsByName != null)
            {
                savedRecordingsByName = data.savedRecordingsByName;
            }
            else
            {
                savedRecordingsByName = new Dictionary<string, Recording>();
            }
            Debug.Log("Diskette data loaded!");
        }
        else
        {
            Debug.Log("There is no save data!");
        }
        savedRecordingsByName.Clear();
        savedModelsByName.Clear();
    }
}


[Serializable]
class SaveData
{
    public int savedInt;
    //public float savedFloat;
    //public bool savedBool;

    public Dictionary<string, Model> savedModelsByName;
    public Dictionary<string, Recording> savedRecordingsByName;
}