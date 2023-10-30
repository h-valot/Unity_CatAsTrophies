using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EntitiesConfig", menuName = "Config/Entities", order = 1)]
public class EntitiesConfig : ScriptableObject
{
    public List<EntityConfig> cats;
    public List<EntityConfig> enemies;
}