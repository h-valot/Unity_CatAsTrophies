using System.Collections.Generic;
using UnityEngine;

public class CatGenerator : MonoBehaviour
{
    public static CatGenerator Instance;
    
    [Header("DEBUGGING")]
    public List<Cat> cats;
    
    public int totalCatCount;
    private Cat spawnedCat;
    private GameObject spawnedCatGO;

    private void Awake() => Instance = this;
    
    public void Initialize()
    {
        InstantiateCats();
    }

    private void InstantiateCats()
    {
        for (int i = 0; i < Registry.playerConfig.deckLenght; i++)
        {
            Cat newCat = Instantiate(Registry.entitiesConfig.cats[0].catBasePrefab, transform).GetComponent<Cat>();
            newCat.gameObject.SetActive(false);
            cats.Add(newCat);
            EntityManager.Instance.entities.Add(newCat);
            
            // add this new cat into the deck
            DeckManager.Instance.AddCat(newCat.id);
        }
    }
    
    /// <summary>
    /// Depool cat from the pool the instantiate them 
    /// </summary>
    /// <param name="_typeIndex">Type of the cat</param>
    /// <param name="_pos">Position of the cat</param>
    public string SpawnCatGraphics(int _typeIndex)
    {
        // get cat game object and place it
        spawnedCatGO = cats[totalCatCount].gameObject;
        spawnedCatGO.name = $"Cat_{totalCatCount}_{Registry.entitiesConfig.cats[_typeIndex].catName}";

        // setup the cat
        spawnedCat = spawnedCatGO.GetComponent<Cat>();
        spawnedCat.Initialize(_typeIndex);
        spawnedCat.ability = Registry.entitiesConfig.cats[_typeIndex].ability;
        spawnedCat.PutInHand();
        totalCatCount++;

        return spawnedCat.id;
    }
}