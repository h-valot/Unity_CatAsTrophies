using System;
using UnityEngine;

public class BattlePawnManager : MonoBehaviour
{
    public static BattlePawnManager Instance;
    
    public BattlePawn[] battlePawns;
    public float distanceThreshold = 1f;

    public void Awake() => Instance = this;

    public void Initialize()
    {
        // do nothing
    }

    public BattlePawn GetNearestPawnFromCursor(Vector2 originPos)
    {
        int closestPawnIndex = -1;
        float shortestDistance = 999;
        
        for (int i = 0; i < Instance.battlePawns.Length; i++)
        {
            // getting the distance between both vectors
            Vector2 pawnPosition = new Vector2(Instance.battlePawns[i].transform.position.x, Instance.battlePawns[i].transform.position.y);
            float distanceBetweenPositions = Vector2.Distance(originPos, pawnPosition);

            if (distanceBetweenPositions < shortestDistance)
            {
                shortestDistance = distanceBetweenPositions;
                closestPawnIndex = i;
            }
        }

        return Instance.battlePawns[closestPawnIndex];
    }

    public bool IsCloseEnough(Vector2 u, Vector2 v)
    {
        return Vector2.Distance(u, v) <= Instance.distanceThreshold;
    }
}