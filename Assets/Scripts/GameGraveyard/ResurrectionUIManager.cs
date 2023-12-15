using System;
using System.Collections.Generic;
using Data;
using Player;
using UnityEngine;

public class ResurrectionUIManager : MonoBehaviour
{
    [Header("REFERENCES")]
    public GameObject graphicsParent;
    public ResurrectionUICard resurrectionUICardPrefab;
    public Transform contentTransform;

    public int selectedCat;
    public Action<int> onCatSelected;

    public List<int> candidates = new List<int>();
    public List<ResurrectionUICard> cards = new List<ResurrectionUICard>();
    
    public void Initialize()
    {
        Hide();
        onCatSelected += Select;
    }

    private void OnDisable()
    {
        onCatSelected -= Select;
    }

    private void GetCandidates()
    {
        for (var index = 0; index < DataManager.data.playerStorage.inGameDeck.Count; index++)
        {
            // continue, if the cat is alive
            if (!DataManager.data.playerStorage.inGameDeck[index].isDead) continue;

            candidates.Add(DataManager.data.playerStorage.inGameDeck[index].entityIndex);
        }
    }

    private void InstantiateCards()
    {
        for (var index = 0; index < candidates.Count; index++)
        {
            var newCard = Instantiate(resurrectionUICardPrefab, contentTransform);
            newCard.Initialize(index);
            cards.Add(newCard);
        }
    }

    private void ClearCards()
    {
        foreach (var card in cards)
        {
            Destroy(card.gameObject);
        }
        cards.Clear();
    }
    
    public void Select(int selectedIndex)
    {
        selectedCat = selectedIndex;
    }
    
    public void Resurrect()
    {
        DataManager.data.playerStorage.inGameDeck[candidates[selectedCat]].Reset();
    }

    public void Show()
    {
        GetCandidates();
        InstantiateCards();
        graphicsParent.SetActive(true);
    }
    
    public void Hide()
    {
        ClearCards();
        graphicsParent.SetActive(false);
    }
}