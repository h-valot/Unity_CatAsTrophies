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
            HandleDeath(); 
        }
    }

    protected void TriggerAllEffects()
    {
        foreach (Effect effect in effects)
        {
            effect.Trigger();
        }
    }
    
    protected virtual void HandleDeath()
    {
        // do nothing in the parent
    }
}