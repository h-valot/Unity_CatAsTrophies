using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Config/GameSettings", order = 2)]
public class GameSettings : ScriptableObject
{
    public string startingScene;

    [Header("ANIMATIONS")]
    public float delayBeforeAnimation;
    public float abilityAnimationDuration;
    public float delayAfterAnimation;

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

    [Header("SCROLLING TEXT EFFECT")]
    public float scrollingFeedbackLifetime;
    public float defaultFontSize;
    public float effectFontSize;
    public Color colorTextDamage;
    public Color colorTextHeal;
    public Color colorTextArmor;
    public Color colorTextEffect;
}