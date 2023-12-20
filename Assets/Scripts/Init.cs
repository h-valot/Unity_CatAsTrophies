using Data;
using List;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Init : MonoBehaviour
{
    public EntitiesConfig entitiesConfig;
    public GameSettings gameSettings;
    public PlayerConfig playerConfig;
    public MapConfig mapConfig;
    public Events events;

    [Header("GRAPHICS")] 
    public LoadingScreenUIManager loadingScreenUIManager;
    
    private async void Start()
    {
        Registry.entitiesConfig = entitiesConfig;
        Registry.gameSettings = gameSettings;
        Registry.playerConfig = playerConfig;
        Registry.mapConfig = mapConfig;
        Registry.events = events;

        DataManager.Load();
        
        // debugging
        DataManager.data.playerStorage ??= new PlayerStorage();
        if (gameSettings.playerDeckDebugMode)
        {
            DataManager.data.playerStorage.deck = Registry.playerConfig.deck.Copy();
            if (DataManager.data.playerStorage.collection.Count == 0)
                foreach (var item in DataManager.data.playerStorage.deck)
                    DataManager.data.playerStorage.collection.Add(new Item(item.entityIndex));
        }

        if (gameSettings.playLoadingScreen) await loadingScreenUIManager.Animate();

        Registry.isInitialized = true;
        SceneManager.LoadScene(gameSettings.startingScene);
    }
}