using UnityEngine;
using UnityEngine.SceneManagement;

public class GameGraveyardManager : MonoBehaviour
{
    public VolumeControl volumeControl;
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
        volumeControl.Initialize();
        resurrectionManager.Initialize();
    }
}