using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public static InputHandler Instance;
    
    public bool isTouching;

    private Camera cam;
    private Vector2 fingerPosition;

    private void Awake()
    {
        Instance = this;
    }

    public void Initialize()
    {
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
#if UNITY_ANDROID || UNITY_EDITOR
        if(Input.touchCount > 0 ||
           Input.GetMouseButton(0))
        {
            isTouching = true;
            fingerPosition = Input.GetTouch(0).position;
        }
        else
        {
            isTouching = false;
        }
#endif
    }

    public Vector3 GetMouseWorldPosition()
    {
        return cam.ScreenToWorldPoint(fingerPosition);
    }
}