using TMPro;
using UnityEngine;

namespace UI.Shop
{
    public class TransactionUIManager : MonoBehaviour
    {
        [Header("REFERENCES")] 
        public GameObject graphicsParent;
        public RSO_CurrencyTuna rsoCurrencyTuna;
        public RSO_CurrencyTreat rsoCurrencyTreat;
        public RSE_DebugLog rseDebugLog;

        [Header("NOT ENOUGH")] 
        public ShopbuttonManager shopbuttonManager;
        public GameObject buyButton, coinsButton;

        [Header("TEXTS")] 
        public TextMeshProUGUI titleTM;
        public TextMeshProUGUI flavorTM;
        public TextMeshProUGUI confirmationTM;

        private string _name;
        private Currency _currency;
        private int _givenCurrency;
        private int _cost;
    
        public void UpdateGraphics(string name, string price, Currency currency, string givenCurrency = "")
        {
            _name = name;
            _currency = currency;
            if (int.TryParse(givenCurrency, out var given)) _givenCurrency = given;
            if (int.TryParse(price, out var cost)) _cost = cost;
        
            titleTM.text = $"{name}";
            confirmationTM.text = price;

            var canBuy = false;
        
            switch (currency)
            {
                case Currency.TUNA:
                    canBuy = rsoCurrencyTuna.value >= _cost;
                    flavorTM.text = canBuy ? $"New balance: {rsoCurrencyTuna.value - _cost} TUNA" : "Not enough";
                    break;
                case Currency.TREATS:
                    canBuy = rsoCurrencyTreat.value >= _cost;
                    flavorTM.text = canBuy ? $"New balance: {rsoCurrencyTreat.value - _cost} TREATS" : "Not enough";
                    break;
                case Currency.REAL:
                    canBuy = true;
                    flavorTM.text = "DEBUG: You can buy it.";
                    break;
            }

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
            shopbuttonManager.OpenCoinsPanel();
            Hide();
        }
    
        public void Buy()
        {
            switch (_currency)
            {
                case Currency.TUNA:
                    rsoCurrencyTuna.value -= _cost;
                    rseDebugLog.Call($"{_name} successfully bought!", Color.white);
                    break;
                case Currency.TREATS:
                    rsoCurrencyTreat.value -= _cost;
                    rseDebugLog.Call($"{_name} successfully bought!", Color.white);
                    break;
                case Currency.REAL:
                    rsoCurrencyTuna.value += _givenCurrency;
                    rseDebugLog.Call($"{_givenCurrency} tuna successfully added!", Color.white);
                    break;
            }
            Hide();
        }
    
        public void Show() => graphicsParent.SetActive(true);
        public void Hide() => graphicsParent.SetActive(false);
    }

    public enum Currency
    {
        TUNA = 0,
        TREATS,
        REAL
    }
}