using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInitializer : MonoBehaviour
{
    public GameSettings gameSettings;
    public CardsConfig cardConfig;

    private void Start()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(gameSettings.startingScene);
        
        Registry.cardConfig = cardConfig;
        Registry.gameSettings = gameSettings;

        Registry.isInitialized = true;
        asyncLoad.allowSceneActivation = true;
    }
}