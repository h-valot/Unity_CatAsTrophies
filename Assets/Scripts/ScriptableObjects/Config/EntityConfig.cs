using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

[CreateAssetMenu(fileName = "Entity", menuName = "Config/Entity", order = 1)]
public class EntityConfig : ScriptableObject
{
    [HideInInspector] public bool isCat;
    
    [Header("STATS")]
    public string entityName;
    public float health;
    
    [Header("ABILITY")]
    public Ability ability;
    public Ability[] autoAttack;

    [Header("GRAPHICS")]
    public GameObject basePrefab;
    public GameObject rightHandAddon;
    public GameObject leftHandAddon;
    public GameObject headAddon;
}