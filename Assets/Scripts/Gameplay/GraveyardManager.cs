using System.Collections.Generic;
using Unity.Android.Gradle.Manifest;
using UnityEngine;

public class GraveyardManager : MonoBehaviour
{
    public static GraveyardManager Instance;

    [Header("DEBUGGING")] 
    public List<string> catsInGraveyard;

    private void Awake() => Instance = this;

    public void Initialize()
    {
        Instance.catsInGraveyard = new List<string>();
    }
    
    public void AddCat(string _cat)
    {
        Instance.catsInGraveyard.Add(_cat); 
    }
    
    public void MergeGraveyardIntoDeck()
    {
        foreach (string catId in catsInGraveyard)
        {
            DeckManager.Instance.AddCat(catId);
        }
        catsInGraveyard.Clear();
    }
}