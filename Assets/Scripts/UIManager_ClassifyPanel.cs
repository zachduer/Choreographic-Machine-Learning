using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager_ClassifyPanel : MonoBehaviour
{
    public UIManager uiManager;
    public DataManager dataManager;

    public TMP_Dropdown modelDropdown;

    public GameObject recordingsListGrid;
    public GameObject UI_RecordingItem_Prefab;

    public Text selectedRecordingName;

    public Text nnAssociatedRecordingLabels;

    public GameObject classifyButton;
    //public GameObject applyButtonActive;

    public void RefreshPanel()
    {
        RefreshModelDropdown(dataManager.GetModelNames());
        RefreshRecordingsUIList();
        ClearSelectedRecordingNameText();
    }

    public void RefreshModelDropdown(List<string> options)
    {
        modelDropdown.ClearOptions();
        modelDropdown.AddOptions(options);
    }

    public void ClearSelectedRecordingNameText()
    {
        selectedRecordingName.text = "";
    }

    public void RefreshRecordingsUIList()
    {
        // Delete the existing UI objects
        foreach (Transform child in recordingsListGrid.transform)
        {
            Destroy(child.gameObject);
        }

        // Get the current model name selected in the drop down
        if (modelDropdown.options.Count <= 0)
        {
            return;
        }
        string selectedModelName = modelDropdown.options[modelDropdown.value].text;

        // Get the list of recordings names from the data manager
        List<string> recordingNames = dataManager.GetRecordingNames();

        // If the recording has a model of the same type currently selected in the dropdown,
        // add an RecordingUI object for that recording name
        for (int i = 0; i < recordingNames.Count; i++)
        {
            //Debug.Log("recording name: " + recordingNames[i]);
            //Debug.Log("model name for that recording from data manager: " + dataManager.GetModelFromRecording(recordingNames[i]));
            //Debug.Log("selected model name: " + selectedModelName);
            if (dataManager.GetModelFromRecording(recordingNames[i]) == selectedModelName)
            {
                GameObject newUIRecordingtem = Instantiate(UI_RecordingItem_Prefab);
                newUIRecordingtem.transform.parent = recordingsListGrid.transform;
                Transform recordingTitle = newUIRecordingtem.transform.Find("Recording_Title");
                recordingTitle.GetComponent<Text>().text = recordingNames[i];
            }
        }
    }

    public void SelectRecording(string recordingName)
    {
        selectedRecordingName.text = recordingName;
    }

    public void StartClassification()
    {
        
    }
}
