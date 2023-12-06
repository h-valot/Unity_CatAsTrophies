using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public static InputHandler Instance;
    
    [Header("REFERENCES")]
    [SerializeField] public Camera cam;
    [SerializeField] private LayerMask mask;
    
    [Header("DEBUGGING")]
    public Vector3 touchPos;

    private bool mouseAlreadyDown = false;

    private void Awake() => Instance = this;
    
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            var ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hitInfo, 99, ~mask))
            {
                touchPos = hitInfo.point;
            }
            else
            {
                if (!mouseAlreadyDown)
                {
                    Registry.events.OnClickNotCat.Invoke();
                    mouseAlreadyDown = true;
                }
            }
        }
        else
        {
            mouseAlreadyDown = false;
        }
    }

    /// <summary>
    /// Check if inputs are likely to be used:
    /// (1) if all ui menus are closed 
    /// </summary>
    public bool CanAccessInput()
    {
        // if there is no ui menu opened
        return !MenuManager.Instance.IsMenuOpened();
    }
}