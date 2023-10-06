using System;
using UnityEngine;

public class BattlePawnManager : MonoBehaviour
{
    public static BattlePawnManager Instance;
    
    public GameObject[] pawnPositions;
    public float distanceThreshold = 1f;

    public void Awake()
    {
        Instance = this;
    }

    public void Initialize()
    {
        // do nothing
    }

    private void Update()
    {
        // debugging
        if (Input.GetKeyDown(KeyCode.E))
        {
            for (int i = 0; i < pawnPositions.Length; i++)
            {
                Debug.Log($"{pawnPositions[i].name} at x{pawnPositions[i].transform.position.x} y{pawnPositions[i].transform.position.y} z{pawnPositions[i].transform.position.z}");
            }
        }
    }

    public Vector2 GetNearestPawnFromCursor(Vector2 originPos)
    {
        
        int closestPawnIndex = 0;
        float shortestDistance = 999;
        Vector2 closestPawnPosition = Vector2.zero;
        
        for (int i = 0; i < pawnPositions.Length; i++)
        {
            Vector2 pawnPosition = new Vector2(pawnPositions[i].transform.position.x, pawnPositions[i].transform.position.y);
            float distanceBetweenPositions = Vector2.Distance(originPos, pawnPosition);

            if (distanceBetweenPositions < shortestDistance)
            {
                shortestDistance = distanceBetweenPositions;
                closestPawnPosition = pawnPosition;
                closestPawnIndex = i;
            }
        }

        Debug.Log($"BATTLEPAWN: " +
                  $"\r\nclosest pawn is {pawnPositions[closestPawnIndex].name} with {shortestDistance}u." +
                  $"\r\noriginal position x{originPos.x} y{originPos.y}\"");
        
        return closestPawnPosition;
    }

    public bool IsCloseEnough(Vector2 originPos, Vector2 destinationPos)
    {
        return Vector2.Distance(originPos, destinationPos) <= distanceThreshold;
    }
}