using System.Collections.Generic;
using System.Linq;
using Data;
using List;
using UnityEngine;

public class RewardUIManager : MonoBehaviour
{
    [Header("REFERENCES")] 
    public ShopbuttonManager shopbuttonManager;
    public GameObject graphicsParent;
    public RewardUIButton[] buttons;

    [Header("DEBUGGING")] 
    public List<int> rewardsSelected = new List<int>();
    
    public void UpdateDisplay()
    {
        for (int index = 0; index < buttons.Length; index++)
        {
            var isLastButton = index == buttons.Length - 1;
            buttons[index].UpdateDisplay(GetRandomCatReward(isLastButton));
        }
    }

    private int GetRandomCatReward(bool mustBePremium)
    {
        List<int> rewardCandidates = new List<int>();

        for (int index = 0; index < Registry.entitiesConfig.cats.Count; index++)
        {
            // continue, if the cat can't be a reward
            if (!Registry.entitiesConfig.cats[index].canBeReward) continue;

            // continue, if the cat is already a reward
            if (rewardsSelected.Any(rewardIndex => rewardIndex == index)) continue;

            foreach (var tier in Registry.entitiesConfig.cats[index].apparitionTiers)
            {
                // continue, if the tier doesn't match the beaten one
                if (tier != DataManager.data.compoToLoad.tier) continue;

                // get premium candidates
                if (mustBePremium)
                {
                    // continue, if the pricing isn't premium
                    if (Registry.entitiesConfig.cats[index].pricing != RewardPricing.PREMIUM) continue;

                    rewardCandidates.Add(index);
                    break;
                }

                // continue, if the pricing isn't free
                if (Registry.entitiesConfig.cats[index].pricing != RewardPricing.FREE) continue;

                rewardCandidates.Add(index);
                break;
            }
        }

        rewardCandidates.Shuffle();
        int rewardSelected = rewardCandidates[Random.Range(0, rewardCandidates.Count - 1)];
        rewardsSelected.Add(rewardSelected);
        return rewardSelected;
    }
    
    public void Show() => graphicsParent.SetActive(true);
    public void Hide() => graphicsParent.SetActive(false);

    public void ShowShop()
    {
        shopbuttonManager.ShowWithoutCatsButton();
    }
}
