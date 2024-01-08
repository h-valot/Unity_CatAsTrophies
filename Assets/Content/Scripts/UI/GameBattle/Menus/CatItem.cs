using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.GameBattle.Menus
{
    public class CatItem : MonoBehaviour
    {
        [Header("EXTERNAL REFERENCES")] 
        public EntitiesConfig entitiesConfig;
        
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
            var catType = Misc.IdManager.GetCatById(catId).catType;
            
            nameTM.text = entitiesConfig.cats[catType].entityName;

            healthTM.text = $"{Misc.IdManager.GetCatById(catId).health} / {entitiesConfig.cats[catType].health}";
            healthFillImage.fillAmount = (float)Misc.IdManager.GetCatById(catId).health / (float)entitiesConfig.cats[catType].health;
        
            catFaceImage.sprite = entitiesConfig.cats[catType].sprite;
            catBackgroundImage.sprite = entitiesConfig.cats[catType].rarety == Rarety.EPIC ? epicBackground : commonBackground;
        }
    }
}