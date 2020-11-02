using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UIManager_ModelPanel : MonoBehaviour
{
    public DataManager dataManager;
    public UIManager uiManager;

    public InputField newModelNameInputField;
    public Toggle[] newModelJointToggles;
    public InputField newModelLabelsInputField;

    public GameObject modelsListGrid;

    public GameObject UI_ModelItem_Prefab;

    public void RefreshPanel()
    {
        //ClearFields();
        RefreshModelsUIList();
    }

    public void SaveModel()
    {
        if (newModelNameInputField.text == "")
        {
            uiManager.SetTalkbackMessage("You must enter a name for the new model!");
            return;
        }
        uiManager.talkback.text = "";

        string modelName = newModelNameInputField.text;

        List<string> features = new List<string>();

        for (int i = 0; i < newModelJointToggles.Length; i++)
        {
            if (newModelJointToggles[i].GetComponent<Toggle>().isOn)
            {
                features.Add(newModelJointToggles[i].name);
            }
        }

        string[] tempLabels = newModelLabelsInputField.text.Split(',');
        List<string> labels = tempLabels.OfType<string>().ToList<string>();

        dataManager.SaveModel(modelName, features, labels);

        RefreshModelsUIList();
    }

    public void RefreshModelsUIList()
    {
        // Delete the existing UI objects
        foreach (Transform child in modelsListGrid.transform)
        {
            // don't delete the NewModel button
            if(child.name == "NewModel")
            {
                continue;
            }
            Destroy(child.gameObject);
        }

        // Get the list of models names from the data manager
        List<string> modelNames = dataManager.GetModelNames();

        // Add an ModelUI object for each model name
        for(int i = 0; i < modelNames.Count; i++)
        {
            GameObject newUIModelItem = Instantiate(UI_ModelItem_Prefab);
            newUIModelItem.transform.parent = modelsListGrid.transform;
            Transform modelTitle = newUIModelItem.transform.Find("Model_Title");
            modelTitle.GetComponent<Text>().text = modelNames[i];
        }
    }

    public void ClearFields()
    {
        newModelNameInputField.text = "";

        for (int i = 0; i < newModelJointToggles.Length; i++)
        {
            newModelJointToggles[i].GetComponent<Toggle>().isOn = false;
        }

        newModelLabelsInputField.text = "";
    }

    public void LoadModel(string modelName)
    {
        newModelNameInputField.text = modelName;

        List<string> features = dataManager.GetFeaturesFromModel(modelName);

        // loop through all the toggles.  for each toggle, get its name (the feature name).  
        // check to see if the list of features from the model contains that feature name.  if so, turn the toggle on.
        for (int i = 0; i < newModelJointToggles.Length; i++)
        {
            if (features.Contains(newModelJointToggles[i].name))
            {
                newModelJointToggles[i].GetComponent<Toggle>().isOn = true;
            }
        }

        List<string> labels = dataManager.GetLabelsFromModel(modelName);
        string labelsList = "";
        for (int i = 0; i < labels.Count; i++)
        {
            // if we're not on the last label yet, then add the label and a comma
            // otherwise, if we're on the last label, then add the label but no comma
            if (i < labels.Count - 1) {
                labelsList += labels[i] + ", ";
            }
            else{
                labelsList += labels[i];
            }
        }
        newModelLabelsInputField.text = labelsList;
    }

    public void DeleteModel(string modelName)
    {
        dataManager.DeleteModel(modelName);
        RefreshModelsUIList();
    }
}
