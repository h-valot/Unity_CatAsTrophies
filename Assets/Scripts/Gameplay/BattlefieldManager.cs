using UnityEngine;

public class BattlefieldManager : MonoBehaviour
{
    public static BattlefieldManager Instance;
    
    [Header("REFERENCES")]
    public BattlePawn[] catBattlePawns;
    public BattlePawn[] enemyBattlePawns;
    
    [Header("SETTINGS")]
    public float distanceThreshold = 1f;

    [Header("DEBUGGING")]
    public string[] catsOnBattlefield;
    public string[] enemiesOnBattlefield;
        
    public void Awake() => Instance = this;

    public void Initialize()
    {
        catsOnBattlefield = new string[3];
    }

    /// <summary>
    /// Make all cats on the battlefield auto attack there enemies
    /// </summary>
    public void UseAutoAttackOnEnemies()
    {
        foreach (string catId in catsOnBattlefield)
        {
            Misc.GetEntityById(catId).UseAutoAttack();
        }
    }

    /// <summary>
    /// Make all enemies on the battlefield auto attack cats
    /// </summary>
    public void UseAutoAttackOnCats()
    {
        foreach (string enemyId in enemiesOnBattlefield)
        {
            Misc.GetEntityById(enemyId).UseAutoAttack();
        }
    }

    public BattlePawn GetNearestPawnFromCursor(Vector2 originPos)
    {
        int closestPawnIndex = -1;
        float shortestDistance = 999;
        
        for (int i = 0; i < Instance.catBattlePawns.Length; i++)
        {
            // exceptions
            if (catBattlePawns[i].IsLocked()) continue;
            
            // getting the distance between both vectors
            Vector2 pawnPosition = new Vector2(Instance.catBattlePawns[i].transform.position.x, Instance.catBattlePawns[i].transform.position.y);
            float distanceBetweenPositions = Vector2.Distance(originPos, pawnPosition);

            if (distanceBetweenPositions < shortestDistance)
            {
                shortestDistance = distanceBetweenPositions;
                closestPawnIndex = i;
            }
        }

        return Instance.catBattlePawns[closestPawnIndex];
    }

    public bool IsCloseEnough(Vector2 u, Vector2 v)
    {
        return Vector2.Distance(u, v) <= Instance.distanceThreshold;
    }
}