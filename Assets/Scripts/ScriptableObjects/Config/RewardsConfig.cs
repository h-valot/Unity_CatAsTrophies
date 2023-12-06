using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RewardsConfig", menuName = "Config/Reward/Rewards", order = 1)]
public class RewardsConfig : ScriptableObject
{
    [Header("REWARDS")]
    public List<RewardConfig> rewards = new List<RewardConfig>();
}