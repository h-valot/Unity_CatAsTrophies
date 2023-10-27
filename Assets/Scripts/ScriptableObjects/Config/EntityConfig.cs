using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Entity", menuName = "Config/Entity", order = 1)]
public class EntityConfig : ScriptableObject
{
    [Header("STATS")]
    public string catName;
    public float health;
    
    [Header("ABILITY")]
    public Ability ability;
    public List<Ability> autoAttack;

    [Header("GRAPHICS")]
    public GameObject catBasePrefab;
    public GameObject rightHandAddon;
    public GameObject leftHandAddon;
    public GameObject headAddon;
}