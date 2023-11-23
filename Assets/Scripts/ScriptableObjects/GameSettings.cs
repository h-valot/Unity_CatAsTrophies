using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Config/GameSettings", order = 1)]
public class GameSettings : ScriptableObject
{
    public string startingScene;

    [Header("ANIMATIONS")] 
    public float abilityAnimationDuration;

    [Header("GAMEPLAY")] 
    public float holdingTimeToTriggerAbility;

    [Header("EFFECTS")]
    public int dotDamageAmount;
    public int hotHealAmount;
    [Range(0f, 1f)] public float damageResistanceModifier;
    public int antiHealAmout;
    public int buffArmorAmount;
    public int debuffArmorAmout;
    public int buffAttackAmout;
    public int debuffAttackAmout;
    
}