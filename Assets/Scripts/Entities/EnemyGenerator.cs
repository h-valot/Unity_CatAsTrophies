using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    public static EnemyGenerator Instance;
    
    [Header("DEBUGGING")]
    public List<Enemy> enemies;
    public GameObject enemyPrefab;

    private void Awake() => Instance = this;

    public void Initialize()
    {
        enemies = new List<Enemy>();
    }
    
    public Enemy CreateEnemy()
    {
        Enemy newEnemy = Instantiate(enemyPrefab, transform).GetComponent<Enemy>();
        
        EntityManager.Instance.entities.Add(newEnemy);
        enemies.Add(newEnemy);
        newEnemy.Initialize();

        Debug.Log("ENEMY GENERATOR: enemy created");
        return newEnemy;
    }

}