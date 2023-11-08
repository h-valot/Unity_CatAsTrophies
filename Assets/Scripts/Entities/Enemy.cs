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
    
    /// <summary>
    /// Hide the cat's graphics and add a reference to it 
    /// </summary>
    public override void HandleDeath()
    {
        // handle graphics tweaking
        graphicsParent.SetActive(false);
        GraveyardManager.Instance.AddCat(id);
    }
}