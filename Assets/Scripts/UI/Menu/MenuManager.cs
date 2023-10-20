using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;
    private void Awake() => Instance = this;
    
    [Header("REFERENCES")] 
    public GameMenu[] gameMenus;
    
    /// <summary>
    /// Check if there is any ui menu currently opened
    /// </summary>
    public bool IsMenuOpened()
    {
        bool output = false;
        
        foreach (GameMenu menu in gameMenus)
        {
            if (menu.isActivated)
            {
                output = true;
                break;
            }
        }

        return output;
    }
}