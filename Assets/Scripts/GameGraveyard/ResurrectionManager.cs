using System;
using System.Collections.Generic;
using Data;
using Player;
using UnityEngine;

public class ResurrectionManager : MonoBehaviour
{
    [Header("REFERENCES")]
    public GameObject graphicsParent;
    public ResurrectionUICard resurrectionUICardPrefab;
    public Transform contentTransform;

    public int selectedCat;
    public Action<int> onCatSelected;

    public List<Vector2Int> candidates = new List<Vector2Int>();
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
        foreach (var item in DataManager.data.playerStorage.inGameDeck)
        {
            foreach (var cat in item.cats)
            {
                // continue, if the cat is alive
                if (!cat.isDead) continue;

                candidates.Add(new Vector2Int(item.entityIndex, item.cats.IndexOf(cat)));
            }
        }
    }

    private void InstantiateCards()
    {
        for (var index = 0; index < candidates.Count; index++)
        {
            var newCard = Instantiate(resurrectionUICardPrefab, contentTransform);
            newCard.Initialize(index, onCatSelected);
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
        DataManager.data.playerStorage.inGameDeck[candidates[selectedCat].x].cats[candidates[selectedCat].y].Reset();
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