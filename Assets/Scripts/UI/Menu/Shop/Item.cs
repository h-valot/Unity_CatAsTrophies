using TMPro;
using UnityEngine;

namespace UI.Shop
{
    public class Item : MonoBehaviour
    {
        [Header("REFERENCES")] 
        public ShopbuttonManager shopbuttonManager;
        public TextMeshProUGUI titleTM;
        public TextMeshProUGUI priceTM;
        public TextMeshProUGUI givenCurrencyTM;

        [Header("SETTINGS")]
        public Currency currency;
        
        public void Buy()
        {
            if (givenCurrencyTM  != null) shopbuttonManager.Buy(titleTM.text, priceTM.text, currency, givenCurrencyTM.text);
            else shopbuttonManager.Buy(titleTM.text, priceTM.text, currency);
        }
    }
}