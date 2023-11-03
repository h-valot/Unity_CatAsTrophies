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