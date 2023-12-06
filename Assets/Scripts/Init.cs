using Data;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Init : MonoBehaviour
{
    public EntitiesConfig entitiesConfig;
    public GameSettings gameSettings;
    public PlayerConfig playerConfig;
    public MapConfig mapConfig;
    public RewardsConfig rewardsConfig;
    public Events events;

    private void Start()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(gameSettings.startingScene);
        
        Registry.entitiesConfig = entitiesConfig;
        Registry.gameSettings = gameSettings;
        Registry.playerConfig = playerConfig;
        Registry.mapConfig = mapConfig;
        Registry.rewardsConfig = rewardsConfig;
        Registry.events = events;

        DataManager.Load();
        
        Registry.isInitialized = true;
        asyncLoad.allowSceneActivation = true;
    }
}