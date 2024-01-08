using Audio;
using UI.MainMenu;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public VolumeController volumeController;
    public ButtonsAnimation buttonsAnimation;
    
    private void Start()
    {
        // load the init scene if it hasn't been loaded yet
        if (!Registry.isInitialized)
        {
            SceneManager.LoadScene("Init");
            return;
        }
        
        Registry.events.OnSceneLoaded?.Invoke();
        volumeController.Initialize();
        buttonsAnimation.Animate();
    }
}