using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoldUIButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("REFERENCES")] 
    public GameObject graphicsParent;
    public Image blackImage;
    public Image holdingImage;
    
    [Header("SETTINGS")] 
    public float holdingTime;
    public Vector3 scaleDownValue;
    
    [Space(10)]
    public UnityEvent OnClick;

    private bool _canRunTimer;
    private bool _holdEnough;
    private float _currentTime;
    private Sequence _circleAnimationSequence;
    
    public void OnPointerDown(PointerEventData data)
    {
        _canRunTimer = true;
        AnimateButton();
    }
    
    public void OnPointerUp(PointerEventData data)
    {
        // lock and reset timer
        _canRunTimer = false;
        _currentTime = 0f;
        
        CancelButtonAnimation();
        
        // triggers the event, if the player has held long enough 
        if (_holdEnough)
        {
            _holdEnough = false;
            OnClick.Invoke();
        }
    }

    private void Update()
    {
        // exit, if can't run the timer
        if (!_canRunTimer) return;

        // update the timer
        _currentTime += Time.deltaTime;
        if (_currentTime >= holdingTime)
        {
            _holdEnough = true;
        }
    }

    /// <summary>
    /// Animates on button pressed
    /// </summary>
    private void AnimateButton()
    {
        graphicsParent.transform.DOScale(scaleDownValue, 0);
        blackImage.DOFade(0.75f, 0);
        holdingImage.gameObject.SetActive(true);
        _circleAnimationSequence.Append(holdingImage.DOFillAmount(1, holdingTime));
    }

    /// <summary>
    /// Cancel all animations
    /// </summary>
    private void CancelButtonAnimation()
    {
        graphicsParent.transform.DOScale(Vector3.one, 0);
        blackImage.DOFade(0, 0);
        holdingImage.DOKill();
        holdingImage.fillAmount = 0;
        holdingImage.gameObject.SetActive(false);
    }
}
