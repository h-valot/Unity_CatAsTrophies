using UnityEngine;
using UnityEngine.SceneManagement;

public class GameBonfireManager : MonoBehaviour
{
    public VolumeControl volumeControl;
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
    }
}