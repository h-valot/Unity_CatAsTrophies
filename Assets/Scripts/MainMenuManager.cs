using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager Instance;
    private void Awake() => Instance = this;

    private void Start()
    {
        // load the init scene if it hasn't been loaded yet
        if (!Registry.isInitialized)
        {
            SceneManager.LoadScene("Init");
            return;
        }
    }
}