using UnityEngine;

[System.Serializable]
public class Instruction
{
    public InstructionType type;
    public int value = 1;
    public TargetType target;
    public bool us;
}

public enum InstructionType
{
    Damage = 0,
    Dot,
    SelfExplosion,
    ToxicMagic,
    Hook,
    Repulse,
    Switch,
    Heal,
    Hot,
    LinkHealth,
    Res,
    DebuffAttack,
    DebuffArmor,
    Stun,
    Sleep,
    AntiHeal,
    BuffAttack,
    BuffArmor,
    Summon,
    BonusAction,
    Resistance,
    PassArmor,
    ImmunityTurn,
    Invisible,
    Repeat,
    Cleanse,
    Clone
}

public enum TargetType
{
    Self = 0,
    All,
    Front,
    TwoFront,
    Back,
    TwoBack,
    Random,
    MostHealth,
    LessHealth,
    Extremities
}