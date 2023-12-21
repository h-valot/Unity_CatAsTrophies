using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class MainMenuButtonAnimation : MonoBehaviour
{
    public Transform[] buttons;
    public float translationDuration;
    
    private async Task AnimateButtons()
    {
        foreach (Transform button in buttons)
        {
            var position = button.position;
            position = new Vector3(position.x, position.y - 200, position.z);
            button.position = position;
        }
        
        foreach (Transform button in buttons)
        {
            button.DOMoveY(button.position.y + 200, translationDuration).SetEase(Ease.OutBack);
            await Task.Delay(Mathf.RoundToInt(1000 * translationDuration / 2));
        }
    }

    public void Animate()
    {
        AnimateButtons();
    }
}
