using System.Threading.Tasks;
using Data;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HealingUIManager : MonoBehaviour
{
    [Header("EXTERNAL REFERENCES")] 
    public MapManager mapManager;
    
    [Header("REFERENCES")]
    public GameObject heal;
    public GameObject done;

    [Header("DONE PANEL")] 
    public float doneDuration = 3f;
    public Image doneLoadingImage;
    
    public void Initialize()
    {
        ShowHeal();
    }
    
    public void ShowHeal() => heal.SetActive(true);
    public void HideHeal() => heal.SetActive(false);
    public async Task ShowDone()
    {
        done.SetActive(true);
        doneLoadingImage.DOFillAmount(1, doneDuration);
        await Task.Delay(Mathf.RoundToInt(1000 * doneDuration));
        
        mapManager.ShowCanvasLocked();
    }
    public void HideDone() => done.SetActive(false);
    
    /// <summary>
    /// Heals all player's in-game deck cats by a fixed amount setted in game settings
    /// </summary>
    public async void HealCats()
    {
        foreach (var item in DataManager.data.playerStorage.inGameDeck)
        {
            foreach (var cat in item.cats)
            {
                // continue, if the cat is dead
                if (cat.isDead) continue;
            
                cat.health += Registry.gameSettings.healAmount;
                if (cat.health > Registry.entitiesConfig.cats[item.entityIndex].health)
                    cat.health = Registry.entitiesConfig.cats[item.entityIndex].health;
            }
        }
        
        // stop fire noises
        Registry.events.onRestClick?.Invoke();
        
        HideHeal();
        await ShowDone();
    }
}