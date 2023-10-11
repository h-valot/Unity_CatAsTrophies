using UnityEngine;

public class Cat : MonoBehaviour
{
    public BattleState state;
    public int typeIndex;

    public void Initialize(int _typeIndex)
    {
        typeIndex = _typeIndex;
    }
    
    public bool CanMove() => state == BattleState.InHand;

    public void Remove()
    {
        // remove this chick
    }
}

public enum BattleState
{
    InHand = 0,
    OnBattle,
    Discarded,
    InDeck
}