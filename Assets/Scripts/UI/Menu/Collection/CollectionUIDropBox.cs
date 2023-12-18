using Data;
using UnityEngine;
using UnityEngine.EventSystems;

public class CollectionUIDropBox : MonoBehaviour, IDropHandler
{
    [Header("REFERENCES")]
    public RSE_CollectionDragBox rseCollectionDragBox;
    
    [Header("SETTINGS")] 
    public bool isDeck;

    public void OnDrop(PointerEventData eventData)
    {
        // exit, if the dropped data isn't a collection ui draggable
        if (!eventData.pointerDrag.TryGetComponent<CollectionUIDraggable>(out var draggable)) return;

        // exit, if the currently draggable object shouldn't be
        if (!draggable.canDrag) return;
        
        // exit, if the player released a cat onto the deck panel that was already in the deck
        if (draggable.itemUI.isInDeck && isDeck) return;

        // exit, if the player released a cat onto the collection panel that was already in the collection
        if (!draggable.itemUI.isInDeck && !isDeck) return;

        // exit, if the player's deck is full of cats
        if (isDeck && DataManager.data.playerStorage.GetLenght(DataManager.data.playerStorage.deck) >= Registry.playerConfig.deckMaxLengh) return;

        // exit, if the player's deck has already reach the maximum capacity of different cats
        if (isDeck && DataManager.data.playerStorage.deck.Count == 7) return;
        
        // transfers cat depending on their origin
        if (draggable.itemUI.isInDeck)
        {
            DataManager.data.playerStorage.Transfer(
                DataManager.data.playerStorage.deck, 
                DataManager.data.playerStorage.collection, 
                draggable.entityIndexBeforeDrag);
        }
        else
        {
            DataManager.data.playerStorage.Transfer(
                DataManager.data.playerStorage.collection, 
                DataManager.data.playerStorage.deck, 
                draggable.entityIndexBeforeDrag);
        }
        
        rseCollectionDragBox.Call(draggable.itemUI.isInDeck, draggable.entityIndexBeforeDrag);
    }
}