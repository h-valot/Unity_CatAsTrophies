using UnityEngine;

public class PauseButton : MonoBehaviour
{
    [SerializeField] private GameObject buttonGraphicsParent;
    [SerializeField] private GameObject menuGraphicsParent;
    private bool isActivated;

    private void Start()
    {
        if (menuGraphicsParent == null)
        {
            Debug.LogError("PauseButton.cs: Reference non-attributed in inspector.", this);
            Debug.Break();
        }
    }

    public void ToggleMenu()
    {
        if (isActivated)
        {
            CloseMenu();
        }
        else
        {
            OpenMenu();
        }
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
