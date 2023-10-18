using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "Config/Player", order = 1)]
public class PlayerConfig : ScriptableObject
{
    public List<Cat> deck;
    public List<Cat> collection;
}