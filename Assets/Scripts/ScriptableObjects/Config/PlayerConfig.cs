using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "Config/Player", order = 1)]
public class PlayerConfig : ScriptableObject
{
    public List<EntityConfig> deck;
    public List<EntityConfig> collection;

    [Header("DEBUGGING")] 
    public int deckLenght;
}