using UnityEngine;

namespace UI.Menu
{
    public class MenuManager : MonoBehaviour
    {
        public static MenuManager Instance;
    
        [Header("REFERENCES")] 
        public MenuItem[] gameMenus;
    
        private void Awake() => Instance = this;

        private void OnEnable()
        {
            if (Registry.events == null) return;
            Registry.events.OnSceneLoaded += Initialize;
        }

        private void OnDisable()
        {
            if (Registry.events == null) return;
            Registry.events.OnSceneLoaded -= Initialize;
        }
    
        public void Initialize()
        {
            foreach (MenuItem menu in gameMenus)
            {
                menu.Initialize();
            }
        }
    
        /// <summary>
        /// Check if there is any ui menu currently opened
        /// </summary>
        public bool IsMenuOpened()
        {
            bool output = false;
        
            foreach (MenuItem menu in gameMenus)
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
}