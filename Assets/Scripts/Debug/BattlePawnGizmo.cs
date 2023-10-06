using UnityEngine;

public class BattlePawnGizmo : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, BattlePawnManager.Instance.distanceThreshold);
    }
}