using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public static InputHandler Instance;
    
    [Header("REFERENCES")]
    [SerializeField] public Camera cam;
    [SerializeField] private LayerMask mask;

    [Header("DEBUGGING")]
    public Vector3 touchPos;

    private bool clickAlreadyCat = false;
    private bool clickAlreadyNotCat = false;

    private void Awake() => Instance = this;
    
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            var ray = cam.ScreenPointToRay(Input.mousePosition);

            // Raycast that only return cats, used to know if you clicked on a cat or not, to reset hand if you didn't click a cat in your hand
            if (Physics.Raycast(ray, out var hitInfoCat, 99, mask))
            {
                if (!clickAlreadyCat)
                {
                    if (hitInfoCat.transform.gameObject.GetComponentInParent<Cat>().state != CatState.InHand)
                    {
                        HandManager.Instance.ArrangeHand();
                    }
                    clickAlreadyCat = true;
                }
            }
            else
            {
                if (!clickAlreadyNotCat && !clickAlreadyCat)
                {
                    HandManager.Instance.ArrangeHand();
                    clickAlreadyNotCat = true;
                }
            }

            // Raycast that exclude cats, used to drag the cat on the drag plane in DragAndDrop.cs
            if (Physics.Raycast(ray, out var hitInfoNotCat, 99, ~mask))
            {
                touchPos = hitInfoNotCat.point;
            }
        }
        else
        {
            clickAlreadyCat = false;
            clickAlreadyNotCat = false;
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