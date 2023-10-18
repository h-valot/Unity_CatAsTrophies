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
        foreach (Cat cat in GraveyardManager.Instance.catsInGraveyard)
        {
            catsInDeck.Add(cat.id);
            GraveyardManager.Instance.catsInGraveyard.Remove(cat);
        }
        ShuffleDeck();
    }
}