using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

[CreateAssetMenu(fileName = "EntityConfig", menuName = "Config/Entity/Entity", order = 1)]
public class EntityConfig : ScriptableObject
{
    [HideInInspector] public bool isCat;
    public string id;
    
    [Header("STATS")]
    public string entityName;
    public float health;
    public int armorAtStart;
    
    [Header("ABILITY")]
    public Ability ability = new Ability();
    public List<Ability> autoAttack = new List<Ability>();

    [Header("GRAPHICS")]
    public GameObject basePrefab;
    public Texture catSkinTexture;
    public Texture catEyesTexture;
    public GameObject rightHandAddon;
    public GameObject leftHandAddon;
    public GameObject headAddon;
}