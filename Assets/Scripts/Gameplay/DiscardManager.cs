using System.Collections.Generic;
using UnityEngine;

public class DiscardManager : MonoBehaviour
{
    public static DiscardManager Instance;

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
            DeckManager.Instance.AddCat(catId);
        }
        catsDiscarded.Clear();
    }
}