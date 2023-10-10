using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private SO_Inputs inputs;
    [SerializeField] private Camera cam;

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            var ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hitInfo))
            {
                inputs.touchPos = hitInfo.point;
            }
        }
    }
}