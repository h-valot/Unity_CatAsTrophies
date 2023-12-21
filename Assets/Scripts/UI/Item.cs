using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Battle
{
    public class Item : MonoBehaviour
    {
        [Header("INFORMATION")]
        public TextMeshProUGUI nameTM;

        [Header("IMAGE")]
        public Image catFaceImage;
        public Image catBackgroundImage;
        public Sprite epicBackground;
        public Sprite commonBackground;
    
        [Header("HEALTH")]
        public TextMeshProUGUI healthTM;
        public Image healthFillImage;
    
        public void UpdateDisplay(string catId)
        {
            var catType = Misc.GetCatById(catId).catType;
            
            nameTM.text = Registry.entitiesConfig.cats[catType].entityName;

            healthTM.text = $"{Misc.GetCatById(catId).health} / {Registry.entitiesConfig.cats[catType].health}";
            healthFillImage.fillAmount = (float)Misc.GetCatById(catId).health / (float)Registry.entitiesConfig.cats[catType].health;
        
            catFaceImage.sprite = Registry.entitiesConfig.cats[catType].sprite;
            catBackgroundImage.sprite = Registry.entitiesConfig.cats[catType].rarety == Rarety.EPIC ? epicBackground : commonBackground;
        }
    }
}