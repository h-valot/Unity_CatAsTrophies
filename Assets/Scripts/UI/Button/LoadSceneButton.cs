using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneButton : MonoBehaviour
{
    public void LoadScene(string _sceneToLoad)
    {
        SceneManager.LoadScene(_sceneToLoad);
    }
}