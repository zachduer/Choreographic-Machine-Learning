using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ModelItem_Controller : MonoBehaviour
{
    public void LoadModel()
    {
        string modelName = transform.Find("Model_Title").GetComponent<Text>().text;
        GameObject.Find("ModelPanel").GetComponent<UIManager_ModelPanel>().LoadModel(modelName);
    }

    public void DeleteModel()
    {
        string modelName = transform.Find("Model_Title").GetComponent<Text>().text;
        GameObject.Find("ModelPanel").GetComponent<UIManager_ModelPanel>().DeleteModel(modelName);
    }
}
