using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiscardUI : MonoBehaviour
{
    public GameObject cardCatPrefab;
    public VerticalLayoutGroup verticalLayoutGroup;
    public List<CatCardDisplay> catCards;

    private void OnEnable()
    {
        InstantiateAllCats();
    }

    private void OnDisable()
    {
        DestroyAllCats();
    }

    private void InstantiateAllCats()
    {
        foreach (string catId in DiscardManager.Instance.catsDiscarded)
        {
            // instantiate a new display
            var newCard = Instantiate(cardCatPrefab, verticalLayoutGroup.transform).GetComponent<CatCardDisplay>();
            newCard.Initialize(catId);
            newCard.UpdateDisplay();
            catCards.Add(newCard);
        }
    }

    private void DestroyAllCats()
    {
        foreach (var catCard in catCards)
        {
            Destroy(catCard.gameObject);
        }
        catCards.Clear();
    }
}