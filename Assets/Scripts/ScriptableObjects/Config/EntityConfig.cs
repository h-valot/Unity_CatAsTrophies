using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EntityConfig", menuName = "Config/Entity/Entity", order = 1)]
public class EntityConfig : ScriptableObject
{
    [HideInInspector] public bool isCat;
    public string id;
    
    [Header("STATS")]
    public string entityName;
    public float health;
    public int armorAtStart;
    public Sprite sprite;
    
    [Header("ABILITY")]
    public Ability ability = new Ability();
    public List<Ability> autoAttack = new List<Ability>();

    [Header("GRAPHICS")]
    public GameObject basePrefab;
    public Texture catSkinTexture;
    public Texture catEyesTexture;
    public GameObject rightHandAddon;
    public GameObject leftHandAddon;
    public GameObject headAddon;

    [Header("REWARD")] 
    public bool canBeReward;
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
    PREMIUM
}