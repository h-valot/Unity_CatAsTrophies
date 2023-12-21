using UnityEngine;

public class SettingsUIManager : MonoBehaviour
{
    [Header("REFERENCES")] 
    public GameObject settings;
    public GameObject confirmExit;
    public GameObject credits;

    public void HideAll()
    {
        HideSettings();
        HideConfirmExit();
        HideCredits();
    }
    
    public void ShowSettings() => settings.SetActive(true);
    public void HideSettings() => settings.SetActive(false);
    public void ShowConfirmExit() => confirmExit.SetActive(true);
    public void HideConfirmExit() => confirmExit.SetActive(false);
    public void ShowCredits() => credits.SetActive(true);
    public void HideCredits() => credits.SetActive(false);

    public void ExitGame() => Application.Quit();
}