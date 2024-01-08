using UnityEngine;

namespace Data
{
    public class RSODataManager : MonoBehaviour
    {
        [Header("REFERENCES")]
        public RSO_CurrencyTuna rsoCurrencyTuna;
        public RSO_CurrencyTreat rsoCurrencyTreat;
        
        private void Awake() => DontDestroyOnLoad(this);

        private void OnEnable()
        {
            rsoCurrencyTuna.OnChanged += SaveTuna;
            rsoCurrencyTreat.OnChanged += SaveTreat;
        }

        private void OnDisable()
        {
            rsoCurrencyTuna.OnChanged -= SaveTuna;
            rsoCurrencyTreat.OnChanged -= SaveTreat;
        }

        private void SaveTuna() => DataManager.data.tuna = rsoCurrencyTuna.value;
        private void SaveTreat() => DataManager.data.treat = rsoCurrencyTreat.value;
        private void OnApplicationQuit() => DataManager.Save();

        public void Load()
        {
            rsoCurrencyTuna.value = DataManager.data.tuna;
            rsoCurrencyTreat.value = DataManager.data.treat;
        }
    }
}