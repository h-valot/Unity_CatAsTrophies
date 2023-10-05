using UnityEngine;

public class BattlefieldPositionManager : MonoBehaviour
{
    public static BattlefieldPositionManager Instance;
    
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
    
    public Vector2 GetNearestPawnFromCursor(Vector2 cursorPosition)
    {
        for (int i = 0; i < pawnPositions.Length; i++)
        {
            Vector2 pawnPosition = new Vector2(pawnPositions[i].transform.position.x, pawnPositions[i].transform.position.y);
            float distanceBetweenPositions = Mathf.Abs(Mathf.Sqrt((cursorPosition.x - pawnPosition.x) + (cursorPosition.y - pawnPosition.y)));
            if (distanceBetweenPositions <= distanceThreshold)
            {
                return pawnPosition;
            }
        }
        return Vector2.zero;
    }
}