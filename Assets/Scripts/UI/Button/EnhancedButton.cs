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
    public float scaleDownMultiplier = 0.8f;
    
    [Space(10)]
    public UnityEvent OnClick;

    public void OnPointerDown(PointerEventData data)
    {
        AnimateButton();
    }
    
    public void OnPointerUp(PointerEventData data)
    {
        CancelButtonAnimation();
        OnClick.Invoke();
    }

    /// <summary>
    /// Animates on button pressed
    /// </summary>
    private void AnimateButton()
    {
        graphicsParent.transform.DOScale(scaleDownMultiplier, 0.1f).SetEase(Ease.OutBack);
        if (image != null) image.DOFade(0.75f, 0);
    }

    /// <summary>
    /// Cancel all animations
    /// </summary>
    private void CancelButtonAnimation()
    {
        graphicsParent.transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.OutBack);
        if (image != null) image.DOFade(0, 0);
    }
}
