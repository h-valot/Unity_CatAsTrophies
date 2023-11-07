using Mono.Reflection;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class Entity : MonoBehaviour
{
    public string id;

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
        _value += armor;
        if (_value > 0)
        {
            UpdateArmor(_value);
            _value = 0;
        }
        else
        {
            UpdateArmor(0);
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
        foreach (Effect effect in effects)
        {
            effect.Trigger();
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
}