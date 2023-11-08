using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
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
        for (int i = 0; i < Registry.playerConfig.deckEntities.Count; i++)
        {
            for (int j = 0; j < Registry.playerConfig.deckEntitiesCount[i]; j++)
            {
                CatGenerator.Instance.SpawnCatGraphics(Registry.entitiesConfig.cats.IndexOf(Registry.playerConfig.deckEntities[i]));
            }
        }
    }

    /// <summary>
    /// Add a new cat into the player's deck
    /// </summary>
    /// <param name="_catId">ID of the cat</param>
    public void AddCat(string _catId)
    {
        catsInDeck.Add(_catId);
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
        catsInDeck = catsInDeck.OrderBy(_catId => Guid.NewGuid()).ToList();
    }
}