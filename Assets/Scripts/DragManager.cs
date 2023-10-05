using UnityEngine;

public class DragManager : MonoBehaviour
{
    public static DragManager Instance;

    public GameObject pawnDragged;
    
    private void Awake()
    {
        Instance = this;
    }

    public void Initialize()
    {
        pawnDragged = null;
    }
    
    private void Update()
    {
        if (pawnDragged == null) return;
        
        if (InputHandler.Instance.isTouching)
        {
            pawnDragged.transform.position = new Vector2(InputHandler.Instance.GetMouseWorldPosition().x, InputHandler.Instance.GetMouseWorldPosition().y);
        }
        else
        {
            Vector2 nearestPawnFromCursor = BattlefieldPositionManager.Instance.GetNearestPawnFromCursor(InputHandler.Instance.GetMouseWorldPosition());
            if (nearestPawnFromCursor == Vector2.zero)
            {
                pawnDragged.transform.position = HandHandler.Instance.leftLimit.transform.position;
            }
            else
            {
                pawnDragged.transform.position = nearestPawnFromCursor;
            }
        }
    }

    public void SetPawnDragged(GameObject newPawn)
    {
        pawnDragged = newPawn;
    }
}