using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CollectionUIDraggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("REFERENCES")] 
    public CollectionUIItem itemUI;
    public Image[] images;
    
    [HideInInspector] public Transform parentAfterDrag;
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        itemUI.onItemDragBegin?.Invoke();
        itemUI.Drag();
        
        parentAfterDrag = itemUI.transform.parent;
        itemUI.transform.SetParent(itemUI.canvasParent.transform);
        itemUI.transform.SetAsLastSibling();
        
        foreach (var image in images)
            image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        itemUI.transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        itemUI.onItemDragEnd?.Invoke();
        itemUI.Undrag();
        
        itemUI.transform.SetParent(parentAfterDrag);
        
        foreach (var image in images)
            image.raycastTarget = true;
    }
}