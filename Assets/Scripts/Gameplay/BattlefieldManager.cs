using System;
using System.Threading.Tasks;
using UnityEngine;

public class BattlefieldManager : MonoBehaviour
{
    public static BattlefieldManager Instance;
    
    public BattlePawn[] battlePawns;
    public float distanceThreshold = 1f;

    public string[] catsOnBattlefield;
        
    public void Awake() => Instance = this;

    public void Initialize()
    {
        catsOnBattlefield = new string[3];
    }

    /// <summary>
    /// Make all cats on the battlefield auto attack there enemies
    /// </summary>
    public void AutoAttackEnemies()
    {
        for (int i = 0; i < catsOnBattlefield.Length; i++)
        {
            Misc.GetCatById(catsOnBattlefield[i]).UseAutoAttack();
        }
    }

    public BattlePawn GetNearestPawnFromCursor(Vector2 originPos)
    {
        int closestPawnIndex = -1;
        float shortestDistance = 999;
        
        for (int i = 0; i < Instance.battlePawns.Length; i++)
        {
            // exceptions
            if (battlePawns[i].IsLocked()) continue;
            
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