using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    [SerializeField] private SO_Inputs inputs;
    [System.NonSerialized] public HandManager handManager;

    private void Start()
    {
        if (inputs == null)
        {
            Debug.LogError("DRAG AND DROP: inputs is null.", this);
            Debug.Break();
        }
    }

    private void OnMouseDrag()
    {
        transform.position = inputs.touchPos;
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
            transform.position = handManager.GetNewCatPositionInHand();
        }
    }
}