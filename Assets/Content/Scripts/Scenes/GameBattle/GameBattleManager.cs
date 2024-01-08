using Audio;
using Data;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameBattleManager : MonoBehaviour
{
    [Header("EXTERNAL REFERENCES")] 
    public DebugCompManager debugCompManager;
    public VolumeController volumeController;
    public GameSettings gameSettings;
    public Events events;

    [Header("MANAGERS")] 
    public DeckManager deckManager;
    public CatManager catManager;
    
    private void Start()
    {
        // load the init scene if it hasn't been loaded yet
        if (!Registry.isInitialized)
        {
            SceneManager.LoadScene("Init");
            return;
        }
        
        // initialize all managers - the order matters
        deckManager.Initialize();
        catManager.Initialize();
        EnemyGenerator.Instance.Initialize();
        TurnManager.Instance.Initialize();
        HandManager.Instance.Initialize();
        BattlefieldManager.Instance.Initialize(); 
        DiscardManager.Instance.Initialize();
        events.OnSceneLoaded?.Invoke();
        volumeController.Initialize();

        // instantiate player's deck of cats
        deckManager.LoadPlayerDeck();
        deckManager.ShuffleDeck();
        
        if (gameSettings.gameBattleDebugMode)
        {
            // debugging
            debugCompManager.InstantiateAllButtons();
            debugCompManager.Show();
        }
        else
        {
            debugCompManager.Hide();
            EnemyGenerator.Instance.GenerateComposition(DataManager.data.compoToLoad);
        }
        
        // start the game loop
        TurnManager.Instance.HandleTurnState();
    }
}