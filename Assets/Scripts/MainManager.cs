using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;

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
        
        InputHandler.Instance.Initialize();
        DragManager.Instance.Initialize();
        BattlefieldPositionManager.Instance.Initialize();
        
        // debug
        HandHandler.Instance.AddCard(0);
    }
}