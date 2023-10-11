using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI text;
    [SerializeField] public bool isActivated;

    public void OnButtonClick()
    {
        if (isActivated == true)
        {
            text.text = "Colorblind: OFF";
            isActivated = false;
        }
        else
        {
            text.text = "Colorblind: ON";
            isActivated = true;
        }    
    }
}
