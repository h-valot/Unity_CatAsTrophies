using System;
using System.Threading.Tasks;
using Data;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UI.Reward;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace UI.EndBattle
{
    public class EndBattleUIManager : MonoBehaviour
    {
        [Header("EXTERNAL REFERENCES")]
        public MapManager mapManager;

        [Header("REFERENCES")] 
        public RSO_CurrencyTreat rsoCurrencyTreat;
        
        [Header("END TITLE")] 
        public GameObject endTitleParent;
        public TextMeshProUGUI endTitleTM;
        public Image endTitleLoadingImage;
        public float endTitleDuration;
        public GameObject treatParent;
        public TextMeshProUGUI treatTM;

        [Header("REWARD")] 
        public RewardUIManager rewardUIManager;

        /// <summary>
        /// Shows end battle screen
        /// </summary>
        public async Task AnimateEndBattle()
        {
            await AnimateEndTitle();
            await AnimateEndMap();
            await AnimateEndReward();
        }
    
        public async Task AnimateEndTitle()
        {
            endTitleParent.SetActive(true);
            treatParent.SetActive(false);
            
            endTitleTM.text = $"{DataManager.data.endBattleStatus}";

            if (DataManager.data.endBattleStatus == EndBattleStatus.VICTORY)
            {
                int treatWon = GetTreats();
                treatParent.SetActive(true);
                treatTM.text = $"+{treatWon}";
                rsoCurrencyTreat.value += treatWon;

            }
            
            endTitleLoadingImage.DOFillAmount(1, endTitleDuration);
            await Task.Delay(Mathf.RoundToInt(1000 * endTitleDuration));
        
            endTitleParent.SetActive(false);
        }

        private async Task AnimateEndMap()
        {
            mapManager.ShowCanvasLocked();
        }
    
        private async Task AnimateEndReward()
        {
            rewardUIManager.Show();
            rewardUIManager.UpdateDisplay();
        }

        private int GetTreats()
        {
            var output = 0;
            switch (DataManager.data.compoToLoad.tier)
            {
                case CompositionTier.SIMPLE:
                    output = Random.Range(10, 30);
                    break;
                case CompositionTier.ELITE:
                    output = Random.Range(26, 34);
                    break;
                case CompositionTier.BOSS:
                    output = Random.Range(92, 104);
                    break;
            }
            return output;
        }
    }
}