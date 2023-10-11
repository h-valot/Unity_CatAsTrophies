using System.Collections.Generic;
using UnityEngine;

public class CatGenerator : MonoBehaviour
{
    public static CatGenerator Instance;
    
    [Header("CAT POOLING")] 
    public int catPoolAmount;
    public Queue<GameObject>[] catPool;
    public List<Cat> activeCats;
    
    private GameObject newGameObject;
    private Cat newCat;
    
    private Cat spawnedCat;
    private GameObject spawnedCatGO;
    private Vector3 spawnedCatPos;
    private int value;
    private int totalCatCount;

    private void Awake() => Instance = this;
    
    public void CreateCatGraphics(int catIndex, Vector3 position)
    {
        var newCat = Instantiate(
            Registry.cardConfig.cards[catIndex].prefab,
            position,
            Quaternion.identity,
            transform);
    }

    public void Initialize()
    {
        // create cat pool and fill it
        catPool = new Queue<GameObject>[Registry.cardConfig.cards.Length];
        activeCats = new List<Cat>();
        InitializeCatPool();
    }

    private void InitializeCatPool()
    {
        for (int i = 0; i < Registry.cardConfig.cards.Length; i++)
        {
            catPool[i] = new Queue<GameObject>();
            FillCatPool(i);
        }
    }

    private void FillCatPool(int _index)
    {
        for (int j = 0; j < catPoolAmount; j++)
        {
            newGameObject = Instantiate(Registry.cardConfig.cards[_index].prefab, transform);
            newCat.typeIndex = _index;
            newGameObject.SetActive(false);
            catPool[_index].Enqueue(newGameObject);
        }
    }

    public void SpawnCat(int _typeIndex, Vector3 _pos)
    {
        totalCatCount++;

        // determine cat position
        spawnedCatPos = _pos;

        if (catPool[_typeIndex].Count <= 0)
        {
            FillCatPool(_typeIndex);
        }

        // get cat game object and place it
        spawnedCatGO = catPool[_typeIndex].Dequeue();
        spawnedCatGO.transform.position = spawnedCatPos;

        // set the chick up
        spawnedCat = spawnedCatGO.GetComponent<Cat>();
        spawnedCat.Initialize(_typeIndex);

        activeCats.Add(spawnedCat);

        spawnedCatGO.name = "Cat_" + totalCatCount + "_" + Registry.cardConfig.cards[_typeIndex].cardName;

        // activate the cat
        spawnedCatGO.SetActive(true);
        spawnedCat.state = BattleState.InHand;
    }

    public void RemoveAllChicks()
    {
        foreach(Cat cat in FindObjectsOfType<Cat>())
        {
            cat.Remove();
        }
    }
}