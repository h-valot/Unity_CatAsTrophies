using Data;
using Data.Player;
using List;
using UI.Init;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Init : MonoBehaviour
{
    public EntitiesConfig entitiesConfig;
    public GameSettings gameSettings;
    public PlayerConfig playerConfig;
    public MapConfig mapConfig;
    public Events events;

    [Header("REFERENCES")] 
    public LoadingScreen loadingScreen;
    public RSODataManager rsoDataManager;
    
    private async void Start()
    {
        Registry.entitiesConfig = entitiesConfig;
        Registry.gameSettings = gameSettings;
        Registry.playerConfig = playerConfig;
        Registry.mapConfig = mapConfig;
        Registry.events = events;

        DataManager.Load();
        rsoDataManager.Load();
        
        // debugging
        DataManager.data.playerStorage ??= new PlayerStorage();
        if (gameSettings.playerDeckDebugMode)
        {
            DataManager.data.playerStorage.deck = Registry.playerConfig.deck.Copy();
            if (DataManager.data.playerStorage.collection.Count == 0)
                foreach (var item in DataManager.data.playerStorage.deck)
                    DataManager.data.playerStorage.collection.Add(new Item(item.entityIndex));
        }

        if (gameSettings.playLoadingScreen) await loadingScreen.Animate();

        Registry.isInitialized = true;
        SceneManager.LoadScene(gameSettings.startingScene);
    }
}