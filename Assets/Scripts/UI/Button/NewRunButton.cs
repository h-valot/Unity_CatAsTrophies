using UnityEngine;
using UnityEngine.SceneManagement;

public class NewRunButton : MonoBehaviour
{
    public void LaunchGame()
    {
        SceneManager.LoadScene("Main");
    }
}