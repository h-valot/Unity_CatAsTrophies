using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
}
