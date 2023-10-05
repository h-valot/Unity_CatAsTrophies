using System.Collections.Generic;
using UnityEngine;

public class HandHandler : MonoBehaviour
{
    public static HandHandler Instance;

    public List<int> cardsInHand;
    public Transform rightLimit, leftLimit;
    
    private void Awake()
    {
        Instance = this;
    }

    public void AddCard(int newCard)
    {
        cardsInHand.Add(newCard);
        Instantiate(
            Registry.cardConfig.cards[newCard].prefab,
            new Vector3(leftLimit.position.x, leftLimit.position.y, leftLimit.position.z),
            Quaternion.identity,
            transform);
    }

    public void RemoveCard(int cardToRemove)
    {
        cardsInHand.Remove(cardToRemove);
    }
}