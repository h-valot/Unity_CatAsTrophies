using UnityEngine;
using UnityEngine.UI;

public class ShopbuttonManager : MonoBehaviour
{
    [Header("REFERENCES")] 
    public GameObject graphicsParent;
    public Image featuredTabImage, catsTabImage, boostsTabImage, coinsTabImage;
    public GameObject featuredPanel, catsPanel, boostsPanel, coinsPanel;
    public GameObject catsCollectionButton;

    [Header("SETTINGS")] 
    public Color unselectedTabColor;

    public void ShowNormal()
    {
        graphicsParent.SetActive(true);
        catsCollectionButton.SetActive(true);
        OpenFeaturedPanel();
    }
    
    public void ShowWithoutCatsButton()
    {
        graphicsParent.SetActive(true);
        catsCollectionButton.SetActive(false);
        OpenCoinsPanel();
    }
    
    
    public void Hide() => graphicsParent.SetActive(false);
    
    public void OpenFeaturedPanel()
    {
        HideAllPanels();
        SetAllTabsUnselected();
        
        featuredPanel.SetActive(true);
        featuredTabImage.color = Color.white;
    }
    
    public void OpenCatsPanel()
    {
        HideAllPanels();
        SetAllTabsUnselected();
        
        catsPanel.SetActive(true);
        catsTabImage.color = Color.white;
    }
    
    public void OpenBoostsPanel()
    {
        HideAllPanels();
        SetAllTabsUnselected();
        
        boostsPanel.SetActive(true);
        boostsTabImage.color = Color.white;
    }
    
    public void OpenCoinsPanel()
    {
        HideAllPanels();
        SetAllTabsUnselected();
        
        coinsPanel.SetActive(true);
        coinsTabImage.color = Color.white;
    }

    private void SetAllTabsUnselected()
    {
        featuredTabImage.color = unselectedTabColor;
        catsTabImage.color = unselectedTabColor;
        boostsTabImage.color = unselectedTabColor;
        coinsTabImage.color = unselectedTabColor;
    }

    private void HideAllPanels()
    {
        featuredPanel.SetActive(false);
        catsPanel.SetActive(false);
        boostsPanel.SetActive(false);
        coinsPanel.SetActive(false);
    }
}