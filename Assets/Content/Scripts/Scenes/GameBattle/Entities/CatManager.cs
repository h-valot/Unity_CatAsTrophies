using System.Collections.Generic;
using Data.Player;
using UnityEngine;

public class CatManager : MonoBehaviour
{
    public static CatManager Instance;

    [Header("EXTERNAL REFERENCES")] 
    public EntitiesConfig entitiesConfig;

    [Header("REFERENCES")]
    public EntityManager entityManager;
    public DeckManager deckManager;
    
    [Header("DEBUGGING")]
    public List<Cat> cats;
    public int totalCatCount;
    public int deadCatAmount;

    public bool allCatsDead => deadCatAmount == totalCatCount;
    
    private Cat _spawnedCat;
    private GameObject _newCatGO;

    private void Awake() => Instance = this;
    
    public void Initialize()
    {
        cats = new List<Cat>();
    }
    
    /// <summary>
    /// Link all cat type's specificities into the new instantiated cat.
    /// </summary>
    /// <param name="catData">Type of the cat</param>
    public void SpawnCatGraphics(CatData catData)
    {
        Cat newCat = Instantiate(entitiesConfig.cats[0].basePrefab, transform).GetComponent<Cat>();
        
        // setup the cat
        newCat.Initialize(catData.entityIndex, catData.health);
        newCat.ability = entitiesConfig.cats[catData.entityIndex].ability;
        newCat.autoAttacks = entitiesConfig.cats[catData.entityIndex].autoAttack;
        
        // register the entity in lists 
        cats.Add(newCat);
        entityManager.entities.Add(newCat);
        deckManager.AddCat(newCat.id);
        
        // name the cat's game object
        newCat.gameObject.name = $"Cat_{totalCatCount}_{entitiesConfig.cats[catData.entityIndex].entityName}";
        newCat.graphicsParent.SetActive(false);

        totalCatCount++;
    }
}