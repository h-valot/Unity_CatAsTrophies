using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ExitGame : MonoBehaviour
{
    public void Quit()
    {
        Application.Quit();
        #if UNITY_EDITOR
        EditorApplication.isPlaying = false;
        Debug.Log("Quitter le jeu?");
        #endif
    }
}
