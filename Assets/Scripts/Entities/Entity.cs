using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public string id;

    [Header("GAMEPLAY")] 
    public float health;
    public float maxHealth;
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
            ability.Use(this);
        }
    }
    
    public void UpdateHealth(int _value)
    {
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
}