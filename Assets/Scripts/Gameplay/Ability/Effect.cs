public class Effect
{
    public EffectType type;

    public void Trigger()
    {
        switch (type)
        {
            case EffectType.Stun:
                break;
            case EffectType.Sleep:
                break;
        }
    }
}

public enum EffectType
{
    Stun = 0,
    Sleep
}