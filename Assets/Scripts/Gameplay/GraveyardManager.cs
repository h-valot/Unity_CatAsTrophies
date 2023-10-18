using System.Collections.Generic;
using UnityEngine;

public class GraveyardManager : MonoBehaviour
{
    public static GraveyardManager Instance;

    [Header("DEBUGGING")] 
    public List<Cat> catsInGraveyard;

    private void Awake() => Instance = this;

    public void Initialize()
    {
        Instance.catsInGraveyard = new List<Cat>();
    }
    
    public void AddCat(Cat _cat)
    {
        Instance.catsInGraveyard.Add(_cat); 
    }
}