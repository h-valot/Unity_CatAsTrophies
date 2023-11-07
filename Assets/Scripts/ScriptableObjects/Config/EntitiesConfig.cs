using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EntitiesConfig", menuName = "Config/Entities", order = 1)]
public class EntitiesConfig : ScriptableObject
{
    [Header("TEAMS")]
    public List<CompositionConfig> compositions;
    
    [Header("ENTITIES")]
    public List<EntityConfig> cats;
    public List<EntityConfig> enemies;
}