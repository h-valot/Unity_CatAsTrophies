using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public string id;
    
    [Header("GAMEPLAY")]
    public float health, maxHealth;
    public List<Ability> autoAttacks;
    public List<Effect> effects;

    public virtual void Initialize()
    {
        id = Misc.GetRandomId();
    }

    public void UpdateHealth(int _value)
    {
        health -= _value;
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
    
    private void HandleDeath()
    {
        
    }
}