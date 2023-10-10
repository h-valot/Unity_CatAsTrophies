using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Card", menuName = "Config/Card", order = 1)]
public class Card_Config : ScriptableObject
{
    public string cardName;
    public float strengh;
    public float health;

    [FormerlySerializedAs("mesh")] public GameObject prefab;
}