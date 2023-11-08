using JetBrains.Annotations;
using UnityEngine;

public class Enemy : Entity
{
    public int enemyType;

    [Header("REFERENCES")]
    public GameObject graphicsParent;
    public Animator animator;

    [Header("GRAPHICS TWEAKING")]
    public Vector3 battleRotation;
    public Vector3 baseRotation;
    public float battleScale;


    // Place enemy on battlefield 
    public void Initialize(int _enemyType)
    {
        base.Initialize();
        enemyType = _enemyType;

        graphicsParent.transform.eulerAngles = battleRotation;
        graphicsParent.transform.localScale *= battleScale;

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