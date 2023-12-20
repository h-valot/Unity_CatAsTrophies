using Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardUIButton : MonoBehaviour
{
    [Header("REFERENCES")] 
    public GameObject imageParent;
    public GameObject infoParent;

    [Header("GLOBAL")]
    public TextMeshProUGUI catNameTM;
    public GameObject selectButton;
    public GameObject buyButton;

    [Header("REWARD")] 
    public Image catFaceImage;
    public Image catBackgroundImage;
    public Sprite epicBackground, commonBackground;
    
    [Header("INFO")]
    public TextMeshProUGUI infoCatHealthTM;
    public TextMeshProUGUI infoCatAbilityTM;

    private int _catRewardIndex;
    private bool _isInfoShown;

    public void UpdateDisplay(int newCatRewardIndex)
    {
        _catRewardIndex = newCatRewardIndex;
        
        catNameTM.text = Registry.entitiesConfig.cats[_catRewardIndex].entityName;
        
        catBackgroundImage.sprite = Registry.entitiesConfig.cats[_catRewardIndex].rarety == Rarety.EPIC ? epicBackground : commonBackground;
        catFaceImage.sprite = Registry.entitiesConfig.cats[_catRewardIndex].sprite;

        infoCatHealthTM.text = $"{Registry.entitiesConfig.cats[_catRewardIndex].health}";
        infoCatAbilityTM.text = $"{Registry.entitiesConfig.cats[_catRewardIndex].abilityDescription}";
        
        selectButton.SetActive(Registry.entitiesConfig.cats[_catRewardIndex].pricing == RewardPricing.FREE);
        buyButton.SetActive(Registry.entitiesConfig.cats[_catRewardIndex].pricing == RewardPricing.PREMIUM);
    }

    public void ShowInfo()
    {
        _isInfoShown = true;
        infoParent.SetActive(true);
        imageParent.SetActive(false);
    }
    public void HideInfo()
    {
        _isInfoShown = false;
        infoParent.SetActive(false);
        imageParent.SetActive(true);
    }
    
    /// <summary>
    /// Adds the catReward to the player's run deck
    /// </summary>
    public void GatherReward()
    {
        DataManager.data.playerStorage.AddToInGameDeck(_catRewardIndex);
    }
}