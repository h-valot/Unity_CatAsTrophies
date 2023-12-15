using UnityEngine;

public class ResurrectionUICard : MonoBehaviour
{
    [Header("REFERENCES")]
    public GameObject graphicsParent;

    public int id;

    public void Initialize(int newId)
    {
        id = newId;
        UpdateGraphics();
    }

    private void UpdateGraphics()
    {
        
    }
    
    
}