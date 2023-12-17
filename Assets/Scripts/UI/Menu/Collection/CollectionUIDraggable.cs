using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CollectionUIDraggable : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [Header("REFERENCES")] 
    public CollectionUIItem itemUI;
    public Image[] images;
    
    [HideInInspector] public Transform parentAfterDrag;
    [HideInInspector] public int siblingIndexAfterDrag;
    
    public void OnPointerDown(PointerEventData eventData)
    {
        // update graphics
        itemUI.onItemDragBegin?.Invoke(itemUI.isInDeck);
        itemUI.SetGraphicsCatAmount(1);
        
        // save grid content transform
        parentAfterDrag = itemUI.transform.parent;
        siblingIndexAfterDrag = itemUI.transform.GetSiblingIndex();
        
        // highlight item ui
        itemUI.transform.SetParent(itemUI.canvasParent.transform);
        itemUI.transform.SetAsLastSibling();
        
        // disable images raycasts
        foreach (var image in images)
            image.raycastTarget = false;
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        // dynamically update position
        itemUI.transform.position = Input.mousePosition;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // replace item ui to its original parent
        itemUI.transform.SetParent(parentAfterDrag);
        itemUI.transform.SetSiblingIndex(siblingIndexAfterDrag);
        
        // enable images raycasts
        foreach (var image in images)
            image.raycastTarget = true;
        
        // update graphics
        itemUI.UpdateGraphics();
        itemUI.onItemDragEnd?.Invoke(itemUI.isInDeck);
    }
}