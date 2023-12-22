using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace UI.MainMenu
{
    public class ButtonsAnimation : MonoBehaviour
    {
        [Header("REFERENCES")]
        public Transform[] buttons;
    
        [Header("TWEAKING")]
        public float translationDuration;
        public int offset;
    
        private async Task AnimateButtons()
        {
            foreach (Transform button in buttons)
            {
                var position = button.position;
                position = new Vector3(position.x, position.y - offset, position.z);
                button.position = position;
            }
        
            foreach (Transform button in buttons)
            {
                button.DOMoveY(button.position.y + offset, translationDuration).SetEase(Ease.OutBack);
                await Task.Delay(Mathf.RoundToInt(1000 * translationDuration / 2));
            }
        }

        public void Animate() => AnimateButtons();
    }
}