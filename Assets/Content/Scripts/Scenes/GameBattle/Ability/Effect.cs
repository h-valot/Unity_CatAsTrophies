[System.Serializable]
public class Effect
{
    public EffectType type;
    public int turnDuration;
    public bool isHarmful;
    public string sourceId;

    public Effect(EffectType type, int turnDuration, string sourceId)
    {
        this.type = type;
        this.turnDuration = turnDuration;
        this.sourceId = sourceId;
    }
    
    public void Trigger()
    {
        switch (type)
        {
            case EffectType.DOT: // usable
                Misc.IdManager.GetEntityById(sourceId).UpdateHealth(-Registry.gameSettings.dotDamageAmount);
                break;
            case EffectType.HOT: // usable
                Misc.IdManager.GetEntityById(sourceId).HealUpdate(Registry.gameSettings.hotHealAmount);
                break;
            case EffectType.DEBUFF_ATTACK: // usable
                break;
            case EffectType.DEBUFF_ARMOR: // usable
                break;
            case EffectType.STUN: // usable
                break;
            case EffectType.SLEEP: // usable
                break;
            case EffectType.ANTI_HEAL: // usable
                break;
            case EffectType.BUFF_ATTACK: // usable
                break;
            case EffectType.BUFF_ARMOR: // usable
                break;
            case EffectType.RESISTANCE: // usable but actual value of resistance is to be determined
                break;
            case EffectType.PASS_ARMOR: // usable
                break;
            case EffectType.IMMUNITY_TURN:
                break;
            case EffectType.INVISIBLE:
                break;
        }

        turnDuration--;
    }
}

public enum EffectType
{
    DOT = 0,
    HOT,
    DEBUFF_ATTACK,
    DEBUFF_ARMOR,
    STUN,
    SLEEP,
    ANTI_HEAL,
    BUFF_ATTACK,
    BUFF_ARMOR,
    RESISTANCE,
    PASS_ARMOR,
    IMMUNITY_TURN,
    INVISIBLE
}