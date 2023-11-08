using System.Collections.Generic;
using UnityEngine;

public class CatGenerator : MonoBehaviour
{
    public static CatGenerator Instance;
    
    [Header("DEBUGGING")]
    public List<Cat> cats;
    
    public int totalCatCount;
    private Cat spawnedCat;
    private GameObject newCatGO;

    private void Awake() => Instance = this;
    
    public void Initialize()
    {
        cats = new List<Cat>();
    }
    
    /// <summary>
    /// Link all cat type's specificities into the new instantiated cat.
    /// </summary>
    /// <param name="_typeIndex">Type of the cat</param>
    /// <param name="_pos">Position of the cat</param>
    public void SpawnCatGraphics(int _typeIndex)
    {
        Cat newCat = Instantiate(Registry.entitiesConfig.cats[0].basePrefab, transform).GetComponent<Cat>();
        
        // setup the cat
        newCat.Initialize(_typeIndex);
        newCat.ability = Registry.entitiesConfig.cats[_typeIndex].ability;
        newCat.autoAttacks = Registry.entitiesConfig.cats[_typeIndex].autoAttack;
        
        // register the entity in lists 
        cats.Add(newCat);
        EntityManager.Instance.entities.Add(newCat);
        DeckManager.Instance.AddCat(newCat.id);
        
        // name the cat's game object
        newCat.gameObject.name = $"Cat_{totalCatCount}_{Registry.entitiesConfig.cats[_typeIndex].entityName}";
        newCat.graphicsParent.SetActive(false);

        totalCatCount++;
    }
}