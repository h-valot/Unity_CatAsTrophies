using Data;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Button
{
    public class SceneLoader : MonoBehaviour
    {
        public void LoadScene(string sceneToLoad)
        {
            // exception, update end battle left status
            if (sceneToLoad == "mainmenu" && 
                SceneManager.GetActiveScene().name == "GameBattle" &&
                DataManager.data.endBattleStatus == EndBattleStatus.NONE)
            {
                DataManager.data.endBattleStatus = EndBattleStatus.LEFT;
            }
        
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}