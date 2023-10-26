using TMPro;
using UnityEngine;

public class CatCardDisplay : MonoBehaviour
{
    public GameObject graphicsParent;
    public string catId;
    public TextMeshProUGUI nameTM, healthTM;

    public void Initialize(string _catId)
    {
        catId = _catId;
    }
    
    public void UpdateDisplay()
    {
        nameTM.text = Registry.catConfig.cats[Misc.GetCatById(catId).catType].catName;
        healthTM.text = $"{Misc.GetCatById(catId).health}";
    }
}