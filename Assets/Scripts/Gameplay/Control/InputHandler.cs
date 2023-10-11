using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public static InputHandler Instance;
    [SerializeField] private Camera cam;
    [SerializeField] private float maxCastDistance;
    [SerializeField] private LayerMask mask;
    
    [Header("DEBUGGING")]
    public Vector3 touchPos;

    private void Awake() => Instance = this;
    
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            var ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hitInfo, maxCastDistance, ~mask))
            {
                touchPos = hitInfo.point;
            }
        }
    }
}