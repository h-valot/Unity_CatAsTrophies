using System.Collections.Generic;
using UnityEngine;

public class GraveyardManager : MonoBehaviour
{
    public static GraveyardManager Instance;

    [Header("DEBUGGING")] 
    public List<Cat> graveyard;

    private void Awake() => Instance = this;

    public void Initialize()
    {
        Instance.graveyard = new List<Cat>();
    }
    
    public void AddCat(Cat _cat)
    {
        Instance.graveyard.Add(_cat); 
    }
}