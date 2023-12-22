using Data;
using TMPro;
using UI.Reward;
using UnityEngine;

namespace UI.Currencies
{
    public class TransactionReward : MonoBehaviour
    {
        [Header("EXTERNAL REFERENCES")]
        public RewardManager rewardManager; 
        public Shop.ShopManager shopManager;
            
        [Header("REFERENCES")]
        public GameObject graphicsParent;
        public RSE_DebugLog rseDebugLog;
        public RSO_CurrencyTuna rsoCurrencyTuna;

        [Header("NOT ENOUGH")] 
        public GameObject buyButton;
        public GameObject coinsButton;

        [Header("TEXTS")] 
        public TextMeshProUGUI titleTM;
        public TextMeshProUGUI flavorTM;
        public TextMeshProUGUI confirmationTM;

        private int _catIndex;
    
        public void UpdateGraphics(int catIndex)
        {
            _catIndex = catIndex;
            
            titleTM.text = Registry.entitiesConfig.cats[catIndex].entityName;
            confirmationTM.text = $"{Mathf.RoundToInt(Registry.entitiesConfig.cats[catIndex].cost)}";
            
            var canBuy = rsoCurrencyTuna.value >= Mathf.RoundToInt(Registry.entitiesConfig.cats[catIndex].cost);
            flavorTM.text = canBuy ? $"New balance: {rsoCurrencyTuna.value - Mathf.RoundToInt(Registry.entitiesConfig.cats[catIndex].cost)} TUNA" : "Not enough";

            if (!canBuy)
            {
                coinsButton.SetActive(true);
                buyButton.SetActive(false);
            }
            else
            {
                coinsButton.SetActive(false);
                buyButton.SetActive(true);
            }
        }

        public void OpenCoins()
        {
            shopManager.ShowOnCoins();
            Hide();
        }
    
        public void Buy()
        {
            rsoCurrencyTuna.value -= Mathf.RoundToInt(Registry.entitiesConfig.cats[_catIndex].cost);
            DataManager.data.playerStorage.AddToInGameDeck(_catIndex);
            rseDebugLog.Call($"{Registry.entitiesConfig.cats[_catIndex].entityName} has been added to you in game deck", Color.white);
            
            rewardManager.Hide();
            Hide();
        }
    
        public void Show() => graphicsParent.SetActive(true);
        public void Hide() => graphicsParent.SetActive(false);
    }
}