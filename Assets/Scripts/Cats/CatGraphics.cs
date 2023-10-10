using UnityEngine;

public class CatGraphics : MonoBehaviour
{
    [SerializeField] private DragAndDrop dragAndDrop;

    public CatGraphics(HandManager handManager)
    {
        // exceptions
        if (dragAndDrop == null)
        {
            Debug.LogError("CAT: reference null. please verify links.", this);
            Debug.Break();
        }
        dragAndDrop.handManager = handManager;
    }
}