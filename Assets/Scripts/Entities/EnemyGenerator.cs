using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyGenerator : MonoBehaviour
{
    public static EnemyGenerator Instance;
    
    [Header("DEBUGGING")]
    public List<Enemy> enemies;
    public int totalEnemyCount;
    public bool allEnemiesDead;

    private void Awake() => Instance = this;

    public void Initialize()
    {
        enemies = new List<Enemy>();
    }

    public void GenerateComposition(CompositionConfig composition)
    {
        // exception
        if (composition == null)
        {
            Debug.LogError($"ENEMY GENERATOR: trying to generate an enemy composition that doesn't exists", this);
            return;
        }
        
        // generate one enemy per battle pawn
        for (int enemyIndex = 0; enemyIndex < BattlefieldManager.Instance.enemyBattlePawns.Length; enemyIndex++)
        {
            // continue if the composition slot is empty
            if (!composition.entities[enemyIndex]) continue;
            
            var newEnemyId = SpawnEnemyGraphics(composition, enemyIndex);
            BattlefieldManager.Instance.enemyBattlePawns[enemyIndex].Setup(newEnemyId);
            Misc.GetEntityById(newEnemyId).gameObject.transform.position = BattlefieldManager.Instance.enemyBattlePawns[enemyIndex].transform.position;
        }
    }

    private string SpawnEnemyGraphics(CompositionConfig composition, int enemyIndex)
    {
        // creating enemy        
        Enemy newEnemy = Instantiate(composition.entities[enemyIndex].basePrefab, transform).GetComponent<Enemy>();
        newEnemy.Initialize(Registry.entitiesConfig.enemies.IndexOf(composition.entities[enemyIndex]));
        newEnemy.name = $"Enemy_{totalEnemyCount}_{composition.entities[enemyIndex].entityName}";
        totalEnemyCount++;
        
        // store entities to lists (reference for misc functions)
        EntityManager.Instance.entities.Add(newEnemy);
        enemies.Add(newEnemy);

        return newEnemy.id;
    }

    public void Remove(Enemy enemy)
    {
        enemies.Remove(enemy);

        // exit, if the debug mode in enabled
        if (Registry.gameSettings.gameBattleDebugMode) return;

        // exit, if there are no enemies left
        if (enemies.Count > 0) return;
        
        allEnemiesDead = true;
    }
}