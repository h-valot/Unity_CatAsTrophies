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
            case EffectType.DebuffArmor: //Usable
                break;
            case EffectType.Stun: //Usable
                break;
            case EffectType.Sleep: //Usable
                break;
            case EffectType.AntiHeal: //Usable
                break;
            case EffectType.BuffAttack: //Usable
                break;
            case EffectType.BuffArmor: //Usable
                break;
            case EffectType.Resistance: //Usable but actual value of resistance is to be determined
                break;
            case EffectType.PassArmor: //Usable
                break;
            case EffectType.ImmunityTurn:
                break;
            case EffectType.Invisible:
                break;
        }

        turnDuration--;
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