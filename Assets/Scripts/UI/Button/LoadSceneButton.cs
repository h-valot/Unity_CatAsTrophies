using Data;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneButton : MonoBehaviour
{
    public void LoadScene(string sceneToLoad)
    {
        // exception, update end battle left status
        if (sceneToLoad == "MainMenu" && 
            SceneManager.GetActiveScene().name == "GameBattle" &&
            DataManager.data.endBattleStatus == EndBattleStatus.NONE)
        {
            DataManager.data.endBattleStatus = EndBattleStatus.LEFT;
        }
        
        SceneManager.LoadScene(sceneToLoad);
    }
}