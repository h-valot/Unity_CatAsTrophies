using System;
using System.Collections.Generic;
using UnityEngine;

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
    public int armor = 0;
    public List<Ability> autoAttacks = new List<Ability>();
    public List<Effect> effects = new List<Effect>();

    [Header("GRAPHICS TWEAKING")] 
    public Vector3 battleRotation;
    public Vector3 baseRotation;
    public float battleScale;
    
    public Action OnHealthChange;
    public Action OnEffectAdded;
    public Action OnEffectRemoved;
    public Action OnBattlefieldEntered;
    
    public void Initialize()
    {
        id = Misc.GetRandomId();
    }

    /// <summary>
    /// Use auto attacks abilities
    /// </summary>
    public void UseAutoAttack()
    {
        foreach (var ability in autoAttacks)
        {
            // exceptions
            if (HasEffect(EffectType.Stun)) continue;
            if (HasEffect(EffectType.Sleep)) continue;

            ability.Use(this);
            break;
        }
    }
    
    public void UpdateHealth(int _value)
    {
        // apply resistance if
        // - has effect
        // - value update if inferior to 0 (we don't want to resistance the healing)
        if(HasEffect(EffectType.Resistance))
        {
            // multiply the damage value by the resistance modifier
            _value = Mathf.FloorToInt(_value * Registry.gameSettings.damageResistanceModifier);
        }

        _value += armor;
        if (_value > 0)
        {
            ArmorLoss(_value);
            _value = 0;
        }
        else
        {
            ArmorLoss(0);
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
        
        OnHealthChange?.Invoke();
    }

    public void ApplyEffect(EffectType _effectType, int _turnDuration)
    {
        // increment the effect if already exists
        foreach (Effect effect in effects)
        {
            if (effect.type == _effectType)
            {
                effect.turnDuration += _turnDuration;
                return;
            }
        }
        
        // else, create a new effect
        effects.Add(new Effect(_effectType, _turnDuration, id));
        
        OnEffectAdded?.Invoke();
    }
    
    protected void TriggerAllEffects()
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
        
        OnEffectRemoved?.Invoke();
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
        
        OnEffectRemoved?.Invoke();
    }

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
        // do nothing in the parent
    }
    
    public void UpdateArmor(int _value)
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
        armor = temporaryArmor;
        Debug.Log("Armor is now set to " + temporaryArmor);
        OnHealthChange?.Invoke();
    }

    public void ArmorLoss(int _value)
    {
        armor = _value;
        Debug.Log("Armor after the hit is " + armor);
        OnHealthChange?.Invoke();
    }

    public void HealUpdate(int _value)
    {
        int temporaryHeal = _value;
        if (HasEffect(EffectType.AntiHeal))
        {
            temporaryHeal = temporaryHeal - Registry.gameSettings.antiHealAmout;
        }
        health = health + temporaryHeal;

        if (health > maxHealth)
        {
            health = maxHealth;
        }
        
        OnHealthChange?.Invoke();
    }

    public void UpdateHealthNoArmor(int _value)
    {
        if (HasEffect(EffectType.Resistance))
        {
            // multiply the damage value by the resistance modifier
            _value = Mathf.FloorToInt(_value * Registry.gameSettings.damageResistanceModifier);
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
        
        OnHealthChange?.Invoke();
    }

    /// <summary>
    /// Update the entity position depending on the desired battle position
    /// </summary>
    /// <param name="_battlePosition">Desired position</param>
    public virtual void UpdateBattlePosition(BattlePosition _battlePosition)
    {
        battlePosition = _battlePosition;
    }
}

public enum BattlePosition
{
    Front = 0,
    Middle,
    Back
}