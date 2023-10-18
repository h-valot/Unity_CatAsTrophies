using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    public Cat catDragged;
    
    private void OnMouseDown()
    {
        if (!catDragged.CanMove()) return;
        HandManager.Instance.RemoveFromHand(catDragged.id);
    }

    private void OnMouseDrag()
    {
        if (!catDragged.CanMove()) return;
        transform.position = InputHandler.Instance.touchPos;
    }
 
    private void OnMouseUp()
    {
        if (!catDragged.CanMove()) return;
        VerifyDistances();
    }

    /// <summary>
    /// Snap the object on the nearest pawn position if close enough
    /// Otherwise, it's get back to the player's hand
    /// </summary>
    private void VerifyDistances()
    {
        BattlePawn closestPawn = BattlePawnManager.Instance.GetNearestPawnFromCursor(new Vector2(transform.position.x, transform.position.y));
        Vector2 transformPos = new Vector2(transform.position.x, transform.position.y);
        
        // snap to the closest battle pawn
        // else gets back into the player's hand
        if (BattlePawnManager.Instance.IsCloseEnough(transformPos, closestPawn.transform.position))
        {
            // if there is already a cat on that battle pawn,
            // put the former cat into the graveyard and place the new one
            if (closestPawn.catIdLinked != "")
            {
                Misc.GetCatById(CatGenerator.Instance.instantiatedCats, closestPawn.catIdLinked).Replace();
            }
            closestPawn.Setup(catDragged.id);
            transform.position = closestPawn.transform.position;
            catDragged.state = CatState.OnBattle;
        }
        else
        {
            HandManager.Instance.AddToHand(catDragged.id);
            transform.position = HandManager.Instance.GetAvailablePosition();
            catDragged.state = CatState.InHand;
        }
    }
}