using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EntitiesConfig", menuName = "Config/Entity/Entities", order = 2)]
public class EntitiesConfig : ScriptableObject
{
    [Header("TEAMS")]
    public List<CompositionConfig> compositions;
    
    [Header("ENTITIES")]
    public List<EntityConfig> cats;
    public List<EntityConfig> enemies;
}