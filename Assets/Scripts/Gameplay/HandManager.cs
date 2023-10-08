using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    public static HandManager Instance;

    public List<int> cardsInHand;
    public Transform handCenterPoint;
    
    private void Awake()
    {
        Instance = this;
    }

    public void AddCard(int newCard)
    {
        cardsInHand.Add(newCard);
        
        Instantiate(
            Registry.cardConfig.cards[newCard].prefab,
            new Vector3(handCenterPoint.position.x, handCenterPoint.position.y, handCenterPoint.position.z),
            Quaternion.identity,
            transform);
    }

    public void RemoveCard(int cardToRemove)
    {
        cardsInHand.Remove(cardToRemove);
    }
}