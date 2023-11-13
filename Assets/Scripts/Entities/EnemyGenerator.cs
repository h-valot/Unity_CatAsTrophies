using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    public static EnemyGenerator Instance;
    
    [Header("DEBUGGING")]
    public List<Enemy> enemies;
    public GameObject enemyPrefab;
    public int totalEnemyCount;

    private void Awake() => Instance = this;

    public void Initialize()
    {
        enemies = new List<Enemy>();
    }

    public void GenerateComposition(int _compositionIndex)
    {
        // exception
        if (_compositionIndex > Registry.entitiesConfig.compositions.Count)
        {
            Debug.LogError($"ENEMY GENERATOR: trying to generate an enemy composition that doesn't exists", this);
            return;
        }
        
        // generate one enemy per battle pawn
        for (int enemyIndex = 0; enemyIndex < BattlefieldManager.Instance.enemyBattlePawns.Length; enemyIndex++)
        {
            // continue if the composition slot is empty
            if (!Registry.entitiesConfig.compositions[_compositionIndex].entities[enemyIndex]) continue;
            
            var newEnemyId = SpawnEnemyGraphics(_compositionIndex, enemyIndex);
            BattlefieldManager.Instance.enemyBattlePawns[enemyIndex].Setup(newEnemyId);
            Misc.GetEntityById(newEnemyId).gameObject.transform.position = BattlefieldManager.Instance.enemyBattlePawns[enemyIndex].transform.position;
        }
    }

    public string SpawnEnemyGraphics(int _compositionIndex, int _enemyIndex)
    {
        // creating enemy        
        Enemy newEnemy = Instantiate(Registry.entitiesConfig.compositions[_compositionIndex].entities[_enemyIndex].basePrefab, transform).GetComponent<Enemy>();
        newEnemy.Initialize(Registry.entitiesConfig.enemies.IndexOf(Registry.entitiesConfig.compositions[_compositionIndex].entities[_enemyIndex]));
        newEnemy.name = $"Enemy_{totalEnemyCount}_{Registry.entitiesConfig.compositions[_compositionIndex].entities[_enemyIndex].entityName}";
        totalEnemyCount++;
        
        // store entities to lists (reference for misc's functions)
        EntityManager.Instance.entities.Add(newEnemy);
        enemies.Add(newEnemy);

        return newEnemy.id;
    }
}