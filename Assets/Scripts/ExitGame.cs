using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ExitGame : MonoBehaviour
{
    public void Quit()
    {
        Application.Quit();
        EditorApplication.isPlaying = false;
        Debug.Log("Quitter le jeu?");
    }
}
