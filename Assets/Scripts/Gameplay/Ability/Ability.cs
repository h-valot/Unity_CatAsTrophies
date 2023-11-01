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
                break;
            
            case InstructionType.Dot:
                break;
            
            case InstructionType.SelfExplosion:
                break;
            
            case InstructionType.ToxicMagic:
                break;
            
            case InstructionType.Hook:
                break;
            
            case InstructionType.Repulse:
                break;
            
            case InstructionType.Switch:
                break;
            
            case InstructionType.Heal:
                break;
            
            case InstructionType.Hot:
                break;
            
            case InstructionType.LinkHealth:
                break;
            
            case InstructionType.Res:
                break;
            
            case InstructionType.DebuffAttack:
                break;
            
            case InstructionType.DebuffArmor:
                break;
            
            case InstructionType.Stun:
                break;
            
            case InstructionType.Sleep:
                break;
            
            case InstructionType.AntiHeal:
                break;
            
            case InstructionType.BuffAttack:
                break;
            
            case InstructionType.BuffArmor:
                break;
            
            case InstructionType.Summon:
                break;
            
            case InstructionType.BonusAction:
                break;
            
            case InstructionType.Resistance:
                break;
            
            case InstructionType.PassArmor:
                break;
            
            case InstructionType.ImmunityTurn:
                break;
            
            case InstructionType.Invisible:
                break;
            
            case InstructionType.Repeat:
                break;
            
            case InstructionType.Cleanse:
                break;
            
            case InstructionType.Clone:
                break;
        }
    }
}
