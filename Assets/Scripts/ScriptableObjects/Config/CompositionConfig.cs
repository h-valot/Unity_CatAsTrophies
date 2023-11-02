using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CompositionConfig", menuName = "Config/Composition", order = 1)]
public class CompositionConfig : ScriptableObject
{
    public string id;

    public string compositionName;
    public List<EntityConfig> enemies = new List<EntityConfig>();

    public void Initialize()
    {
        for (int i = 0; i < 3; i++)
        {
            enemies.Add(new EntityConfig());
        }
    }
}