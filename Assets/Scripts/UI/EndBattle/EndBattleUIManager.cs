using System.Threading.Tasks;
using Data;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UI.Reward;
using UnityEngine.UI;

namespace UI.EndBattle
{
    public class EndBattleUIManager : MonoBehaviour
    {
        [Header("EXTERNAL REFERENCES")]
        public MapManager mapManager;
    
        [Header("END TITLE")] 
        public GameObject endTitleParent;
        public TextMeshProUGUI endTitleTM;
        public Image endTitleLoadingImage;
        public float endTitleDuration;

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

            endTitleTM.text = $"{DataManager.data.endBattleStatus}";

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
    }
}