using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public string id;
    
    [Header("GAMEPLAY")]
    public float health, maxHealth;
    public List<Ability> autoAttacks;

    public virtual void Initialize()
    {
        id = Misc.GetRandomId();
    }
    
    
}