public class Enemy : Entity
{
    public int enemyType;

    public void Initialize(int _enemyType)
    {
        base.Initialize();
        enemyType = _enemyType;
        
        // GAME STATS
        autoAttacks = Registry.entitiesConfig.enemies[enemyType].autoAttack;  
    }
    
    private void OnEnable()
    {
        Registry.events.OnNewEnemyTurn += TriggerAllEffects;
        Registry.events.OnEnemiesUseAutoAttack += UseAutoAttack;
    }

    private void OnDisable()
    {
        Registry.events.OnNewEnemyTurn -= TriggerAllEffects;
        Registry.events.OnEnemiesUseAutoAttack -= UseAutoAttack;
    }
}