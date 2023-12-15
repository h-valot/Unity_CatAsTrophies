using System.Linq;
using Data;
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

    private void Start()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(gameSettings.startingScene);
        
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
            DataManager.data.playerStorage.deck = Registry.playerConfig.deck.ToList();
        }
        
        Registry.isInitialized = true;
        asyncLoad.allowSceneActivation = true;
    }
}