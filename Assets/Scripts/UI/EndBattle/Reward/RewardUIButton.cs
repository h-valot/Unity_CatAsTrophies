using Data;
using TMPro;
using UI.Shop;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Reward
{
    public class RewardUIButton : MonoBehaviour
    {
        [Header("REFERENCES")] 
        public TransactionRewardUIManager transactionRewardUIManager;
        public RSE_DebugLog rseDebugLog;
        public GameObject faceParent;
        public GameObject infoParent;

        [Header("GLOBAL")]
        public TextMeshProUGUI catNameTM;
        public GameObject selectButton;
        public GameObject buyButton;
        public TextMeshProUGUI buyPriceTM;

        [Header("REWARD")] 
        public Image catFaceImage;
        public Image catBackgroundImage;
        public Sprite epicBackground, commonBackground;
    
        [Header("INFO")]
        public TextMeshProUGUI infoCatHealthTM;
        public TextMeshProUGUI infoCatAbilityTM;
    
        private int _catIndex;
        private bool _isInfoShown;

        public void UpdateDisplay(int newCatRewardIndex)
        {
            _catIndex = newCatRewardIndex;
        
            catNameTM.text = Registry.entitiesConfig.cats[_catIndex].entityName;
        
            catBackgroundImage.sprite = Registry.entitiesConfig.cats[_catIndex].rarety == Rarety.EPIC ? epicBackground : commonBackground;
            catFaceImage.sprite = Registry.entitiesConfig.cats[_catIndex].sprite;

            infoCatHealthTM.text = $"{Registry.entitiesConfig.cats[_catIndex].health}";
            infoCatAbilityTM.text = $"{Registry.entitiesConfig.cats[_catIndex].abilityDescription}";
        
            selectButton.SetActive(Registry.entitiesConfig.cats[_catIndex].pricing == RewardPricing.FREE);
            buyButton.SetActive(Registry.entitiesConfig.cats[_catIndex].pricing == RewardPricing.PREMIUM);
            buyPriceTM.text = $"{Registry.entitiesConfig.cats[_catIndex].cost}";
        }

        public void ShowInfo()
        {
            _isInfoShown = true;
            infoParent.SetActive(true);
            faceParent.SetActive(false);
        }
    
        public void HideInfo()
        {
            _isInfoShown = false;
            infoParent.SetActive(false);
            faceParent.SetActive(true);
        }
        
        public void GatherReward()
        {
            DataManager.data.playerStorage.AddToInGameDeck(_catIndex);
            rseDebugLog.Call($"{Registry.entitiesConfig.cats[_catIndex].entityName} has been added to you in game deck", Color.white);
        }

        public void Buy()
        {
            transactionRewardUIManager.UpdateGraphics(_catIndex);
            transactionRewardUIManager.Show();
        }
    }
}