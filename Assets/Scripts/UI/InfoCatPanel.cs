using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoCatPanel : MonoBehaviour
{
    public GameObject graphicsParent;
    public TextMeshProUGUI nameCat;
    public TextMeshProUGUI infoCat;

    // Start is called before the first frame update
    void OnEnable()
    {
        Registry.events.OnCatStacked += DisplayCatInfo;
        Registry.events.OnCatDestacked += RemoveCatInfo;
    }

    private void DisplayCatInfo(string catTypeName, string catTypeInfo)
    {
        graphicsParent.SetActive(true);
        nameCat.text = catTypeName;
        infoCat.text = catTypeInfo;
    }

    private void RemoveCatInfo()
    {
        graphicsParent.SetActive(false);
    }
}
