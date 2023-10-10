using System.Collections.Generic;
using UnityEngine;

public class CatGenerator : MonoBehaviour
{
    [SerializeField] public HandManager handManager;

    [Header("DEBUGGING")] 
    public Queue<Cat> cats;

    private void Start()
    {
        cats = new Queue<Cat>();
    }

    public void CreateNewCat(int catIndex, Vector3 position)
    {
        var newCat = Instantiate(
            Registry.cardConfig.cards[catIndex].prefab, 
            position, 
            Quaternion.identity, 
            transform);
        
        newCat.AddComponent<CatGraphics>();
        
        
    }
}