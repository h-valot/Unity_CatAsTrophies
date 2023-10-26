using UnityEngine;
using UnityEngine.SceneManagement;

public class GameBattleManager : MonoBehaviour
{
    public static GameBattleManager Instance;
    private void Awake() => Instance = this;

    private void Start()
    {
        // load the init scene if it hasn't been loaded yet
        if (!Registry.isInitialized)
        {
            SceneManager.LoadScene("Init");
            return;
        }
        
        // the order matters
        // gameplay
        DeckManager.Instance.Initialize();
        CatGenerator.Instance.Initialize();
        EnemyGenerator.Instance.Initialize();
        TurnManager.Instance.Initialize();
        HandManager.Instance.Initialize();
        BattlefieldManager.Instance.Initialize();
        GraveyardManager.Instance.Initialize();
        // ui
        MenuManager.Instance.Initialize();

        foreach (var battlePawn in BattlefieldManager.Instance.enemyBattlePawns)
        {
            var newEnemy = EnemyGenerator.Instance.CreateEnemy();
            battlePawn.Setup(newEnemy.id);
            newEnemy.transform.position = battlePawn.transform.position;
        }
        
        // start the game loop
        TurnManager.Instance.HandleTurnState();
    }
}