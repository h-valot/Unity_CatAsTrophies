using System.Collections.Generic;
using Data;
using List;
using UnityEngine;

public class RewardUIManager : MonoBehaviour
{
    [Header("REFERENCES")] 
    public RewardUIButton[] buttons;
    
    public void UpdateDisplay()
    {
        for (int index = 0; index < buttons.Length; index++)
        {
            var isLastButton = index == buttons.Length - 1;
            buttons[index].UpdateDisplay(GetRandomReward(isLastButton));
        }
    }

    private RewardConfig GetRandomReward(bool mustBePremium)
    {
        List<RewardConfig> rewardCandidates = new List<RewardConfig>();
            
        foreach (var reward in Registry.rewardsConfig.rewards)
        {
            foreach (var tier in reward.apparitionTiers)
            {
                // continue, if the tier doesn't match the beaten one
                if (tier != DataManager.data.compoToLoad.tier) continue;
                
                // get premium candidates
                if (mustBePremium)
                {
                    // continue, if the pricing isn't premium
                    if (reward.pricing != RewardPricing.PAID) continue;
                    
                    rewardCandidates.Add(reward);
                    break;
                }
                
                // continue, if the pricing isn't free
                if (reward.pricing != RewardPricing.FREE) continue;
                
                rewardCandidates.Add(reward);
                break;
            }
        }

        rewardCandidates.Shuffle();
        return rewardCandidates[Random.Range(0, rewardCandidates.Count - 1)];
    }
}
