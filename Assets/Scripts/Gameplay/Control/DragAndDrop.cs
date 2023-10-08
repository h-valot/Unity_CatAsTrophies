using System;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private void OnMouseDrag()
    {
        transform.position = InputHandler.Instance.GetInputWorldPos();
    }
    
    private void OnMouseUp()
    {
        Vector2 closestPawnPos = BattlePawnManager.Instance.GetNearestPawnFromCursor(new Vector2(transform.position.x, transform.position.y));
        Vector2 transformPos = new Vector2(transform.position.x, transform.position.y);
        
        if (BattlePawnManager.Instance.IsCloseEnough(transformPos, closestPawnPos))
        {
            transform.position = closestPawnPos;
        }
        else
        {
            transform.position = HandManager.Instance.handCenterPoint.position;
        }
    }
}