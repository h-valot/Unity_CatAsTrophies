using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Card", menuName = "ScriptableObjects/Card", order = 1)]
public class Card : ScriptableObject
{
    public string cardName;
    public float strengh;
    public float health;

    [FormerlySerializedAs("mesh")] public GameObject prefab;
}