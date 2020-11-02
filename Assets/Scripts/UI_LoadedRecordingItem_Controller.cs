using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_LoadedRecordingItem_Controller : MonoBehaviour
{
    public void RemoveRecordingFromLoadedRecordingsList()
    {
        string recordingName = transform.Find("LoadedRecording_Title").GetComponent<Text>().text;
        GameObject.Find("TrainPanel").GetComponent<UIManager_TrainPanel>().RemoveLoadedRecording(recordingName);
    }
}
