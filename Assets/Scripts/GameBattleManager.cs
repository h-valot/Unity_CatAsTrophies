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
        
        // initialize all managers - the order matters
        DeckManager.Instance.Initialize();
        CatGenerator.Instance.Initialize();
        EnemyGenerator.Instance.Initialize();
        TurnManager.Instance.Initialize();
        HandManager.Instance.Initialize();
        BattlefieldManager.Instance.Initialize();
        DiscardManager.Instance.Initialize();
        Registry.events.OnSceneLoaded?.Invoke();

        // instantiate player's deck of cats
        DeckManager.Instance.LoadPlayerDeck();
        DeckManager.Instance.ShuffleDeck();
        
        // debugging
        DebugCompManager.Instance.InstantiateAllButtons();
        
        // start the game loop
        TurnManager.Instance.HandleTurnState();
    }
}