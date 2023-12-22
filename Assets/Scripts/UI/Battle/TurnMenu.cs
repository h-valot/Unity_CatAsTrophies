using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace UI.Battle
{
    public class TurnMenu : MonoBehaviour
    {
        [Header("REFERENCES")]
        public GameObject turnTitleGO;
        public TextMeshProUGUI turnTitleTM;
        public float turnTitleDuration;
    
        public async Task AnimateTitle(string message)
        {
            // enable
            var titleColor = turnTitleTM.color;
            turnTitleTM.DOColor(new Color(titleColor.r, titleColor.g, titleColor.b, 1), 0);
            turnTitleGO.SetActive(true);
            turnTitleGO.transform.DOScale(1.4f, 0.1f).SetEase(Ease.OutBack);
            turnTitleGO.transform.DOScale(1f, 0.2f).SetEase(Ease.InBack);
            
        
            // update dispay
            turnTitleTM.text = message;
        
            // disable
            turnTitleTM.DOColor(new Color(titleColor.r, titleColor.g, titleColor.b, 0), turnTitleDuration).SetEase(Ease.InExpo);
            await Task.Delay(Mathf.RoundToInt(turnTitleDuration * 1000));
            turnTitleGO.SetActive(false);
        }
    }
}