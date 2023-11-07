using Mono.Reflection;

public class Effect
{
    public EffectType type;
    public int turnDuration;
    public bool isHarmful;
    public string sourceId;

    public Effect(EffectType _type, int _turnDuration, string _sourceId)
    {
        type = _type;
        turnDuration = _turnDuration;
        sourceId = _sourceId;
        
    }
    
    public void Trigger()
    {
        switch (type)
        {
            case EffectType.Dot:
                Misc.GetEntityById(sourceId).UpdateHealth(-1);
                break;
            case EffectType.Hot:
                Misc.GetEntityById(sourceId).UpdateHealth(1);
                break;
            case EffectType.DebuffAttack:
                break;
            case EffectType.DebuffArmor:
                break;
            case EffectType.Stun:
                break;
            case EffectType.Sleep:
                break;
            case EffectType.AntiHeal:
                break;
            case EffectType.BuffAttack:
                break;
            case EffectType.BuffArmor:
                break;
            case EffectType.Resistance:
                break;
            case EffectType.PassArmor:
                break;
            case EffectType.ImmunityTurn:
                break;
            case EffectType.Invisible:
                break;
        }

        turnDuration--;
        
        // if the effect expire, remove it
        if (turnDuration <= 0)
        {
            Misc.GetEntityById(sourceId).effects.Remove(this);
        }
    }
}

public enum EffectType
{
    Dot = 0,
    Hot,
    DebuffAttack,
    DebuffArmor,
    Stun,
    Sleep,
    AntiHeal,
    BuffAttack,
    BuffArmor,
    Resistance,
    PassArmor,
    ImmunityTurn,
    Invisible
}