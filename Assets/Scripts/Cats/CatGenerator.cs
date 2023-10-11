using System.Collections.Generic;
using UnityEngine;

public class CatGenerator : MonoBehaviour
{
    [Header("DEBUGGING")] 
    public Queue<Cat> cats;

    private void Start()
    {
        cats = new Queue<Cat>();
    }

    public void CreateCatGraphics(int catIndex, Vector3 position)
    {
        var newCat = Instantiate(
            Registry.cardConfig.cards[catIndex].prefab,
            position,
            Quaternion.identity,
            transform);
    }
}