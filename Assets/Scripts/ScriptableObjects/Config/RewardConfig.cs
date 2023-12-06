using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Reward", menuName = "Config/Reward/Reward", order = 2)]
public class RewardConfig : ScriptableObject
{
    public string id;

    public Sprite sprite;
    public string rewardName;
    public int rewardCatId;

    public List<CompositionTier> apparitionTiers = new List<CompositionTier>();
    
    public RewardPricing pricing;
    public float cost;

    public void Initialize()
    {
        apparitionTiers.Add(CompositionTier.SIMPLE);
    }
}

public enum RewardPricing
{
    FREE = 0,
    PAID
}