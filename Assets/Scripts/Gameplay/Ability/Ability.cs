using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Ability 
{
    public List<Instruction> instructions = new List<Instruction>();
    
    private Entity source;

    public void Use(Entity _source)
    {
        // store last ability cast
        source = _source;
        Debug.Log($"ABILITY: use function entered with {source.name}");
        
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

    private List<string> GetTargets(Instruction _instruction)
    {
        // initialization
        List<string> targets = new List<string>();
        
        // find out if the target of the ability is a cat or not
        bool isTargetingCats = 
            (Misc.GetCatById(source.id) != null && _instruction.us) ||
            (Misc.GetCatById(source.id) == null && !_instruction.us);
        
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
                        if (battlePawn.entityIdLinked != "empty")
                        {
                            targets.Add(battlePawn.entityIdLinked);
                        }
                    }
                }
                else
                {
                    foreach (var battlePawn in BattlefieldManager.Instance.enemyBattlePawns)
                    {
                        if (battlePawn.entityIdLinked != "empty")
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
                        if (BattlefieldManager.Instance.catBattlePawns[i].entityIdLinked != "empty")
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
                        if (BattlefieldManager.Instance.enemyBattlePawns[i].entityIdLinked != "empty")
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
                        if (BattlefieldManager.Instance.catBattlePawns[i].entityIdLinked != "empty")
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
                        if (BattlefieldManager.Instance.enemyBattlePawns[i].entityIdLinked != "empty")
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
                        if (BattlefieldManager.Instance.catBattlePawns[i].entityIdLinked != "empty")
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
                        if (BattlefieldManager.Instance.enemyBattlePawns[i].entityIdLinked != "empty")
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
                        if (BattlefieldManager.Instance.catBattlePawns[i].entityIdLinked != "empty")
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
                        if (BattlefieldManager.Instance.enemyBattlePawns[i].entityIdLinked != "empty")
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
                        if (BattlefieldManager.Instance.catBattlePawns[rndIndex].entityIdLinked != "empty")
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
                        if (BattlefieldManager.Instance.enemyBattlePawns[rndIndex].entityIdLinked != "empty")
                        {
                            targets.Add(BattlefieldManager.Instance.enemyBattlePawns[rndIndex].entityIdLinked);
                            break;
                        }
                    }
                }
                break;
            case TargetType.MostHealth:
                float weakestEntityHealth = Mathf.Infinity; 
                if (isTargetingCats)
                {
                    for (int i = BattlefieldManager.Instance.catBattlePawns.Length - 1; i >= 0; i--)
                    {
                        if (BattlefieldManager.Instance.catBattlePawns[i].entityIdLinked != "empty")
                        {
                            float currentEntityhealth = Misc.GetEntityById(BattlefieldManager.Instance.catBattlePawns[i].entityIdLinked).health;
                            if (currentEntityhealth < weakestEntityHealth)
                            {
                                weakestEntityHealth = currentEntityhealth;
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
                        if (BattlefieldManager.Instance.enemyBattlePawns[i].entityIdLinked != "empty")
                        {
                            float currentEntityhealth = Misc.GetEntityById(BattlefieldManager.Instance.enemyBattlePawns[i].entityIdLinked).health;
                            if (currentEntityhealth < weakestEntityHealth)
                            {
                                weakestEntityHealth = currentEntityhealth;
                                targets.Clear();
                                targets.Add(BattlefieldManager.Instance.enemyBattlePawns[i].entityIdLinked);
                            }
                        }
                    }
                }
                break;
            case TargetType.LessHealth:
                float strongestEntityHealth = 0; 
                if (isTargetingCats)
                {
                    for (int i = BattlefieldManager.Instance.catBattlePawns.Length - 1; i >= 0; i--)
                    {
                        if (BattlefieldManager.Instance.catBattlePawns[i].entityIdLinked != "empty")
                        {
                            float currentEntityhealth = Misc.GetEntityById(BattlefieldManager.Instance.catBattlePawns[i].entityIdLinked).health;
                            if (currentEntityhealth > strongestEntityHealth)
                            {
                                strongestEntityHealth = currentEntityhealth;
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
                        if (BattlefieldManager.Instance.enemyBattlePawns[i].entityIdLinked != "empty")
                        {
                            float currentEntityhealth = Misc.GetEntityById(BattlefieldManager.Instance.enemyBattlePawns[i].entityIdLinked).health;
                            if (currentEntityhealth > strongestEntityHealth)
                            {
                                strongestEntityHealth = currentEntityhealth;
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
                        if (BattlefieldManager.Instance.catBattlePawns[i].entityIdLinked != "empty")
                        {
                            targets.Add(BattlefieldManager.Instance.catBattlePawns[i].entityIdLinked);
                            break;
                        }
                    }
                    // back
                    for (int i = BattlefieldManager.Instance.catBattlePawns.Length - 1; i >= 0; i--)
                    {
                        if (BattlefieldManager.Instance.catBattlePawns[i].entityIdLinked != "empty")
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
                        if (BattlefieldManager.Instance.enemyBattlePawns[i].entityIdLinked != "empty")
                        {
                            targets.Add(BattlefieldManager.Instance.enemyBattlePawns[i].entityIdLinked);
                            break;
                        }
                    }
                    // back
                    for (int i = BattlefieldManager.Instance.enemyBattlePawns.Length - 1; i >= 0; i--)
                    {
                        if (BattlefieldManager.Instance.enemyBattlePawns[i].entityIdLinked != "empty")
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

        // debugging
        if (targets.Count == 0)
        {
            Debug.Log("ABILITY MANAGER: there is no target for this ability");
        }

        foreach (string targetId in targets)
        {
            Debug.Log($"ABILITY MANAGER: {Misc.GetEntityById(targetId).id} is a target");
        }
        
        return targets;
    }

    private void ApplyEffect(Instruction _instruction, string _targetId)
    {
        switch (_instruction.type)
        {
            case InstructionType.Damage:
                int temporaryAttack = _instruction.value;
                if (source.HasEffect(EffectType.DebuffAttack))
                {
                    temporaryAttack= temporaryAttack - 1;
                }
                if (source.HasEffect(EffectType.BuffAttack))
                {
                    temporaryAttack = temporaryAttack + 1;
                }
                Misc.GetEntityById(_targetId).UpdateHealth(-temporaryAttack);
                break;

            case InstructionType.Dot:
                Misc.GetEntityById(_targetId).ApplyEffect(EffectType.Dot, _instruction.value);
                break;
            
            case InstructionType.SelfExplosion:
                // isn't use for the moment
                Misc.GetEntityById(_targetId).UpdateHealth(-_instruction.value);
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
                Misc.GetEntityById(_targetId).UpdateHealth(_instruction.value);
                break;
            
            case InstructionType.Hot:
                Misc.GetEntityById(_targetId).ApplyEffect(EffectType.Hot, _instruction.value);
                break;
            
            case InstructionType.LinkHealth:
                // isn't use for the moment
                break;
            
            case InstructionType.Res:
                // isn't use for the moment
                break;
            
            case InstructionType.DebuffAttack:
                // isn't use for the moment
                Misc.GetEntityById(_targetId).ApplyEffect(EffectType.DebuffAttack, _instruction.value);
                break;
            
            case InstructionType.DebuffArmor:
                // isn't use for the moment
                Misc.GetEntityById(_targetId).ApplyEffect(EffectType.DebuffArmor, _instruction.value);
                break;
            
            case InstructionType.Stun:
                Misc.GetEntityById(_targetId).ApplyEffect(EffectType.Stun, _instruction.value);
                break;
            
            case InstructionType.Sleep:
                Misc.GetEntityById(_targetId).ApplyEffect(EffectType.Sleep, _instruction.value);
                break;
            
            case InstructionType.AntiHeal:
                Misc.GetEntityById(_targetId).ApplyEffect(EffectType.AntiHeal, _instruction.value);
                break;
            
            case InstructionType.BuffAttack:
                // isn't use for the moment
                Misc.GetEntityById(_targetId).ApplyEffect(EffectType.BuffAttack, _instruction.value);
                break;
            
            case InstructionType.BuffArmor:
                // isn't use for the moment
                Misc.GetEntityById(_targetId).ApplyEffect(EffectType.BuffArmor, _instruction.value);
                break;
            
            case InstructionType.Summon:
                // isn't use for the moment
                break;
            
            case InstructionType.BonusAction:
                // isn't use for the moment
                break;
            
            case InstructionType.Resistance:
                // isn't use for the moment
                Misc.GetEntityById(_targetId).ApplyEffect(EffectType.Resistance, _instruction.value);
                break;
            
            case InstructionType.PassArmor:
                // isn't use for the moment
                Misc.GetEntityById(_targetId).ApplyEffect(EffectType.PassArmor, _instruction.value);
                break;
            
            case InstructionType.ImmunityTurn:
                // isn't use for the moment
                Misc.GetEntityById(_targetId).ApplyEffect(EffectType.ImmunityTurn, _instruction.value);
                break;
            
            case InstructionType.Invisible:
                // isn't use for the moment
                Misc.GetEntityById(_targetId).ApplyEffect(EffectType.Invisible, _instruction.value);
                break;
            
            case InstructionType.Repeat:
                // isn't use for the moment
                break;
            
            case InstructionType.Cleanse:
                // isn't use for the moment
                Misc.GetEntityById(_targetId).ClearAllHarmfulEffects();
                break;
            
            case InstructionType.Clone:
                // isn't use for the moment
                break;

            case InstructionType.AddArmor:
                // add armor to the target
                Misc.GetEntityById(_targetId).UpdateArmor(_instruction.value);
                break;
        }
    }
}
