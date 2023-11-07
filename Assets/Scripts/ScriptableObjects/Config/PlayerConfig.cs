using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "Config/Player", order = 1)]
public class PlayerConfig : ScriptableObject
{
    public List<EntityConfig> deckEntities = new List<EntityConfig>();
    public List<int> deckEntitiesCount = new List<int>();
    
    public List<EntityConfig> collection;

    [Header("DEBUGGING")] 
    public int deckLenght;
}