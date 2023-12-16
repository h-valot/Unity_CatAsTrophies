using System.Linq;
using Data;
using UnityEngine;
using UnityEngine.EventSystems;

public class CollectionUIDropBox : MonoBehaviour, IDropHandler
{
    [Header("REFERENCES")] 
    public Transform content;
    
    [Header("SETTINGS")] 
    public bool isDeck;
    
    public void OnDrop(PointerEventData eventData)
    {
        // exit, if the dropped data isn't a collection ui draggable
        if (!eventData.pointerDrag.TryGetComponent<CollectionUIDraggable>(out var draggable)) return;

        // exit, if the player released a cat onto the deck panel that was already in the deck
        if (draggable.itemUI.isInDeck && isDeck) return;

        // exit, if the player released a cat onto the collection panel that was already in the collection
        if (!draggable.itemUI.isInDeck && !isDeck) return;
        
        // exit, if there is no more cats to switch
        if (draggable.itemUI.item.cats.Count == 0)
        {
            draggable.itemUI.UpdateGraphics();
            return;
        }
        
        // transfers cat depending on their origin
        if (draggable.itemUI.isInDeck)
        {
            Debug.Log("COLLECTION UI DROP BOX: tranfers item from deck to collection");
            DataManager.data.playerStorage.Transfer(
                DataManager.data.playerStorage.deck, 
                DataManager.data.playerStorage.collection, 
                draggable.itemUI.item.entityIndex);
        }
        else
        {
            Debug.Log("COLLECTION UI DROP BOX: tranfers item from collection to deck");
            DataManager.data.playerStorage.Transfer(
                DataManager.data.playerStorage.collection, 
                DataManager.data.playerStorage.deck, 
                draggable.itemUI.item.entityIndex);
        }
        
        draggable.itemUI.UpdateGraphics();
    }
}