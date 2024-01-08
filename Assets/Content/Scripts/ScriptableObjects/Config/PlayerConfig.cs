using System.Collections.Generic;
using Data.Player;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "Config/Player", order = 1)]
public class PlayerConfig : ScriptableObject
{
    public int deckMaxLengh = 10;
    public List<Item> deck = new List<Item>();
    
    [Header("DEBUGGING")] 
    public int deckLenght;
}