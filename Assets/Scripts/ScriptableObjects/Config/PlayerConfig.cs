using System.Collections.Generic;
using Player;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "Config/Player", order = 1)]
public class PlayerConfig : ScriptableObject
{
    public List<Item> deck = new List<Item>();

    [Header("DEBUGGING")] 
    public int deckLenght;
}