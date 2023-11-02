public class Enemy : Entity
{
    public int enemyType;

    public void Initialize(int _enemyType)
    {
        base.Initialize();

        enemyType = _enemyType;
    }
    
    private void OnEnable()
    {
        Registry.events.OnNewEnemyTurn += TriggerAllEffects;
    }

    private void OnDisable()
    {
        Registry.events.OnNewEnemyTurn -= TriggerAllEffects;
    }
}