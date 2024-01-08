using UnityEngine;

public class DebugCompManager : MonoBehaviour
{
    [Header("EXTERNAL REFERENCES")]
    public EntitiesConfig entitiesConfig;

    [Header("REFERENCES")]
    public Transform layoutGroup;
    public GameObject debugButtonPrefab;
    public GameObject graphicsParent;
    
    public void InstantiateAllButtons()
    {
        for (var index = 0; index < entitiesConfig.compositions.Count; index++)
        {
            var newDebugButton = Instantiate(debugButtonPrefab, layoutGroup).GetComponent<DebugCompButton>();
            newDebugButton.Initialize(index);
        }
    }

    public void Show() => graphicsParent.SetActive(true);
    public void Hide() => graphicsParent.SetActive(false);
}