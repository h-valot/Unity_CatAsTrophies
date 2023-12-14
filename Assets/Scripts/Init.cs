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
        if (gameSettings.playerDeckDebugMode)
        {
            DataManager.data.playerStorage.deck = Registry.playerConfig.deck;
            if (DataManager.data.playerStorage.collection.Count == 0)
                foreach (var item in DataManager.data.playerStorage.deck)
                    DataManager.data.playerStorage.collection.Add(new Item(item.entityIndex, 0));
        }
        
        Registry.isInitialized = true;
        asyncLoad.allowSceneActivation = true;
    }
}