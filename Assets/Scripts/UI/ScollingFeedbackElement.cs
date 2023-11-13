using TMPro;
using UnityEngine;

public class ScrollingFeedbackElement : MonoBehaviour
{
    public TextMeshProUGUI ScrollingFeedbackTM;

    public void UpdateDisplay(string text)
    {
        ScrollingFeedbackTM.text = text;
    }
}