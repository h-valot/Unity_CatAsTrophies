using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class Entity : MonoBehaviour
{
    public string id;

    [Header("REFERENCES")]
    public GameObject graphicsParent;
    public Animator animator;
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public SpriteRenderer blobShadowRenderer;

    [Header("GAMEPLAY")] 
    public BattlePosition battlePosition;
    public float health;
    public float maxHealth;
    public int armor;
    public List<Ability> autoAttacks = new List<Ability>();
    public List<Effect> effects = new List<Effect>();

    [Header("GRAPHICS TWEAKING")] 
    public Vector3 battleRotation;
    public Vector3 baseRotation;
    public float battleScale;
    
    public Action OnStatsUpdate;
    public Action OnBattlefieldEntered;
    public Action<string, Color, bool> OnStatusRecieved; //text to display, color of the text, is an effect or not (change font size)

    private int selectedAutoAttack;
    protected bool stopAsync;

    public bool isInFrontOfBackgroundFade = false; //used by TurnManager.cs

    public void Initialize()
    {
        id = Misc.GetRandomId();
        stopAsync = false;
    }

    /// <summary>
    /// Select an auto attacks ability amoung the list of auto attacks abilities.
    /// </summary>
    public void SelectAutoAttack()
    {
        // exit if entity is stunned or slep
        if (!HasEffect(EffectType.Stun) && !HasEffect(EffectType.Sleep))
        {
            selectedAutoAttack = UnityEngine.Random.Range(0, autoAttacks.Count - 1);
        } 
    }

    /// <summary>
    /// return all the target ids of every instruction of the selected ability.
    /// </summary>
    public List<string> GetAutoAttackTarget()
    {
        // exit if entity is stunned or slep
        if (!HasEffect(EffectType.Stun) && !HasEffect(EffectType.Sleep))
        {
            return autoAttacks[selectedAutoAttack].GetInvolvedTargetId(this);
        }
        else
        {
            return new List<string>();
        }
    }

    /// <summary>
    /// Use the auto attacks ability previously selected.
    /// Cannot be used if the entity is stunned or slep
    /// </summary>
    public virtual void UseAutoAttack()
    {
        // exit if entity is stunned or slep
        if (HasEffect(EffectType.Stun)) return;
        if (HasEffect(EffectType.Sleep)) return;

        // exif if entity hasn't auto attacks
        if (autoAttacks.Count == 0) return;
        
        // use a random ability
        autoAttacks[selectedAutoAttack].Use(this);
    }
    
    public void UpdateHealth(int _value)
    {
        //Trigger function in scrollingFeedback.cs attached to each pawn to create a feedback text
        OnStatusRecieved?.Invoke(_value.ToString().TrimStart('-'), Registry.gameSettings.colorTextDamage, false);

        if (!HasEffect(EffectType.Stun) && !HasEffect(EffectType.Sleep))
        {
            animator.SetTrigger("IsTakingDamage");
        }

        TimerToResetToIdleFighting(Registry.gameSettings.abilityAnimationDuration);

        // apply resistance if
        // - has effect
        if (HasEffect(EffectType.Resistance))
        {
            // multiply the damage value by the resistance modifier
            _value = Mathf.FloorToInt(_value * Registry.gameSettings.damageResistanceModifier);
        }

        _value += armor;
        if (_value > 0)
        {
            DecreaseArmorTo(_value);
            _value = 0;
        }
        else
        {
            DecreaseArmorTo(0);
        }
        health += _value;

        if (health <= 0)
        {
            health = 0;
            HandleDeath(); 
        }

        if (health > maxHealth)
        {
            health = maxHealth;
        }
        
        //trigger update display function in EntityUIDisplay.cs
        OnStatsUpdate?.Invoke();
    }

    /// <summary>
    /// Apply the desired effect to this entity
    /// </summary>
    /// <param name="_turnDuration">Number of turn this effect will last</param>
    public void ApplyEffect(EffectType _effectType, int _turnDuration)
    {
        //Trigger function in scrollingFeedback.cs attached to each pawn to create a feedback text
        OnStatusRecieved?.Invoke(_effectType.ToString(), Registry.gameSettings.colorTextEffect, true);

        // increment the effect if already exists
        foreach (Effect effect in effects)
        {
            if (effect.type == _effectType)
            {
                effect.turnDuration += _turnDuration;
                OnStatsUpdate?.Invoke();
                return;
            }
        }
        
        // else, create a new effect
        effects.Add(new Effect(_effectType, _turnDuration, id));

        //trigger update display function in EntityUIDisplay.cs
        OnStatsUpdate?.Invoke();
    }
    
    /// <summary>
    /// Trigger all effects
    /// </summary>
    protected virtual void TriggerAllEffects()
    {
        List<Effect> effectsToRemove = new List<Effect>();
        foreach (var effect in effects)
        {
            effect.Trigger();
            
            // if the effect expire, add it to a list of all effects to remove
            if (effect.turnDuration <= 0)
            {
                effectsToRemove.Add(effect);
            }
        }

        // removes all expired effects
        foreach (var effect in effectsToRemove)
        {
            effects.Remove(effect);
        }


        if (!HasEffect(EffectType.Stun) && !HasEffect(EffectType.Sleep))
        {
            Debug.Log($"{this}: Set bool is Sleeping to false.");
            animator.SetBool("IsSleeping", false);
        }

        //trigger update display function in EntityUIDisplay.cs
        OnStatsUpdate?.Invoke();
    }
    
    public void ClearAllHarmfulEffects()
    {
        List<Effect> effectsToRemove = new List<Effect>();
        
        foreach (Effect effect in effects)
        {
            if (effect.isHarmful)
            {
                effectsToRemove.Add(effect);
            }
        }

        foreach (Effect effect in effectsToRemove)
        {
            effects.Remove(effect);
        }

        //trigger update display function in EntityUIDisplay.cs
        OnStatsUpdate?.Invoke();
    }

    /// <summary>
    /// Browse all effects to check if this entity has the requested effect
    /// </summary>
    public bool HasEffect(EffectType _effectType)
    {
        bool output = false;
        foreach (var effect in effects)
        {
            if (effect.type == _effectType)
            {
                output = true;
                break;
            }
        }
        return output;
    }
    
    public virtual void HandleDeath()
    {
        stopAsync = true;
        // do nothing in the parent
    }
    
    public void IncreaseArmor(int _value)
    {
        int temporaryArmor = _value;
        if (HasEffect(EffectType.BuffArmor))
        {
            temporaryArmor = temporaryArmor + Registry.gameSettings.buffArmorAmount;
        }
        if (HasEffect(EffectType.DebuffArmor))
        {
            temporaryArmor = temporaryArmor - Registry.gameSettings.debuffArmorAmout;
        }

        //Trigger function in scrollingFeedback.cs attached to each pawn to create a feedback text
        OnStatusRecieved?.Invoke(_value.ToString(), Registry.gameSettings.colorTextArmor, false);

        armor += temporaryArmor;

        //trigger update display function in EntityUIDisplay.cs
        OnStatsUpdate?.Invoke();
    }

    // function used at the start of every entity's turn 
    public void ResetArmor()
    {
        armor = 0;
    }

    public void DecreaseArmorTo(int _value)
    {
        armor = _value;

        //trigger update display function in EntityUIDisplay.cs
        OnStatsUpdate?.Invoke();
    }

    public void HealUpdate(int _value)
    {
        int temporaryHeal = _value;
        if (HasEffect(EffectType.AntiHeal))
        {
            temporaryHeal = temporaryHeal - Registry.gameSettings.antiHealAmout;
        }

        //Trigger function in scrollingFeedback.cs attached to each pawn to create a feedback text
        OnStatusRecieved?.Invoke(_value.ToString(), Registry.gameSettings.colorTextHeal, false);

        health = health + temporaryHeal;

        if (health > maxHealth)
        {
            health = maxHealth;
        }

        //trigger update display function in EntityUIDisplay.cs
        OnStatsUpdate?.Invoke();
    }

    public void UpdateHealthPassArmor(int _value)
    {
        if (HasEffect(EffectType.Resistance))
        {
            // multiply the damage value by the resistance modifier
            _value = Mathf.FloorToInt(_value * Registry.gameSettings.damageResistanceModifier);
        }

        //Trigger function in scrollingFeedback.cs attached to each pawn to create a feedback text
        OnStatusRecieved?.Invoke(_value.ToString().TrimStart('-'), Registry.gameSettings.colorTextDamage, false);

        health += _value;

        if (health <= 0)
        {
            health = 0;
            HandleDeath();
        }

        if (health > maxHealth)
        {
            health = maxHealth;
        }

        //trigger update display function in EntityUIDisplay.cs
        OnStatsUpdate?.Invoke();
    }

    /// <summary>
    /// Update the entity position depending on the desired battle position
    /// </summary>
    /// <param name="_battlePosition">Desired position</param>
    public virtual void UpdateBattlePosition(BattlePosition _battlePosition)
    {
        battlePosition = _battlePosition;
    }

    private async void TimerToResetToIdleFighting(float _timerToWait)
    {
        await Task.Delay((int)(_timerToWait * 1000));
        if (!stopAsync)
        {
            animator.SetTrigger("IsFighting");
        }
    }
}

public enum BattlePosition
{
    Front = 0,
    Middle,
    Back
}