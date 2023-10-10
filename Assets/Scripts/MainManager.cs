using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;

    public HandManager handManager;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (!Registry.isInitialized)
        {
            SceneManager.LoadScene("Init");
            return;
        }
        
        BattlePawnManager.Instance.Initialize();
        
        // debug
        handManager.AddCat(0);
    }
}