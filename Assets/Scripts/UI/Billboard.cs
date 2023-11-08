using UnityEngine;

public class Billboard : MonoBehaviour
{
    private void LateUpdate()
    {
        transform.LookAt(transform.position + InputHandler.Instance.cam.transform.forward);
    }
}