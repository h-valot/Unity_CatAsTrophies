using UnityEngine;

public class Clickable : MonoBehaviour
{
    private void OnMouseDown()
    {
        DragManager.Instance.SetPawnDragged(this.gameObject);
    }
}