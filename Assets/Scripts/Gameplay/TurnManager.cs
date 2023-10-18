using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public TurnState state;
    
        
}

public enum TurnState
{
    PlayerTurn = 0,
    EnemyTurn
}