using System.Collections.Generic;
using UnityEngine;

public class CatGenerator : MonoBehaviour
{
    public static CatGenerator Instance;
    
    [Header("CAT POOLING")] 
    public int poolSize;
    
    [Header("DEBUGGING")]
    public List<Cat> instantiatedCats;
    
    private Queue<Cat> pool;
    private int totalCatCount;
    private Cat spawnedCat;
    private GameObject spawnedCatGO;

    private void Awake() => Instance = this;
    
    
    public void Initialize()
    {
        Instance.pool = new Queue<Cat>();
        Instance.FillPool();
    }

    private void FillPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            Instance.Pool(Instantiate(Registry.catConfig.cats[0].catBasePrefab, Instance.transform).GetComponent<Cat>());
        }
    }

    public void Pool(Cat _toPool)
    {
        instantiatedCats.Remove(_toPool);
        Instance.pool.Enqueue(_toPool);
        _toPool.gameObject.SetActive(false);
    }
    
    private Cat Depool()
    {
        if (Instance.pool.Count < 1) Instance.FillPool();

        Cat depooled = Instance.pool.Dequeue();
        depooled.gameObject.SetActive(true);
        return depooled;
    }
    
    /// <summary>
    /// Depool cat from the pool the instantiate them 
    /// </summary>
    /// <param name="_typeIndex">Type of the cat</param>
    /// <param name="_pos">Position of the cat</param>
    public Cat SpawnCat(int _typeIndex, Vector3 _pos)
    {
        Instance.totalCatCount++;

        // get cat game object and place it
        Instance.spawnedCatGO = Instance.Depool().gameObject;
        Instance.spawnedCatGO.transform.position = _pos;
        Instance.spawnedCatGO.name = $"Cat_{Instance.totalCatCount}_{Registry.catConfig.cats[_typeIndex].catName}";

        // setup the cat
        Instance.spawnedCat = Instance.spawnedCatGO.GetComponent<Cat>();
        Instance.spawnedCat.Initialize(_typeIndex);
        Instance.instantiatedCats.Add(Instance.spawnedCat);
        Instance.spawnedCatGO.SetActive(true);

        return Instance.spawnedCat;
    }

    /// <summary>
    /// Get a reference to a cat thanks to its id
    /// </summary>
    public Cat GetCatById(string _id)
    {
        Cat output = new Cat();
        
        for (int i = 0; i < Instance.instantiatedCats.Count; i++)
        {
            if (Instance.instantiatedCats[i].id == _id)
            {
                output = Instance.instantiatedCats[i];
                break;
            }
        }
        
        return output;
    }
}