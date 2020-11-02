﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_RecordingItem_Controller : MonoBehaviour
{
    public void LoadRecording()
    {
        string recordingName = transform.Find("Recording_Title").GetComponent<Text>().text;
        GameObject.Find("TrainPanel").GetComponent<UIManager_TrainPanel>().PreviewRecording(recordingName);
    }
}
