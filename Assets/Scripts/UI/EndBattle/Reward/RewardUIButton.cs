using Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardUIButton : MonoBehaviour
{
    [Header("REFERENCES")] 
    public GameObject graphicsParent;
    public HoldUIButton holdButton;
    public Image rewardImage;
    public GameObject buyButton;
    public TextMeshProUGUI rewardNameTM;

    private int catRewardIndex;

    public void UpdateDisplay(int newCatRewardIndex)
    {
        catRewardIndex = newCatRewardIndex;
        
        // lock the button, if the cat reward is premium
        if (Registry.entitiesConfig.cats[catRewardIndex].pricing == RewardPricing.PREMIUM) holdButton.Lock();
        
        rewardImage.sprite = Registry.entitiesConfig.cats[catRewardIndex].sprite;
        rewardNameTM.text = Registry.entitiesConfig.cats[catRewardIndex].entityName;
        
        buyButton.SetActive(Registry.entitiesConfig.cats[catRewardIndex].pricing == RewardPricing.PREMIUM);
    }
    
    public void GatherReward()
    {
        // add the catReward to the player's run deck
        DataManager.data.playerStorage.AddToInGameDeck(catRewardIndex);
    }
}