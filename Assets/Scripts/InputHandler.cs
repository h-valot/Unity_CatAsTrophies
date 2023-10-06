using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public static InputHandler Instance;
    
    public Vector3 touchPos;
    public bool isTouching;

    private Camera cam;

    private void Awake()
    {
        Instance = this;
    }

    public void Initialize()
    {
        cam = Camera.main;
    }

    private void Update()
    {
#if UNITY_ANDROID || UNITY_EDITOR
        if(Input.touchCount > 0 ||
           Input.GetMouseButton(0))
        {
            isTouching = true;
            touchPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        }
        else
        {
            isTouching = false;
            touchPos = Vector3.zero;
        }
#endif
    }

    public Vector3 GetInputWorldPos()
    {
        touchPos.z = cam.WorldToScreenPoint(transform.position).z;
        return cam.ScreenToWorldPoint(touchPos);
    }
}