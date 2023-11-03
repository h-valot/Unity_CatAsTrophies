using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EntityUIDisplay : MonoBehaviour
{
    [Header("REFERENCES")] 
    public Entity entity;
    
    [Header("UI DISPLAY")] 
    public GameObject canvasGO;
    public Image healthFillImage;
    public TextMeshProUGUI healthTM;

    private void Update()
    {
        if (entity.TryGetComponent(out Cat cat))
        {
            canvasGO.gameObject.SetActive(cat.state == CatState.OnBattle);
        }

        // exit if the canvas isn't active
        if (!canvasGO.activeInHierarchy) return;

        // update ui displays
        healthFillImage.fillAmount = (float)entity.health / (float)entity.maxHealth;
        healthTM.text = $"{entity.health}/{entity.maxHealth}";
    }
}