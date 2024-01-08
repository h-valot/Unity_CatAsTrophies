using System.Collections.Generic;
using UnityEngine;

public class DiscardManager : MonoBehaviour
{
    public static DiscardManager Instance;

    [Header("REFERENCES")] 
    public DeckManager deckManager;
    
    [Header("DEBUGGING")] 
    public List<string> catsDiscarded;

    private void Awake() => Instance = this;

    public void Initialize()
    {
        Instance.catsDiscarded = new List<string>();
    }
    
    public void AddCat(string _cat)
    {
        Instance.catsDiscarded.Add(_cat); 
    }
    
    public void MergeDiscardIntoDeck()
    {
        foreach (string catId in catsDiscarded)
        {
            deckManager.AddCat(catId);
        }
        catsDiscarded.Clear();
    }
}