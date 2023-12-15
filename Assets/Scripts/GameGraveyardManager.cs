using UnityEngine;
using UnityEngine.SceneManagement;

public class GameGraveyardManager : MonoBehaviour
{
    [Header("REFERENCES")]
    public ResurrectionManager resurrectionManager;
    
    void Start()
    {
        // load the init scene if it hasn't been loaded yet
        if (!Registry.isInitialized)
        {
            SceneManager.LoadScene("Init");
            return;
        }

        Registry.events.OnSceneLoaded?.Invoke();
        resurrectionManager.Initialize();
    }
}