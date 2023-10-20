using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public static DeckManager Instance;

    [Header("DEBUGGING")] 
    public List<string> catsInDeck;

    private void Awake() => Instance = this;

    public void Initialize()
    {
        catsInDeck = new List<string>();
    }

    public void AddCat(string _catIndex)
    {
        catsInDeck.Add(_catIndex);
    }

    public string RemoveCat()
    {
        if (catsInDeck.Count <= 0)
        {
            MergeGraveyardIntoDeck();
        }
        
        string output = catsInDeck[0];
        catsInDeck.Remove(catsInDeck[0]);        
        return output;
    }

    public void ShuffleDeck()
    {
        catsInDeck = catsInDeck.OrderBy(s => Guid.NewGuid()).ToList();
    }

    public void MergeGraveyardIntoDeck()
    {
        for (int i = 0; i < GraveyardManager.Instance.catsInGraveyard.Count; i++)
        {
            catsInDeck.Add(GraveyardManager.Instance.catsInGraveyard[i]);
            GraveyardManager.Instance.catsInGraveyard.Remove(GraveyardManager.Instance.catsInGraveyard[i]);
        }
        ShuffleDeck();
    }
}