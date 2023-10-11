using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    public float verticalOffset;

    private void OnMouseDrag()
    {
        transform.position = InputHandler.Instance.touchPos;
    }
 
    /// <summary>
    /// Snap the object on the nearest pawn position if close enough
    /// Otherwise, it's get back to the player's hand
    /// </summary>
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
            transform.position = HandManager.Instance.GetNewCatPositionInHand();
        }
    }
}