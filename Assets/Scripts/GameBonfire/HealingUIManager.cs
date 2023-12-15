using Data;
using UnityEngine;

public class HealingUIManager : MonoBehaviour
{
    [Header("REFERENCES")]
    public GameObject graphicsParent;
    
    public void Initialize()
    {
        Hide();
    }
    
    public void Show() => graphicsParent.SetActive(true);
    public void Hide() => graphicsParent.SetActive(false);
    
    /// <summary>
    /// Heals all player's in-game deck cats by a fixed amount setted in game settings
    /// </summary>
    public void HealCats()
    {
        foreach (var item in DataManager.data.playerStorage.inGameDeck)
        {
            // continue, if the cat is dead
            if (item.isDead) continue;
            
            item.health += Registry.gameSettings.healAmount;
            if (item.health > Registry.entitiesConfig.cats[item.entityIndex].health)
                item.health = Registry.entitiesConfig.cats[item.entityIndex].health;
        }
    }
}