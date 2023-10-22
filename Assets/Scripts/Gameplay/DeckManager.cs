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
            GraveyardManager.Instance.MergeGraveyardIntoDeck();
            ShuffleDeck();
        }
        
        string output = catsInDeck[0];
        catsInDeck.Remove(catsInDeck[0]);
        return output;
    }

    private void ShuffleDeck()
    {
        catsInDeck = catsInDeck.OrderBy(_catId => Guid.NewGuid()).ToList();
    }
}