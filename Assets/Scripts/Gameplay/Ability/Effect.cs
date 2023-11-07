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
            case EffectType.Dot: //Usable
                Misc.GetEntityById(sourceId).UpdateHealth(-1);
                break;
            case EffectType.Hot: //Usable
                Misc.GetEntityById(sourceId).UpdateHealth(1);
                break;
            case EffectType.DebuffAttack: //Usable
                break;
            case EffectType.DebuffArmor:
                break;
            case EffectType.Stun: //Usable
                break;
            case EffectType.Sleep: //Usable
                break;
            case EffectType.AntiHeal:
                break;
            case EffectType.BuffAttack: //Usable
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