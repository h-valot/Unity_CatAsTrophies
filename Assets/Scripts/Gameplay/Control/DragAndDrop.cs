using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    public Cat catDragged;
    
    private void OnMouseDown()
    {
        //exceptions
        if (!CanDrag()) return;
        
        HandManager.Instance.RemoveFromHand(catDragged.id);
    }

    private void OnMouseDrag()
    {
        //exceptions
        if (!CanDrag()) return;
        
        catDragged.transform.position = InputHandler.Instance.touchPos;
    }
 
    private void OnMouseUp()
    {
        //exceptions
        if (!CanDrag()) return;
        
        VerifyDistances();
    }

    /// <summary>
    /// Check if OnMouse() function can be used:
    /// (1) if the cat can move (is in the player's hand)
    /// (2) if the player can access inputs
    /// </summary>
    private bool CanDrag()
    {
        return catDragged.CanMove() && 
               InputHandler.Instance.CanAccessInput();
    }
    
    /// <summary>
    /// Snap the object on the nearest pawn position if close enough
    /// Otherwise, it's get back to the player's hand
    /// </summary>
    private void VerifyDistances()
    {
        BattlePawn closestPawn = BattlefieldManager.Instance.GetNearestPawnFromCursor(new Vector2(transform.position.x, transform.position.y));
        Vector2 transformPos = new Vector2(catDragged.transform.position.x, catDragged.transform.position.y);
        
        // snap to the closest battle pawn
        // else gets back into the player's hand
        if (BattlefieldManager.Instance.IsCloseEnough(transformPos, closestPawn.transform.position))
        {
            // if there is already a cat on that battle pawn,
            // put the former cat into the graveyard and place the new one
            if (closestPawn.entityIdLinked != "")
            {
                Misc.GetCatById(closestPawn.entityIdLinked).Withdraw();
            }
            closestPawn.Setup(catDragged.id);
            catDragged.transform.position = closestPawn.transform.position;
            catDragged.PlaceOnBattlefield();
        }
        else
        {
            catDragged.PutInHand();
            HandManager.Instance.AddToHand(catDragged.id);
        }
    }
}