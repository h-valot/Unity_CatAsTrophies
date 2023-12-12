using Data;
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
        CatManager.Instance.Initialize();
        EnemyGenerator.Instance.Initialize();
        TurnManager.Instance.Initialize();
        HandManager.Instance.Initialize();
        BattlefieldManager.Instance.Initialize(); 
        DiscardManager.Instance.Initialize();
        Registry.events.OnSceneLoaded?.Invoke();

        // instantiate player's deck of cats
        DeckManager.Instance.LoadPlayerDeck();
        DeckManager.Instance.ShuffleDeck();
        
        if (Registry.gameSettings.gameBattleDebugMode)
        {
            // debugging
            DebugCompManager.Instance.InstantiateAllButtons();
            DebugCompManager.Instance.ShowDebugButtons();
        }
        else
        {
            DebugCompManager.Instance.HideDebugButtons();
            EnemyGenerator.Instance.GenerateComposition(DataManager.data.compoIndexToLoad);
        }
        
        // start the game loop
        TurnManager.Instance.HandleTurnState();
    }
}