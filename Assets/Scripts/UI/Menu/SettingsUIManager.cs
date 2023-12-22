using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsUIManager : MonoBehaviour
{
    [Header("REFERENCES")] 
    public GameObject settings;
    public GameObject confirmExit;
    public GameObject confirmExitBattle;
    public GameObject credits;

    public void HideAll()
    {
        HideSettings();
        HideConfirmExit();
        HideConfirmExitBattle();
        HideCredits();
    }
    
    public void ShowSettings() => settings.SetActive(true);
    public void HideSettings() => settings.SetActive(false);
    public void ShowConfirmExit() => confirmExit.SetActive(true);
    public void HideConfirmExit() => confirmExit.SetActive(false);
    public void ShowConfirmExitBattle() => confirmExitBattle.SetActive(true);
    public void HideConfirmExitBattle() => confirmExitBattle.SetActive(false);
    public void ShowCredits() => credits.SetActive(true);
    public void HideCredits() => credits.SetActive(false);

    public void ExitGame() => Application.Quit();
    public void LoadMainMenu() => SceneManager.LoadScene("mainmenu");
}