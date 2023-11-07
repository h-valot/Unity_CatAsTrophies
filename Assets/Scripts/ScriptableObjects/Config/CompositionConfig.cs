using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CompositionConfig", menuName = "Config/Composition", order = 1)]
public class CompositionConfig : ScriptableObject
{
    public string id;
    public bool isPlayerDeck;

    public string compositionName;
    public List<EntityConfig> entities = new List<EntityConfig>();

    public void Initialize()
    {
        for (int i = 0; i < 3; i++)
        {
            entities.Add(new EntityConfig());
        }
    }
}