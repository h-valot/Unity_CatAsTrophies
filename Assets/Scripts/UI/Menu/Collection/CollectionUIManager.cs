using System.Collections.Generic;
using System.Linq;
using Data;
using UnityEngine;

public class CollectionUIManager : MonoBehaviour
{
    [Header("REFERENCES")] 
    public GameObject graphicsParent;
    public CollectionUIItem itemPrefab;
    
    [Header("COLLECTION")]
    public Transform collectionContentTransform;
    public List<CollectionUIItem> collectionItems = new List<CollectionUIItem>();
    
    [Header("DECK")]
    public Transform deckContentTransform;
    public List<CollectionUIItem> deckItems = new List<CollectionUIItem>();
    
    public void Show()
    {
        GenerateItems();
        graphicsParent.SetActive(true);
    }

    public void Hide()
    {
        graphicsParent.SetActive(false);
        ClearItems();
    }

    private void GenerateItems()
    {
        foreach (var item in DataManager.data.playerStorage.collection)
        {
            var newItem = Instantiate(itemPrefab, collectionContentTransform);
            newItem.Initialize(item, false);
            newItem.UpdateGraphics();
            collectionItems.Add(newItem);
        }
        
        foreach (var item in DataManager.data.playerStorage.deck)
        {
            var newItem = Instantiate(itemPrefab, deckContentTransform);
            newItem.Initialize(item, true);
            newItem.UpdateGraphics();
            deckItems.Add(newItem);
        }
    }
    
    private void ClearItems()
    {
        int collectionItemCount = collectionItems.Count;
        for (var index = 0; index < collectionItemCount; index++)
        {
            Destroy(collectionItems[index].gameObject);
        }
        collectionItems.Clear();
        
        int deckItemCount = deckItems.Count;
        for (var index = 0; index < deckItemCount; index++)
        {
            Destroy(deckItems[index].gameObject);
        }
        deckItems.Clear();
    }
}