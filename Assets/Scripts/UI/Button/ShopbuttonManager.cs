using UnityEngine;
using UnityEngine.UI;

public class ShopbuttonManager : MonoBehaviour
{
    public Image featuredTabImage, catsTabImage, boostsTabImage, coinsTabImage;
    public Transform featuredPanel, catsPanel, boostsPanel, coinsPanel;

    [Header("SETTINGS")] 
    public Color unselectedTabColor;

    public void OpenFeaturedPanel()
    {
        featuredPanel.SetAsLastSibling();
        SetAllTabsUnselected();
        featuredTabImage.color = Color.white;
    }
    
    public void OpenCatsPanel()
    {
        catsPanel.SetAsLastSibling();
        SetAllTabsUnselected();
        catsTabImage.color = Color.white;
    }
    
    public void OpenBoostsPanel()
    {
        boostsPanel.SetAsLastSibling();
        SetAllTabsUnselected();
        boostsTabImage.color = Color.white;
    }
    
    public void OpenCoinsPanel()
    {
        coinsPanel.SetAsLastSibling();
        SetAllTabsUnselected();
        coinsTabImage.color = Color.white;
    }

    private void SetAllTabsUnselected()
    {
        featuredTabImage.color = unselectedTabColor;
        catsTabImage.color = unselectedTabColor;
        boostsTabImage.color = unselectedTabColor;
        coinsTabImage.color = unselectedTabColor;
    }
}