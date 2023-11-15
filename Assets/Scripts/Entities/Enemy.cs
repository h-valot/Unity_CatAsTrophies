using System;
using UnityEngine;

public class Enemy : Entity
{
    [Header("DEBUGGING")]
    public int enemyType;

    public void Initialize(int _enemyType)
    {
        base.Initialize();
        enemyType = _enemyType;
        
        // GAME STATS
        maxHealth = Registry.entitiesConfig.cats[enemyType].health;
        health = maxHealth;
        autoAttacks = Registry.entitiesConfig.enemies[enemyType].autoAttack;  
        
        // update game stat on ui displayer
        OnStatsUpdate?.Invoke();
        
        // graphics tweaking
        graphicsParent.transform.eulerAngles = battleRotation;
        graphicsParent.transform.localScale *= battleScale;
        
        OnBattlefieldEntered?.Invoke();
    }
    
    private void OnEnable()
    {
        Registry.events.OnNewEnemyTurn += ResetArmor;
        Registry.events.OnNewEnemyTurn += TriggerAllEffects;
        Registry.events.OnEnemiesUseAutoAttack += UseAutoAttack;
    }

    private void OnDisable()
    {
        Registry.events.OnNewEnemyTurn -= ResetArmor;
        Registry.events.OnNewEnemyTurn -= TriggerAllEffects;
        Registry.events.OnEnemiesUseAutoAttack -= UseAutoAttack;
    }
    
    public override void UpdateBattlePosition(BattlePosition _battlePosition)
    {
        base.UpdateBattlePosition(_battlePosition);
        
        // set the entity position to the corresponding battle pawn
        transform.position = BattlefieldManager.Instance.enemyBattlePawns[(int)battlePosition].transform.position;
    }
    
    /// <summary>
    /// Hide the cat's graphics and add a reference to it 
    /// </summary>
    public override void HandleDeath()
    {
        // handle graphics tweaking
        graphicsParent.SetActive(false);
        BattlefieldManager.Instance.RemoveFromBattlePawn(id);
    }
}