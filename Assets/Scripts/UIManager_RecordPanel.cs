using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager_RecordPanel : MonoBehaviour
{
    public UIManager uiManager;

    public KinectInterface kinectInterface;
    public DataManager dataManager;

    public InputField recordingName;

    public TMP_Dropdown modelToApply_Dropdown;

    public GameObject[] labelToggles;

    public GameObject recordButton;
    public GameObject recordingButton;

    public void RefreshPanel()
    {
        RefreshModelToApplyDropdown(dataManager.GetModelNames());
        SelectModel(modelToApply_Dropdown);
    }


    public void RefreshModelToApplyDropdown(List<string> options)
    {
        modelToApply_Dropdown.ClearOptions();
        modelToApply_Dropdown.AddOptions(options);
    }

    public void SelectModel(TMP_Dropdown dropdown)
    {
        // if we don't have any models yet, and therefore no dropdown option to pick a model, don't try to select a model
        // but make sure to clear the labels list
        if(dropdown.options.Count <= 0)
        {
            UpdateLabelToggles(new List<string>());
            return;
        }
        string selectedModelName = dropdown.options[dropdown.value].text;
        //Debug.Log("selectedModelName: " + selectedModelName);
        List<string> labels = dataManager.GetLabelsFromModel(selectedModelName);
        UpdateLabelToggles(labels);
    }

    public void UpdateLabelToggles(List<string> labels)
    {
        // this should never happen because the drop down is populated from the datamanager
        // and the value of the drop down is what's used to pull the name of the model
        if(labels == null)
        {
            Debug.LogWarning("Selected model has no labels");
            uiManager.SetTalkbackMessage("Selected model has no labels");
            return;
        }

        // turn off all the label toggles
        for(int i = 0; i < labelToggles.Length; i++)
        {
            labelToggles[i].SetActive(false);
        }

        // turn on only the needed toggles and update the text
        for(int i = 0; i < labels.Count; i++)
        {
            labelToggles[i].SetActive(true);
            labelToggles[i].transform.Find("Label").GetComponent<Text>().text = labels[i];
        }
    }

    public void StartRecording()
    {
        if(recordingName.text == "")
        {
            uiManager.SetTalkbackMessage("You must specify a recording name!");
            return;
        }
        if(modelToApply_Dropdown.options.Count <= 0)
        {
            uiManager.SetTalkbackMessage("You must specify a learning model!");
            return;
        }

        List<string> recordingLabels = new List<string>();
        for (int i = 0; i < labelToggles.Length; i++)
        {
            if (labelToggles[i].GetComponent<Toggle>().isOn)
            {
                recordingLabels.Add(labelToggles[i].transform.Find("Label").GetComponent<Text>().text);
            }
        }

        if(recordingLabels.Count <= 0)
        {
            uiManager.SetTalkbackMessage("You must specify at least one label!");
            return;
        }

        recordButton.SetActive(false);
        recordingButton.SetActive(true);

        kinectInterface.StartRecording(recordingName.text);
    }

    public void StopRecording()
    {
        recordButton.SetActive(true);
        recordingButton.SetActive(false);

        kinectInterface.StopRecording();

        string modelName = modelToApply_Dropdown.options[modelToApply_Dropdown.value].text;
        List<string> recordingLabels = new List<string>();
        for (int i = 0; i < labelToggles.Length; i++)
        {
            if (labelToggles[i].GetComponent<Toggle>().isOn)
            {
                recordingLabels.Add(labelToggles[i].transform.Find("Label").GetComponent<Text>().text);
            }
        }
        dataManager.SaveRecording(recordingName.text, modelName, recordingLabels);
    }
}
