using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public string id;

    [Header("REFERENCES")]
    public GameObject graphicsParent;
    public Animator animator;
    
    [Header("GAMEPLAY")] 
    public float health;
    public float maxHealth;
    public int armor = 0;
    public List<Ability> autoAttacks = new List<Ability>();
    public List<Effect> effects = new List<Effect>();

    public void Initialize()
    {
        id = Misc.GetRandomId();
    }

    /// <summary>
    /// Use auto attacks abilities
    /// </summary>
    public void UseAutoAttack()
    {
        foreach (Ability ability in autoAttacks)
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
        if(HasEffect(EffectType.Resistance))
        {
            _value = _value / 2;
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
    }

    public virtual void HandleDeath()
    {
        // do nothing in the parent
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

    public void UpdateArmor(int _value)
    {
        int temporaryArmor = _value;
        if (HasEffect(EffectType.BuffArmor))
        {
            temporaryArmor = temporaryArmor + 1;
        }
        if (HasEffect(EffectType.DebuffArmor))
        {
            temporaryArmor = temporaryArmor - 1;
        }
        armor = temporaryArmor;
        Debug.Log("Armor is now set to " + temporaryArmor);
    }

    public void ArmorLoss(int _value)
    {
        armor = _value;
        Debug.Log("Armor after the hit is " + armor);
    }

    public void HealUpdate(int _value)
    {
        int temporaryHeal = _value;
        if (HasEffect(EffectType.AntiHeal))
        {
            temporaryHeal = temporaryHeal - 1;
        }
        health = health + temporaryHeal;

        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }

    public void UpdateHealthNoArmor(int _value)
    {
        if (HasEffect(EffectType.Resistance))
        {
            _value = _value / 2;
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
    }
}