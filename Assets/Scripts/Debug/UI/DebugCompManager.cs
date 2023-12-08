using UnityEngine;

public class DebugCompManager : MonoBehaviour
{
    public static DebugCompManager Instance;

    public Transform layoutGroup;
    public GameObject debugButtonPrefab;
    public GameObject graphicsParent;
    
    private void Awake() => Instance = this;
    
    public void InstantiateAllButtons()
    {
        for (var index = 0; index < Registry.entitiesConfig.compositions.Count; index++)
        {
            var newDebugButton = Instantiate(debugButtonPrefab, layoutGroup).GetComponent<DebugCompButton>();
            newDebugButton.Initialize(index);
        }
    }

    public void ShowDebugButtons() => graphicsParent.SetActive(true);
    public void HideDebugButtons() => graphicsParent.SetActive(false);
}