using Data;
using UnityEditor;
using UnityEngine;

public class ExitGameButton : MonoBehaviour
{
    public void QuitApplication()
    {
        DataManager.Save();

        Application.Quit();
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
        Debug.Log("BUTTON MANAGER: Application exited");
#endif
    }
}