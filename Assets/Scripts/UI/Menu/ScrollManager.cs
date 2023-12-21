using UnityEngine;

public class ScrollManager : MonoBehaviour
{
    public CanvasRenderer firstElement;
    public CanvasRenderer lastElement;
    public GameObject leftArrow;
    public GameObject rightArrow;
    
    void Update()
    {
        leftArrow.SetActive(firstElement.cull);
        rightArrow.SetActive(lastElement.cull);
    }
}