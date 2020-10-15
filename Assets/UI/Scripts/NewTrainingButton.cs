using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;
public class NewTrainingButton : MonoBehaviour
{
    public GameObject theText;
    public GameObject thePanel;

    public GameObject MainPanel_1;
    public GameObject AddVideoPanel;

    public GameObject MainPanel_2;

    public GameObject FilePanel;

    public GameObject ApplyPanel;

    public GameObject ResultPanel;

    public GameObject FileTab;
    public GameObject ApplyTab;
    public GameObject ResultTab;

    Color32 titleColor;
    Color32 subTitleColor;
    // on start 
    void Start()
    {
        // MainPanel_1.SetActive(false);     
        titleColor = new Color32(149, 130, 178, 255);
        subTitleColor = new Color32(149, 130, 178, 153);
        openFP()  ;
        MainPanel_1.SetActive(false);
        AddVideoPanel.SetActive(false);
        MainPanel_2.SetActive(false);
    }
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

        FileTab.GetComponentInChildren<Text>().text = firstLine;
        reader.Close();
    }

    public void openFP()
    {
        
        FileTab.GetComponentInChildren<Text>().color = titleColor;
        ApplyTab.GetComponentInChildren<Text>().color = subTitleColor;
        ResultTab.GetComponentInChildren<Text>().color = subTitleColor;

        FilePanel.SetActive(true);
        ApplyPanel.SetActive(false);
        ResultPanel.SetActive(false);

        // SingleFileSelector();
        ReadString();
    }

    public void openAP()
    {
        FileTab.GetComponentInChildren<Text>().color = subTitleColor;
        ApplyTab.GetComponentInChildren<Text>().color = titleColor;
        ResultTab.GetComponentInChildren<Text>().color = subTitleColor;


        FilePanel.SetActive(false);
        ApplyPanel.SetActive(true);
        ResultPanel.SetActive(false);
    }

    public void openRP()
    {
        FileTab.GetComponentInChildren<Text>().color = subTitleColor;
        ApplyTab.GetComponentInChildren<Text>().color = subTitleColor;
        ResultTab.GetComponentInChildren<Text>().color = titleColor;

        FilePanel.SetActive(false);
        ApplyPanel.SetActive(false);
        ResultPanel.SetActive(true);
    }
    //open main panel when click new Training
    public void OpenMp1()
    {
        MainPanel_1.SetActive(true);
    }

    //from mp1 to mp2 when click next button
    public void Mp1ToMp2()
    {
        MainPanel_1.SetActive(false);
        MainPanel_2.SetActive(true);
    }

    //from mp2 to mp1 when click back button
    public void Mp2ToMp1()
    {
        MainPanel_1.SetActive(true);
        MainPanel_2.SetActive(false);
    }
    

    public void Submit()
    {
        MainPanel_1.SetActive(false);
        MainPanel_2.SetActive(false);
    }

    // open the addnvideopanel
    public void OpenNewVideoPanel()
    {
        AddVideoPanel.SetActive(true);
    }
    // close the addnvideopanel
    public void CloseNewVideoPanel()
    {
        AddVideoPanel.SetActive(false);
    }
    // submit add new video
    public void AddNewVideo()
    {
        AddVideoPanel.SetActive(false);
        MainPanel_1.SetActive(true);
    }

    public void Close()
    {
        MainPanel_1.SetActive(false);
        AddVideoPanel.SetActive(false);
        MainPanel_2.SetActive(false);
    }

    

    public void ClearText()
    {
        theText.GetComponent<InputField>().text = "";
    }

    public void CancelButton()
    {
        thePanel.SetActive(false);
    }

    public void OpenPanel()
    {
        thePanel.SetActive(true);
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}

