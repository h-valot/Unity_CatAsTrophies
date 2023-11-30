using UnityEngine;

public class BattlefieldManager : MonoBehaviour
{
    public static BattlefieldManager Instance;
    
    [Header("REFERENCES")]
    public BattlePawn[] catBattlePawns;
    public BattlePawn[] enemyBattlePawns;
    
    [Header("SETTINGS")]
    public float distanceThreshold = 1f;

    public void Awake() => Instance = this;

    public void Initialize()
    {
        for (int i = 0; i < 3; i++)
        {
            catBattlePawns[i].battlePosition = (BattlePosition)i;
            enemyBattlePawns[i].battlePosition = (BattlePosition)i;
        }
    }

    public void RemoveFromBattlePawn(string _entityId)
    {
        if (Misc.GetEntityById(_entityId).TryGetComponent<Cat>(out var cat))
        {
            foreach (var battlePawn in catBattlePawns)
            {
                if (battlePawn.entityIdLinked == _entityId)
                {
                    battlePawn.Free();
                }
            }
        }
        
        if (Misc.GetEntityById(_entityId).TryGetComponent<Enemy>(out var enemy))
        {
            foreach (var battlePawn in enemyBattlePawns)
            {
                if (battlePawn.entityIdLinked == _entityId)
                {
                    battlePawn.Free();
                }
            }
        }
    }

    public BattlePawn GetNearestPawnFromCursor(Vector2 _originPos)
    {
        int closestPawnIndex = -1;
        float shortestDistance = 999;
        
        for (int i = 0; i < Instance.catBattlePawns.Length; i++)
        {
            // exceptions
            if (catBattlePawns[i].IsLocked()) continue;
            
            // getting the distance between both vectors
            Vector2 pawnPosition = new Vector2(Instance.catBattlePawns[i].transform.position.x, Instance.catBattlePawns[i].transform.position.y);
            float distanceBetweenPositions = Vector2.Distance(_originPos, pawnPosition);

            if (distanceBetweenPositions < shortestDistance)
            {
                shortestDistance = distanceBetweenPositions;
                closestPawnIndex = i;
            }
        }

        return Instance.catBattlePawns[closestPawnIndex];
    }

    public bool IsCloseEnough(Vector2 _u, Vector2 _v)
    {
        return Vector2.Distance(_u, _v) <= Instance.distanceThreshold;
    }
}