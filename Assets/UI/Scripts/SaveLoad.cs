using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveLoad : MonoBehaviour
{

    public string theText;
    public GameObject newContent;
    public GameObject placeHolder;
    public GameObject savedAnim;
    // Start is called before the first frame update

    void Start()
    {
        theText = PlayerPrefs.GetString("InputContents");
        placeHolder.GetComponent<InputField>().text = theText;
    }

    public void SaveNote()
    {
        theText = newContent.GetComponent<Text>().text;
        PlayerPrefs.SetString("InputContents", theText);
        StartCoroutine(SaveTextRoll());
    }

    public void LoadNote()
    {
        theText = PlayerPrefs.GetString("InputContents");
        placeHolder.GetComponent<InputField>().text = theText;
    }
    IEnumerator SaveTextRoll()
    {
        savedAnim.GetComponent<Animator>().Play("SavedAnim");
        yield return new WaitForSeconds(1);
        savedAnim.GetComponent<Animator>().Play("New State");
    }
}
