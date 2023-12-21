using UnityEngine;
using UnityEngine.SceneManagement;

public class GameBonfireManager : MonoBehaviour
{
    public VolumeControl volumeControl;
    public HealingUIManager healingUIManager;
    
    void Start()
    {
        // load the init scene if it hasn't been loaded yet
        if (!Registry.isInitialized)
        {
            SceneManager.LoadScene("Init");
            return;
        }
        
        Registry.events.OnSceneLoaded?.Invoke();    
        healingUIManager.Initialize();
        volumeControl.Initialize();
    }
}