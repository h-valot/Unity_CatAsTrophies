using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public static DeckManager Instance;

    [Header("DEBUGGING")] 
    public List<string> deck;

    private void Awake() => Instance = this;

    public void Initialize()
    {
        Instance.deck = new List<string>();
    }

    public void AddCat(string _catIndex)
    {
        Instance.deck.Add(_catIndex);
    }
}