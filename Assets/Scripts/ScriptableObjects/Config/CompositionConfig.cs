using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CompositionConfig", menuName = "Config/Composition", order = 1)]
public class CompositionConfig : ScriptableObject
{
    public List<EntityConfig> enemies;
}