using TMPro;
using UI.Currencies;
using UnityEngine;

namespace UI.Shop
{
    public class Item : MonoBehaviour
    {
        [Header("REFERENCES")] 
        public ShopManager shopManager;
        public TextMeshProUGUI titleTM;
        public TextMeshProUGUI priceTM;
        public TextMeshProUGUI givenCurrencyTM;

        [Header("SETTINGS")]
        public Currency currency;
        
        public void Buy()
        {
            if (givenCurrencyTM  != null) shopManager.Buy(titleTM.text, priceTM.text, currency, givenCurrencyTM.text);
            else shopManager.Buy(titleTM.text, priceTM.text, currency);
        }
    }
}