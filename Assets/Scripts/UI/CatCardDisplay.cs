using TMPro;
using UnityEngine;

public class CatCardDisplay : MonoBehaviour
{
    public GameObject graphicsParent;
    public TextMeshProUGUI nameTM, healthTM;

    public void UpdateDisplay(string _name, float _health)
    {
        nameTM.text = _name;
        healthTM.text = _health.ToString();
    }

    public void Hide()
    {
        graphicsParent.SetActive(false);
    }

    public void Show()
    {
        graphicsParent.SetActive(true);
    }
}