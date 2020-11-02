using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager_TrainPanel : MonoBehaviour
{
    public UIManager uiManager;
    public DataManager dataManager;

    public TMP_Dropdown modelDropdown;

    public GameObject recordingsListGrid;
    public GameObject UI_RecordingItem_Prefab;

    public GameObject loadedRecordingsListGrid;
    public GameObject UI_RecordingItem_LoadedRecordedForTraining_Prefab;
    List<string> loadedRecordingNames = new List<string>();

    public Text previewedRecordingName;
    public Text previewedRecordingLabels;

    public GameObject trainButton;
    public GameObject trainButtonActive;

    public RuntimeImporter runtimeImporter;

    public void RefreshPanel()
    {
        RefreshModelDropdown(dataManager.GetModelNames());
        RefreshRecordingsUIList();
        RefreshLoadedRecordingsUIList();
    }

    public void StartTraining()
    {
        trainButton.SetActive(false);
        trainButtonActive.SetActive(true);

        string selectedModelName = modelDropdown.options[modelDropdown.value].text;
        runtimeImporter.StartImportAndTraining(loadedRecordingNames, selectedModelName);
    }

    public void StopTraining()
    {
        trainButton.SetActive(true);
        trainButtonActive.SetActive(false);
    }

    public void SelectModel(TMP_Dropdown dropdown)
    {
        RefreshRecordingsUIList();

        // if we don't have any models yet, and therefore no dropdown option to pick a model, don't try to select a model
        // but make sure to clear the labels list
        if (dropdown.options.Count <= 0)
        {
            //UpdateLabelToggles(new List<string>());
            return;
        }
        string selectedModelName = dropdown.options[dropdown.value].text;
        //Debug.Log("selectedModelName: " + selectedModelName);
        //List<string> labels = dataManager.GetLabelsFromModel(selectedModelName);
        //UpdateLabelToggles(labels);
    }

    public void PreviewRecording(string recordingName)
    {
        previewedRecordingName.text = recordingName;

        List<string> labels = dataManager.GetLabelsFromRecording(recordingName);
        string labelsList = "";
        for (int i = 0; i < labels.Count; i++)
        {
            // if we're not on the last label yet, then add the label and a comma
            // otherwise, if we're on the last label, then add the label but no comma
            if (i < labels.Count - 1)
            {
                labelsList += labels[i] + ", ";
            }
            else
            {
                labelsList += labels[i];
            }
        }
        previewedRecordingLabels.text = labelsList;
    }

    public void LoadPreviewedRecording()
    {
        loadedRecordingNames.Add(previewedRecordingName.text);
        RefreshLoadedRecordingsUIList();
    }

    public void RemoveLoadedRecording(string recordingName)
    {
        loadedRecordingNames.Remove(previewedRecordingName.text);
        RefreshLoadedRecordingsUIList();
    }

    public void RefreshModelDropdown(List<string> options)
    {
        modelDropdown.ClearOptions();
        modelDropdown.AddOptions(options);
    }

    public void RefreshLoadedRecordingsUIList()
    {
        // Delete the existing UI objects
        foreach (Transform child in loadedRecordingsListGrid.transform)
        {
            Destroy(child.gameObject);
        }
        // Add an RecordingUI object for each recording name
        for (int i = 0; i < loadedRecordingNames.Count; i++)
        {
            GameObject newUILoadedRecordingtem = Instantiate(UI_RecordingItem_LoadedRecordedForTraining_Prefab);
            newUILoadedRecordingtem.transform.parent = loadedRecordingsListGrid.transform;
            Transform loadedRecordingTitle = newUILoadedRecordingtem.transform.Find("LoadedRecording_Title");
            loadedRecordingTitle.GetComponent<Text>().text = loadedRecordingNames[i];
        }
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
            if (dataManager.GetModelFromRecording(recordingNames[i]) == selectedModelName)
            {
                GameObject newUIRecordingtem = Instantiate(UI_RecordingItem_Prefab);
                newUIRecordingtem.transform.parent = recordingsListGrid.transform;
                Transform recordingTitle = newUIRecordingtem.transform.Find("Recording_Title");
                recordingTitle.GetComponent<Text>().text = recordingNames[i];
            }
        }
    }
}
