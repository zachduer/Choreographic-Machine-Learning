using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;

public class UIManager : MonoBehaviour
{
    public DataManager dataManager;

    //public GameObject theText;
    public GameObject introPanel;

    public GameObject modelPanel;
    public GameObject recordPanel;
    public GameObject trainPanel;
    public GameObject filePanel;
    public GameObject classifyPanel;
    public GameObject resultPanel;

    public GameObject modelTab;
    public GameObject recordTab;
    public GameObject trainTab;
    public GameObject fileTab;
    public GameObject classifyTab;
    public GameObject resultTab;

    public UIManager_RecordPanel uiManager_RecordPanel;
    public UIManager_ModelPanel uiManager_ModelPanel;
    public UIManager_TrainPanel uiManager_TrainPanel;
    public UIManager_ClassifyPanel uiManager_ClassifyPanel;

    //public TMP_Dropdown recordPanel_ModelToApplyDropdown;

    public Text talkback;

    //public GameObject addVideoPanel;
    public GameObject mainPanel_2;



    Color32 titleColor;
    Color32 subTitleColor;
    // on start 
    void Start()
    {
        titleColor = new Color32(149, 130, 178, 255);
        subTitleColor = new Color32(149, 130, 178, 153);

        OpenIntroPanel();

        //introPanel.SetActive(true);
        //trainPanel.SetActive(false);
        //mainPanel_2.SetActive(false);
    }
    /*
    public void SingleFileSelector()
    {
        string path = EditorUtility.OpenFilePanel("Select Skeleton File", "", "fbx");
        if (path != "")
        {
            Debug.Log(path);
        }
    }

    public void MutipleFileSelector()
    {
        string path = EditorUtility.OpenFilePanel("Select Skeleton File", "", "");
        if (path != "")
        {
            string[] files = Directory.GetFiles(path); //This array contains all the files within the selected folder
            for (int i = 0; i < files.Length; i++)
            {
                Debug.Log(files[i]);
            }
        }

    }

    public void ReadString()
    {
        string path = EditorUtility.OpenFilePanel("Select Skeleton File", "", "");
        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path);
        string firstLine = reader.ReadLine();
        if (firstLine == null) firstLine = "";

        fileTab.GetComponentInChildren<Text>().text = firstLine;
        reader.Close();
    }
    */

    

    public void OpenIntroPanel()
    {
        modelTab.GetComponentInChildren<Text>().color = subTitleColor;
        recordTab.GetComponentInChildren<Text>().color = subTitleColor;
        trainTab.GetComponentInChildren<Text>().color = subTitleColor;
        fileTab.GetComponentInChildren<Text>().color = subTitleColor;
        classifyTab.GetComponentInChildren<Text>().color = subTitleColor;
        resultTab.GetComponentInChildren<Text>().color = subTitleColor;

        introPanel.SetActive(true);
        modelPanel.SetActive(false);
        recordPanel.SetActive(false);
        trainPanel.SetActive(false);
        filePanel.SetActive(false);
        classifyPanel.SetActive(false);
        resultPanel.SetActive(false);
    }

    public void OpenModelPanel()
    {
        modelTab.GetComponentInChildren<Text>().color = titleColor;
        recordTab.GetComponentInChildren<Text>().color = subTitleColor;
        trainTab.GetComponentInChildren<Text>().color = subTitleColor;
        fileTab.GetComponentInChildren<Text>().color = subTitleColor;
        classifyTab.GetComponentInChildren<Text>().color = subTitleColor;
        resultTab.GetComponentInChildren<Text>().color = subTitleColor;

        modelPanel.SetActive(true);
        recordPanel.SetActive(false);
        trainPanel.SetActive(false);
        filePanel.SetActive(false);
        classifyPanel.SetActive(false);
        resultPanel.SetActive(false);

        uiManager_ModelPanel.RefreshPanel();
    }

    public void OpenRecordPanel()
    {
        modelTab.GetComponentInChildren<Text>().color = subTitleColor;
        recordTab.GetComponentInChildren<Text>().color = titleColor;
        trainTab.GetComponentInChildren<Text>().color = subTitleColor;
        fileTab.GetComponentInChildren<Text>().color = subTitleColor;
        classifyTab.GetComponentInChildren<Text>().color = subTitleColor;
        resultTab.GetComponentInChildren<Text>().color = subTitleColor;

        modelPanel.SetActive(false);
        recordPanel.SetActive(true);
        trainPanel.SetActive(false);
        filePanel.SetActive(false);
        classifyPanel.SetActive(false);
        resultPanel.SetActive(false);

        uiManager_RecordPanel.RefreshPanel();
    }

    public void OpenTrainingPanel()
    {
        modelTab.GetComponentInChildren<Text>().color = subTitleColor;
        recordTab.GetComponentInChildren<Text>().color = subTitleColor;
        trainTab.GetComponentInChildren<Text>().color = titleColor;
        fileTab.GetComponentInChildren<Text>().color = subTitleColor;
        classifyTab.GetComponentInChildren<Text>().color = subTitleColor;
        resultTab.GetComponentInChildren<Text>().color = subTitleColor;

        modelPanel.SetActive(false);
        recordPanel.SetActive(false);
        trainPanel.SetActive(true);
        filePanel.SetActive(false);
        classifyPanel.SetActive(false);
        resultPanel.SetActive(false);

        uiManager_TrainPanel.RefreshPanel();
    }

    public void OpenClassifyPanel()
    {
        modelTab.GetComponentInChildren<Text>().color = subTitleColor;
        recordTab.GetComponentInChildren<Text>().color = subTitleColor;
        trainTab.GetComponentInChildren<Text>().color = subTitleColor;
        classifyTab.GetComponentInChildren<Text>().color = subTitleColor;
        fileTab.GetComponentInChildren<Text>().color = subTitleColor;
        classifyTab.GetComponentInChildren<Text>().color = subTitleColor;
        resultTab.GetComponentInChildren<Text>().color = subTitleColor;

        modelPanel.SetActive(false);
        recordPanel.SetActive(false);
        trainPanel.SetActive(false);
        filePanel.SetActive(false);
        classifyPanel.SetActive(true);
        resultPanel.SetActive(false);

        uiManager_ClassifyPanel.RefreshPanel();
    }

    public void SetTalkbackMessage(string message)
    {
        talkback.text = message;
    }





    //public void openFP()
    //{
        
    //    fileTab.GetComponentInChildren<Text>().color = titleColor;
    //    applyTab.GetComponentInChildren<Text>().color = subTitleColor;
    //    resultTab.GetComponentInChildren<Text>().color = subTitleColor;

    //    filePanel.SetActive(true);
    //    applyPanel.SetActive(false);
    //    resultPanel.SetActive(false);

    //    // SingleFileSelector();
    //    //ReadString();
    //}

    //public void openAP()
    //{
    //    fileTab.GetComponentInChildren<Text>().color = subTitleColor;
    //    applyTab.GetComponentInChildren<Text>().color = titleColor;
    //    resultTab.GetComponentInChildren<Text>().color = subTitleColor;


    //    filePanel.SetActive(false);
    //    applyPanel.SetActive(true);
    //    resultPanel.SetActive(false);
    //}

    //public void openRP()
    //{
    //    fileTab.GetComponentInChildren<Text>().color = subTitleColor;
    //    applyTab.GetComponentInChildren<Text>().color = subTitleColor;
    //    resultTab.GetComponentInChildren<Text>().color = titleColor;

    //    filePanel.SetActive(false);
    //    applyPanel.SetActive(false);
    //    resultPanel.SetActive(true);
    //}
    //open main panel when click new Training
    //public void OpenMp1()
    //{
    //    trainPanel.SetActive(true);
    //}

    ////from mp1 to mp2 when click next button
    //public void Mp1ToMp2()
    //{
    //    trainPanel.SetActive(false);
    //    mainPanel_2.SetActive(true);
    //}

    ////from mp2 to mp1 when click back button
    //public void Mp2ToMp1()
    //{
    //    trainPanel.SetActive(true);
    //    mainPanel_2.SetActive(false);
    //}
    

    //public void Submit()
    //{
    //    trainPanel.SetActive(false);
    //    mainPanel_2.SetActive(false);
    //}

    //// open the addnvideopanel
    //public void OpenNewVideoPanel()
    //{
    //    //addVideoPanel.SetActive(true);
    //}
    //// close the addnvideopanel
    //public void CloseNewVideoPanel()
    //{
    //   // addVideoPanel.SetActive(false);
    //}
    //// submit add new video
    //public void AddNewVideo()
    //{
    //    //addVideoPanel.SetActive(false);
    //    trainPanel.SetActive(true);
    //}

    //public void Close()
    //{
    //    trainPanel.SetActive(false);
    //   // addVideoPanel.SetActive(false);
    //    mainPanel_2.SetActive(false);
    //}

    

    //public void ClearText()
    //{
    //    //theText.GetComponent<InputField>().text = "";
    //}

    //public void CancelButton()
    //{
    //    //thePanel.SetActive(false);
    //}

    //public void OpenPanel()
    //{
    //   // thePanel.SetActive(true);
    //}

    //public void QuitButton()
    //{
    //    Application.Quit();
    //}
}

