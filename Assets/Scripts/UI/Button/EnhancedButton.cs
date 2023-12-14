using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EnhancedButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("REFERENCES")] 
    [Tooltip("It will be scaled down on pointer down and reset to normal on pointer up")]
    public GameObject graphicsParent;
    [Tooltip("It will be darken on pointer down and hide on pointer up")]
    public Image image = null;
    
    [Header("SETTINGS")]
    public float scaleDownMultiplier;
    public float animationDuration = 0.1f;
    
    [Space(10)]
    public UnityEvent OnClick;

    public void OnPointerDown(PointerEventData data)
    {
        AnimateButton();
    }
    
    public async void OnPointerUp(PointerEventData data)
    {
        await CancelButtonAnimation();
        OnClick.Invoke();
    }

    /// <summary>
    /// Animates on button pressed
    /// </summary>
    private void AnimateButton()
    {
        if (image != null) image.DOFade(0.75f, 0);
        graphicsParent.transform.DOScale(scaleDownMultiplier, animationDuration).SetEase(Ease.OutBack);
    }

    /// <summary>
    /// Cancel all animations
    /// </summary>
    private async Task CancelButtonAnimation()
    {
        graphicsParent.transform.DOScale(Vector3.one, animationDuration).SetEase(Ease.OutBack);
        await Task.Delay(Mathf.RoundToInt(1000 * animationDuration));
        if (image != null) image.DOFade(0, 0);
    }
}
