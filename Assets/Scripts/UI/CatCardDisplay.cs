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
        nameTM.text = Registry.catConfig.cats[Misc.GetCatById(catId).typeIndex].catName;
        healthTM.text = $"{Misc.GetCatById(catId).health}";
    }

    public void Hide() => graphicsParent.SetActive(false);

    public void Show() => graphicsParent.SetActive(true);
}