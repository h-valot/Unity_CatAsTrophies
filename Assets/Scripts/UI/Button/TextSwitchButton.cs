using UnityEngine;
using TMPro;
using UnityEditor;

public class TextSwitchButton : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] public TextMeshProUGUI text;
    
    [Header("TEXTS")]
    [SerializeField] public string text1;
    [SerializeField] public string text2;
    
    private bool isActivated;

    /// <summary>
    /// Switch 2 texts by clicking the same button
    /// </summary>
    public void OnButtonClick()
    {
        if (isActivated)
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