using UnityEngine;
using UnityEngine.SceneManagement;

public class GameGraveyardManager : MonoBehaviour
{
    [Header("REFERENCES")]
    public RessurectionUIManager ressurectionUIManager;
    
    void Start()
    {
        // load the init scene if it hasn't been loaded yet
        if (!Registry.isInitialized)
        {
            SceneManager.LoadScene("Init");
            return;
        }

        Registry.events.OnSceneLoaded?.Invoke();
        ressurectionUIManager.Initialize();
    }
}