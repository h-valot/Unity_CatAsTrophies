using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;

    private void Awake() => Instance = this;

    private void Start()
    {
        // load the init scene if it hasn't been loaded yet
        if (!Registry.isInitialized)
        {
            SceneManager.LoadScene("Init");
            return;
        }
        
        BattlePawnManager.Instance.Initialize();
        
        // DEBUGGING
        HandManager.Instance.AddCat(0);
    }
}