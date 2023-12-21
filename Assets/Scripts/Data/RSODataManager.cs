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
            rsoCurrencyTuna.OnChanged += Save;
            rsoCurrencyTreat.OnChanged += Save;
        }

        private void OnDisable()
        {
            rsoCurrencyTuna.OnChanged -= Save;
            rsoCurrencyTreat.OnChanged -= Save;
        }

        private void Save()
        {
            DataManager.data.tuna = rsoCurrencyTuna.value;
            DataManager.data.treat = rsoCurrencyTreat.value;
        }

        public void Load()
        {
            rsoCurrencyTuna.value = DataManager.data.tuna;
            rsoCurrencyTreat.value = DataManager.data.treat;
        }
    }
}