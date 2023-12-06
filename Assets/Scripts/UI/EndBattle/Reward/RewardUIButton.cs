using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardUIButton : MonoBehaviour
{
    [Header("REFERENCES")] 
    public GameObject graphicsParent;
    public Image rewardImage;
    public GameObject buyButton;
    public TextMeshProUGUI rewardNameTM;

    public void UpdateDisplay(RewardConfig reward)
    {
        rewardImage.sprite = reward.sprite;
        buyButton.SetActive(reward.pricing == RewardPricing.PAID);
        rewardNameTM.text = reward.rewardName;
    }

    public void GatherReward()
    {
        // add reward.rewardCatId to the player's run deck
    }
}