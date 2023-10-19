using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI text;
    [SerializeField] public string text1;
    [SerializeField] public string text2;
    [SerializeField] public bool isActivated;

    public void OnButtonClick()
    {
        if (isActivated == true)
        {
            text.text = text1;
            isActivated = false;
        }
        else
        {
            text.text = text2;
            isActivated = true;
        }
    }
    //Switch 2 texts by clicking the same button

    public void Quit()
    {
        Application.Quit();
        #if UNITY_EDITOR
        EditorApplication.isPlaying = false;
        Debug.Log("Quitter le jeu?");
        #endif
    }
    //Exit game button
}
