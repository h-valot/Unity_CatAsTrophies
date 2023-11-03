using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Events", menuName = "Middle-Men/Events", order = 1)]
public class Events : ScriptableObject
{
    public Action OnNewPlayerTurn;
    public Action OnNewEnemyTurn;

    public Action OnCatsUseAutoAttack;
    public Action OnEnemiesUseAutoAttack;
}