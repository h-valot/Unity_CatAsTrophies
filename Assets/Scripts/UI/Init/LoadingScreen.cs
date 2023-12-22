using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Init
{
    public class LoadingScreen : MonoBehaviour
    {
        [Header("REFERENCES")] 
        public GameObject company;
        public GameObject loading;

        [Header("COMPANY")]
        public Image companyLogo;
        public float companyDuration;
    
        [Header("LOADING")] 
        public Image fillImage;
        public float loadingDuration;
        public AnimationCurve loadingCurve;
    
        public async Task Animate()
        {
            HideAll();
        
            company.SetActive(true);
            companyLogo.DOFade(1, companyDuration);
            await Task.Delay(Mathf.RoundToInt(1000 * companyDuration));
            company.SetActive(false);
        
            loading.SetActive(true);
            fillImage.DOFillAmount(1f, loadingDuration).SetEase(loadingCurve);
            await Task.Delay(Mathf.RoundToInt(1000 * loadingDuration));
            loading.SetActive(false);
        }

        private void HideAll()
        {
            companyLogo.DOFade(0, 0);
            company.SetActive(false);

            fillImage.fillAmount = 0f;
            loading.SetActive(false);
        }
    }
}