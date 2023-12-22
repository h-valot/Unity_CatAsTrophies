using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Collection
{
    public class Draggable : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        [Header("REFERENCES")] 
        public Item itemUI;

        [HideInInspector] public bool canDrag = true;
        [HideInInspector] public int entityIndexBeforeDrag;
        private int _amountAfterDrag;
    
        public void OnPointerDown(PointerEventData eventData)
        {
            // updates informations
            canDrag = true;        
            entityIndexBeforeDrag = itemUI.item.entityIndex;
        
            _amountAfterDrag = itemUI.UpdateGraphicsAmount(-1);
            if (_amountAfterDrag < 0)
            {
                canDrag = false;
                return;
            }
        
            // background animation
            itemUI.onItemDragBegin?.Invoke(itemUI.isInDeck);
        
            // updates model graphics
            itemUI.itemModel.UpdateGraphics(itemUI.item);
            itemUI.itemModel.transform.position = Input.mousePosition;
            itemUI.itemModel.Show();
        }
    
        public void OnDrag(PointerEventData eventData)
        {
            if (!canDrag) return;
        
            // dynamically update position
            itemUI.itemModel.transform.position = Input.mousePosition;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            // updates model graphics
            itemUI.itemModel.Hide();
        
            // updates item ui graphics 
            itemUI.UpdateGraphicsAmount(0);
        
            itemUI.onItemDragEnd?.Invoke(itemUI.isInDeck);
        }
    }
}