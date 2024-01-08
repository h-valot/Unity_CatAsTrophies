using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public class Ability
{
    public AbilityAnimation animation;
    public Sprite intentionSprite;
    public List<Instruction> instructions = new List<Instruction>();
    
    private Entity source;

    public void Use(Entity _source)
    {
        // store last ability cast
        source = _source;

        Animate();
        
        foreach (var instruction in instructions)
        {
            // gather target of the instruction
            List<string> targetIds = GetTargets(instruction);
        
            // apply ability's effect on those targets
            foreach (string targetId in targetIds)
            {
                ApplyEffect(instruction, targetId);
            }
        }
    }

    /// <summary>
    /// Return the target ids of every instruction of the ability
    /// </summary>
    public List<string> GetInvolvedTargetId (Entity _source)
    {
        source = _source;

        List<string> involvedTargetIds = new List<string>();

        foreach (var instruction in instructions)
        {
            foreach (string targetId in GetTargets(instruction))
            {
                involvedTargetIds.Add(targetId);
            }
        }

        return involvedTargetIds;
    }
    
    private List<string> GetTargets(Instruction _instruction)
    {
        // initialization
        List<string> targets = new List<string>();
        
        // find out if the target of the ability is a cat or not
        bool isTargetingCats = 
            (Misc.IdManager.GetCatById(source.id) != null && _instruction.us) ||
            (Misc.IdManager.GetCatById(source.id) == null && !_instruction.us);
        
        // determine targets 
        switch (_instruction.target)
        {
            case TargetType.Self:
                // get the source's id
                targets.Add(source.id);
                break;
            case TargetType.All:
                if (isTargetingCats)
                {
                    foreach (var battlePawn in BattlefieldManager.Instance.catBattlePawns)
                    {
                        if (battlePawn.entityIdLinked != "")
                        {
                            targets.Add(battlePawn.entityIdLinked);
                        }
                    }
                }
                else
                {
                    foreach (var battlePawn in BattlefieldManager.Instance.enemyBattlePawns)
                    {
                        if (battlePawn.entityIdLinked != "")
                        {
                            targets.Add(battlePawn.entityIdLinked);
                        }
                    }
                }
                break;
            case TargetType.Front:
                if (isTargetingCats)
                {
                    for (int i = 0; i < BattlefieldManager.Instance.catBattlePawns.Length; i++)
                    {
                        if (BattlefieldManager.Instance.catBattlePawns[i].entityIdLinked != "")
                        {
                            targets.Add(BattlefieldManager.Instance.catBattlePawns[i].entityIdLinked);
                            break;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < BattlefieldManager.Instance.enemyBattlePawns.Length; i++)
                    {
                        if (BattlefieldManager.Instance.enemyBattlePawns[i].entityIdLinked != "")
                        {
                            targets.Add(BattlefieldManager.Instance.enemyBattlePawns[i].entityIdLinked);
                            break;
                        }
                    }
                }
                break;
            case TargetType.TwoFront:
                if (isTargetingCats)
                {
                    for (int i = 0; i < BattlefieldManager.Instance.catBattlePawns.Length; i++)
                    {
                        if (BattlefieldManager.Instance.catBattlePawns[i].entityIdLinked != "")
                        {
                            targets.Add(BattlefieldManager.Instance.catBattlePawns[i].entityIdLinked);
                            if (targets.Count == 2)
                            {
                                break;
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < BattlefieldManager.Instance.enemyBattlePawns.Length; i++)
                    {
                        if (BattlefieldManager.Instance.enemyBattlePawns[i].entityIdLinked != "")
                        {
                            targets.Add(BattlefieldManager.Instance.enemyBattlePawns[i].entityIdLinked);
                            if (targets.Count == 2)
                            {
                                break;
                            }
                        }
                    }
                }
                break;
            case TargetType.Back:
                if (isTargetingCats)
                {
                    for (int i = BattlefieldManager.Instance.catBattlePawns.Length - 1; i >= 0; i--)
                    {
                        if (BattlefieldManager.Instance.catBattlePawns[i].entityIdLinked != "")
                        {
                            targets.Add(BattlefieldManager.Instance.catBattlePawns[i].entityIdLinked);
                            break;
                        }
                    }
                }
                else
                {
                    for (int i = BattlefieldManager.Instance.enemyBattlePawns.Length - 1; i >= 0; i--)
                    {
                        if (BattlefieldManager.Instance.enemyBattlePawns[i].entityIdLinked != "")
                        {
                            targets.Add(BattlefieldManager.Instance.enemyBattlePawns[i].entityIdLinked);
                            break;
                        }
                    }
                }
                break;
            case TargetType.TwoBack:
                if (isTargetingCats)
                {
                    for (int i = BattlefieldManager.Instance.catBattlePawns.Length - 1; i >= 0; i--)
                    {
                        if (BattlefieldManager.Instance.catBattlePawns[i].entityIdLinked != "")
                        {
                            targets.Add(BattlefieldManager.Instance.catBattlePawns[i].entityIdLinked);
                            if (targets.Count == 2)
                            {
                                break;
                            }
                        }
                    }
                }
                else
                {
                    for (int i = BattlefieldManager.Instance.enemyBattlePawns.Length - 1; i >= 0; i--)
                    {
                        if (BattlefieldManager.Instance.enemyBattlePawns[i].entityIdLinked != "")
                        {
                            targets.Add(BattlefieldManager.Instance.enemyBattlePawns[i].entityIdLinked);
                            if (targets.Count == 2)
                            {
                                break;
                            }
                        }
                    }
                }
                break;
            case TargetType.Random:
                if (isTargetingCats)
                {
                    while (true)
                    {
                        int rndIndex = Random.Range(0, BattlefieldManager.Instance.catBattlePawns.Length);
                        if (BattlefieldManager.Instance.catBattlePawns[rndIndex].entityIdLinked != "")
                        {
                            targets.Add(BattlefieldManager.Instance.catBattlePawns[rndIndex].entityIdLinked);
                            break;
                        }
                    }
                }
                else
                {
                    while (true)
                    {
                        int rndIndex = Random.Range(0, BattlefieldManager.Instance.enemyBattlePawns.Length);
                        if (BattlefieldManager.Instance.enemyBattlePawns[rndIndex].entityIdLinked != "")
                        {
                            targets.Add(BattlefieldManager.Instance.enemyBattlePawns[rndIndex].entityIdLinked);
                            break;
                        }
                    }
                }
                break;
            case TargetType.MostHealth:
                float strongestEntityHealth = 0; 
                if (isTargetingCats)
                {
                    for (int i = BattlefieldManager.Instance.catBattlePawns.Length - 1; i >= 0; i--)
                    {
                        if (BattlefieldManager.Instance.catBattlePawns[i].entityIdLinked != "")
                        {
                            float currentEntityHealth = Misc.IdManager.GetEntityById(BattlefieldManager.Instance.catBattlePawns[i].entityIdLinked).health;
                            if (currentEntityHealth >= strongestEntityHealth)
                            {
                                strongestEntityHealth = currentEntityHealth;
                                targets.Clear();
                                targets.Add(BattlefieldManager.Instance.catBattlePawns[i].entityIdLinked);
                            }
                        }
                    }
                }
                else
                {
                    for (int i = BattlefieldManager.Instance.enemyBattlePawns.Length - 1; i >= 0; i--)
                    {
                        if (BattlefieldManager.Instance.enemyBattlePawns[i].entityIdLinked != "")
                        {
                            float currentEntityHealth = Misc.IdManager.GetEntityById(BattlefieldManager.Instance.enemyBattlePawns[i].entityIdLinked).health;
                            if (currentEntityHealth >= strongestEntityHealth)
                            {
                                strongestEntityHealth = currentEntityHealth;
                                targets.Clear();
                                targets.Add(BattlefieldManager.Instance.enemyBattlePawns[i].entityIdLinked);
                            }
                        }
                    }
                }
                break;
            case TargetType.LessHealth:
                float weakestEntityHealth = 999999; 
                if (isTargetingCats)
                {
                    for (int i = BattlefieldManager.Instance.catBattlePawns.Length - 1; i >= 0; i--)
                    {
                        if (BattlefieldManager.Instance.catBattlePawns[i].entityIdLinked != "")
                        {
                            float currentEntityHealth = Misc.IdManager.GetEntityById(BattlefieldManager.Instance.catBattlePawns[i].entityIdLinked).health;
                            if (currentEntityHealth <= weakestEntityHealth)
                            {
                                weakestEntityHealth = currentEntityHealth;
                                targets.Clear();
                                targets.Add(BattlefieldManager.Instance.catBattlePawns[i].entityIdLinked);
                            }
                        }
                    }
                }
                else
                {
                    for (int i = BattlefieldManager.Instance.enemyBattlePawns.Length - 1; i >= 0; i--)
                    {
                        if (BattlefieldManager.Instance.enemyBattlePawns[i].entityIdLinked != "")
                        {
                            float currentEntityHealth = Misc.IdManager.GetEntityById(BattlefieldManager.Instance.enemyBattlePawns[i].entityIdLinked).health;
                            if (currentEntityHealth <= weakestEntityHealth)
                            {
                                weakestEntityHealth = currentEntityHealth;
                                targets.Clear();
                                targets.Add(BattlefieldManager.Instance.enemyBattlePawns[i].entityIdLinked);
                            }
                        }
                    }
                }
                break;
            case TargetType.Extremities:
                if (isTargetingCats)
                {
                    // front
                    for (int i = 0; i < BattlefieldManager.Instance.catBattlePawns.Length; i++)
                    {
                        if (BattlefieldManager.Instance.catBattlePawns[i].entityIdLinked != "")
                        {
                            targets.Add(BattlefieldManager.Instance.catBattlePawns[i].entityIdLinked);
                            break;
                        }
                    }
                    // back
                    for (int i = BattlefieldManager.Instance.catBattlePawns.Length - 1; i >= 0; i--)
                    {
                        if (BattlefieldManager.Instance.catBattlePawns[i].entityIdLinked != "")
                        {
                            targets.Add(BattlefieldManager.Instance.catBattlePawns[i].entityIdLinked);
                            break;
                        }
                    }
                }
                else
                {
                    // front
                    for (int i = 0; i < BattlefieldManager.Instance.enemyBattlePawns.Length; i++)
                    {
                        if (BattlefieldManager.Instance.enemyBattlePawns[i].entityIdLinked != "")
                        {
                            targets.Add(BattlefieldManager.Instance.enemyBattlePawns[i].entityIdLinked);
                            break;
                        }
                    }
                    // back
                    for (int i = BattlefieldManager.Instance.enemyBattlePawns.Length - 1; i >= 0; i--)
                    {
                        if (BattlefieldManager.Instance.enemyBattlePawns[i].entityIdLinked != "")
                        {
                            targets.Add(BattlefieldManager.Instance.enemyBattlePawns[i].entityIdLinked);
                            break;
                        }
                    }
                }
                // remove duplicates from the list
                targets = targets.Distinct().ToList();
                break;
        }
        
        return targets;
    }

    private void ApplyEffect(Instruction _instruction, string _targetId)
    {
        switch (_instruction.type)
        {
            case InstructionType.Damage:
                // Damage target for X amount
                int temporaryAttack = _instruction.value;
                
                if (source.HasEffect(EffectType.DEBUFF_ATTACK))
                {
                    temporaryAttack -= Registry.gameSettings.debuffAttackAmout;
                }
                
                if (source.HasEffect(EffectType.BUFF_ATTACK))
                {
                    temporaryAttack += Registry.gameSettings.buffAttackAmout;
                }

                if (source.HasEffect(EffectType.PASS_ARMOR))
                {
                    Misc.IdManager.GetEntityById(_targetId).UpdateHealthPassArmor(-temporaryAttack);
                }
                else
                {
                    Misc.IdManager.GetEntityById(_targetId).UpdateHealth(-temporaryAttack);
                }
                break;

            case InstructionType.Dot:
                // Apply 1 damage per turn at the beginning of the turn 
                Misc.IdManager.GetEntityById(_targetId).ApplyEffect(EffectType.DOT, _instruction.value);
                break;
            
            case InstructionType.SelfExplosion:
                // isn't use for the moment
                Misc.IdManager.GetEntityById(_targetId).UpdateHealth(-_instruction.value);
                source.HandleDeath();
                break;
            
            case InstructionType.ToxicMagic:
                // isn't use for the moment
                break;
            
            case InstructionType.Hook:
                // TODO - hook enemy toward the caster by the value amount 
                break;
            
            case InstructionType.Repulse:
                // isn't use for the moment
                break;
            
            case InstructionType.Switch:
                // isn't use for the moment
                break;
            
            case InstructionType.Heal:
                // Heals for an X amount
                Misc.IdManager.GetEntityById(_targetId).HealUpdate(_instruction.value);
                break;
            
            case InstructionType.Hot:
                // Heals 1 damage per turn at the beginning
                Misc.IdManager.GetEntityById(_targetId).ApplyEffect(EffectType.HOT, _instruction.value);
                break;
            
            case InstructionType.LinkHealth:
                // isn't use for the moment
                break;
            
            case InstructionType.Res:
                // isn't use for the moment
                break;
            
            case InstructionType.DebuffAttack:
                // Debuff Damage done by 1
                Misc.IdManager.GetEntityById(_targetId).ApplyEffect(EffectType.DEBUFF_ATTACK, _instruction.value);
                break;
            
            case InstructionType.DebuffArmor:
                // Debuff armor gain by 1
                Misc.IdManager.GetEntityById(_targetId).ApplyEffect(EffectType.DEBUFF_ARMOR, _instruction.value);
                break;
            
            case InstructionType.Stun:
                // Forbids to an Entity to attack at the end of a turn
                Misc.IdManager.GetEntityById(_targetId).ApplyEffect(EffectType.STUN, _instruction.value);
                break;
            
            case InstructionType.Sleep:
                // Forbids to an Entity to attack at the end of a turn
                Misc.IdManager.GetEntityById(_targetId).ApplyEffect(EffectType.SLEEP, _instruction.value);
                break;
            
            case InstructionType.AntiHeal:
                // Reduce heals received by 1
                Misc.IdManager.GetEntityById(_targetId).ApplyEffect(EffectType.ANTI_HEAL, _instruction.value);
                break;
            
            case InstructionType.BuffAttack:
                // Buff damage done by 1
                Misc.IdManager.GetEntityById(_targetId).ApplyEffect(EffectType.BUFF_ATTACK, _instruction.value);
                break;
            
            case InstructionType.BuffArmor:
                // Buff armor gain by 1
                Misc.IdManager.GetEntityById(_targetId).ApplyEffect(EffectType.BUFF_ARMOR, _instruction.value);
                break;
            
            case InstructionType.Summon:
                // isn't use for the moment
                break;
            
            case InstructionType.BonusAction:
                // isn't use for the moment
                break;
            
            case InstructionType.Resistance:
                // isn't use for the moment
                Misc.IdManager.GetEntityById(_targetId).ApplyEffect(EffectType.RESISTANCE, _instruction.value);
                break;
            
            case InstructionType.PassArmor:
                // isn't use for the moment
                Misc.IdManager.GetEntityById(_targetId).ApplyEffect(EffectType.PASS_ARMOR, _instruction.value);
                break;
            
            case InstructionType.ImmunityTurn:
                // isn't use for the moment
                Misc.IdManager.GetEntityById(_targetId).ApplyEffect(EffectType.IMMUNITY_TURN, _instruction.value);
                break;
            
            case InstructionType.Invisible:
                // isn't use for the moment
                Misc.IdManager.GetEntityById(_targetId).ApplyEffect(EffectType.INVISIBLE, _instruction.value);
                break;
            
            case InstructionType.Repeat:
                // isn't use for the moment
                break;
            
            case InstructionType.Cleanse:
                // isn't use for the moment
                Misc.IdManager.GetEntityById(_targetId).ClearAllHarmfulEffects();
                break;
            
            case InstructionType.Clone:
                // isn't use for the moment
                break;

            case InstructionType.AddArmor:
                // add armor to the target
                Misc.IdManager.GetEntityById(_targetId).IncreaseArmor(_instruction.value);
                break;
        }
    }
    
    public void Animate()
    {
        Timer(Registry.gameSettings.abilityAnimationDuration);
        source.animator.SetBool("IsActing", true);
        switch (animation)
        {
            case AbilityAnimation.None:
                // do nothing
                break;
            case AbilityAnimation.Attacking:
                source.animator.SetTrigger("IsAttacking");
                Registry.events.AttackSound?.Invoke();
                break;
            case AbilityAnimation.Casting:
                source.animator.SetTrigger("IsCasting");
                Registry.events.BuffSound?.Invoke();
                break;
            case AbilityAnimation.CastingDebuff:
                source.animator.SetTrigger("IsCasting");
                Registry.events.DebuffSound?.Invoke();
                break;
            case AbilityAnimation.Heal:
                source.animator.SetTrigger("IsCasting");
                Registry.events.HealSound?.Invoke();
                break;
            case AbilityAnimation.Kicking:
                source.animator.SetTrigger("IsKicking");
                Registry.events.AttackSound?.Invoke();
                break;
        }
    }

    private async void Timer(float _timerToWait)
    {
        await Task.Delay((int)(_timerToWait * 1000));
        source.animator.SetBool("IsActing", false);
        if (source.HasEffect(EffectType.STUN) || source.HasEffect(EffectType.SLEEP))
        {
            source.animator.SetBool("IsSleeping", true);
        }
    }
}

public enum AbilityAnimation
{
    None = 0,
    Attacking,
    Casting,
    CastingDebuff,
    Heal,
    Kicking
}