using System;
using UnityEngine;

public class GameMenu : MonoBehaviour
{
    [Header("REFERENCES")]
    public GameObject menuGraphicsParent;
    
    [HideInInspector] public bool isActivated;

    public void Initialize()
    {
        CloseMenu();
    }
    
    public void OpenMenu()
    {
        menuGraphicsParent.SetActive(true);
        isActivated = true;
    }
    
    public void CloseMenu()
    {
        menuGraphicsParent.SetActive(false);
        isActivated = false;
    }
}