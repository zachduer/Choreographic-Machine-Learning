using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_RecordingItem_ClassifyPanel_Controller : MonoBehaviour
{
    public void LoadRecording()
    {
        string recordingName = transform.Find("Recording_Title").GetComponent<Text>().text;
        GameObject.Find("ClassifyPanel").GetComponent<UIManager_ClassifyPanel>().SelectRecording(recordingName);
    }
}
