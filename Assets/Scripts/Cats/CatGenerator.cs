using System.Collections.Generic;
using UnityEngine;

public class CatGenerator : MonoBehaviour
{
    public static CatGenerator Instance;
    
    [Header("CAT POOLING")] 
    public int poolSize;
    
    [Header("DEBUGGING")]
    public List<Cat> activeCats;
    private Queue<Cat> pool;
    
    private int totalCatCount;
    private Cat spawnedCat;
    private GameObject spawnedCatGO;

    private void Awake() => Instance = this;
    
    
    public void Initialize()
    {
        pool = new Queue<Cat>();
        FillPool();
    }

    private void FillPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            Pool(Instantiate(Registry.catConfig.cats[0].catBasePrefab, transform).GetComponent<Cat>());
        }
    }

    private void Pool(Cat _toPool)
    {
        _toPool.gameObject.SetActive(false);
        pool.Enqueue(_toPool);
    }
    
    private Cat Depool()
    {
        if (pool.Count < 1) FillPool();

        Cat depooled = pool.Dequeue();
        depooled.gameObject.SetActive(true);
        return depooled;
    }
    
    public void SpawnCat(int _typeIndex, Vector3 _pos)
    {
        totalCatCount++;

        // get cat game object and place it
        spawnedCatGO = Depool().gameObject;
        spawnedCatGO.transform.position = _pos;
        spawnedCatGO.name = $"Cat_{totalCatCount}_{Registry.catConfig.cats[_typeIndex].catName}";

        // set the cat up
        spawnedCat = spawnedCatGO.GetComponent<Cat>();
        spawnedCat.Initialize(_typeIndex);

        activeCats.Add(spawnedCat);

        // activate the cat
        spawnedCatGO.SetActive(true);
        spawnedCat.state = BattleState.InHand;
    }
}