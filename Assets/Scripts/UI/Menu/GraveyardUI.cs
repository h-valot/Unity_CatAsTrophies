using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GraveyardUI : MonoBehaviour
{
    public GameObject cardCatPrefab;
    public VerticalLayoutGroup verticalLayoutGroup;
    public List<CatCardDisplay> catCards;

    private void OnEnable()
    {
        UpdateAllDisplays();
    }

    private void InstantiateAllCats()
    {
        for (int i = 0; i < GraveyardManager.Instance.graveyard.Count; i++)
        {
            var newCard = Instantiate(cardCatPrefab, verticalLayoutGroup.transform)
                .GetComponent<CatCardDisplay>();
            
            newCard.UpdateDisplay(Registry.catConfig.cats[GraveyardManager.Instance.graveyard[i].typeIndex].catName,
                GraveyardManager.Instance.graveyard[i].health);
            
            catCards.Add(newCard);
        }
    }

    private void UpdateAllDisplays()
    {
        if (catCards.Count < GraveyardManager.Instance.graveyard.Count)
        {
            InstantiateAllCats();
            return;
        }
        
        for (int i = 0; i < catCards.Count; i++)
        {
            catCards[i].UpdateDisplay(
                Registry.catConfig.cats[GraveyardManager.Instance.graveyard[i].typeIndex].catName,
                GraveyardManager.Instance.graveyard[i].health);
        }
    }
}