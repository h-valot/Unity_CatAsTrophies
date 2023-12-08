using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using Data;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public static DeckManager Instance;
    private void Awake() => Instance = this;

    [Header("DEBUGGING")] 
    public List<string> catsInDeck;

    /// <summary>
    /// Initialize the list of cats
    /// </summary>
    public void Initialize()
    {
        catsInDeck = new List<string>();
    }

    /// <summary>
    /// Fulfill the player's deck with cats stored in the tool "Player Deck"
    /// </summary>
    public void LoadPlayerDeck()
    {
        
        if (DataManager.data.collection.inGameDeck.Count == 0)
        {
            Debug.LogError("DECK MANAGER: the deck of the player is empty", this);
            return;
        }
        
        for (int i = 0; i < DataManager.data.collection.inGameDeck.Count; i++)
        {
            for (int j = 0; j < DataManager.data.collection.inGameDeck[i].count; j++)
            {
                CatManager.Instance.SpawnCatGraphics(Registry.entitiesConfig.cats.IndexOf(DataManager.data.collection.inGameDeck[i].entity));
            }
        }
    }

    /// <summary>
    /// Add a new cat into the player's deck
    /// </summary>
    /// <param name="catId">ID of the cat</param>
    public void AddCat(string catId)
    {
        catsInDeck.Add(catId);
    }

    /// <summary>
    /// Remove a cat from the deck.
    /// If there are no more cats in the deck, merge all discarded cats into the deck, then shuffle it.
    /// </summary>
    /// <returns>The cat's id of the cat that has been removed</returns>
    public string RemoveCat()
    {
        if (catsInDeck.Count <= 0)
        {
            DiscardManager.Instance.MergeDiscardIntoDeck();
            ShuffleDeck();
        }
        
        string output = catsInDeck[0];
        catsInDeck.Remove(catsInDeck[0]);
        return output;
    }

    /// <summary>
    /// Randomly shuffle the player's deck
    /// </summary>
    public void ShuffleDeck()
    {
        catsInDeck = catsInDeck.OrderBy(catId => Guid.NewGuid()).ToList();
    }
}