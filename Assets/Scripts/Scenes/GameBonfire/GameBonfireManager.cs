using Audio;
using UI.GameBonfire;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameBonfireManager : MonoBehaviour
{
    public VolumeController volumeController;
    public HealingManager healingManager;
    
    void Start()
    {
        // load the init scene if it hasn't been loaded yet
        if (!Registry.isInitialized)
        {
            SceneManager.LoadScene("Init");
            return;
        }
        
        Registry.events.OnSceneLoaded?.Invoke();    
        healingManager.Initialize();
        volumeController.Initialize();
    }
}