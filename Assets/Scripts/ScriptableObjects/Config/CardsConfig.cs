using UnityEngine;

[CreateAssetMenu(fileName = "CardsConfig", menuName = "Config/Cards", order = 1)]
public class CardsConfig : ScriptableObject
{
    public Card_Config[] cards;
}