using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Config/GameSettings", order = 2)]
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
    public float damageResistanceModifier;
}