using TMPro;
using UI.Currencies;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Shop
{
    public class ShopManager : MonoBehaviour
    {
        [Header("REFERENCES")] 
        public GameObject graphicsParent;
        public Image featuredTabImage, catsTabImage, boostsTabImage, coinsTabImage;
        public GameObject featuredPanel, catsPanel, boostsPanel, coinsPanel;
        public GameObject catsCollectionButton;
        public Transaction transaction;

        [Header("CURRENCY")]
        public RSO_CurrencyTuna rsoCurrencyTuna;
        public RSO_CurrencyTreat rsoCurrencyTreat;
        public TextMeshProUGUI tunaTM, treatTM;
    
        [Header("SETTINGS")] 
        public Color unselectedTabColor;

        public void ShowNormal()
        {
            Show();
            catsCollectionButton.SetActive(true);
            OpenFeaturedPanel();
        }
    
        public void ShowOnCoins()
        {
            Show();
            catsCollectionButton.SetActive(false);
            OpenCoinsPanel();
        }

        private void Show() => graphicsParent.SetActive(true);
        public void Hide() => graphicsParent.SetActive(false);

        private void OnEnable()
        {
            rsoCurrencyTuna.OnChanged += UpdateCurrencies;
            rsoCurrencyTreat.OnChanged += UpdateCurrencies;
            UpdateCurrencies();
        }

        private void OnDisable()
        {
            rsoCurrencyTuna.OnChanged -= UpdateCurrencies;
            rsoCurrencyTreat.OnChanged -= UpdateCurrencies;
        }

        private void UpdateCurrencies()
        {
            tunaTM.text = $"{rsoCurrencyTuna.value}";
            treatTM.text = $"{rsoCurrencyTreat.value}";
        }

        public void Buy(string name, string price, Currency currency, string givenCurrency = "")
        {
            transaction.Show();
            transaction.UpdateGraphics(name, price, currency, givenCurrency);
        }
    
        public void OpenFeaturedPanel()
        {
            HideAllPanels();
            SetAllTabsUnselected();
        
            featuredPanel.SetActive(true);
            featuredTabImage.color = Color.white;
        }
    
        public void OpenCatsPanel()
        {
            HideAllPanels();
            SetAllTabsUnselected();
        
            catsPanel.SetActive(true);
            catsTabImage.color = Color.white;
        }
    
        public void OpenBoostsPanel()
        {
            HideAllPanels();
            SetAllTabsUnselected();
        
            boostsPanel.SetActive(true);
            boostsTabImage.color = Color.white;
        }
    
        public void OpenCoinsPanel()
        {
            HideAllPanels();
            SetAllTabsUnselected();
        
            coinsPanel.SetActive(true);
            coinsTabImage.color = Color.white;
        }

        private void SetAllTabsUnselected()
        {
            featuredTabImage.color = unselectedTabColor;
            catsTabImage.color = unselectedTabColor;
            boostsTabImage.color = unselectedTabColor;
            coinsTabImage.color = unselectedTabColor;
        }

        private void HideAllPanels()
        {
            featuredPanel.SetActive(false);
            catsPanel.SetActive(false);
            boostsPanel.SetActive(false);
            coinsPanel.SetActive(false);
        }
    }
}