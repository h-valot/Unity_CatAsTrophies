using TMPro;
using UnityEngine;

namespace UI.GameBattle
{
    public class ScrollingFeedbackElement : MonoBehaviour
    {
        public TextMeshProUGUI scrollingFeedbackTM;

        private float _time;
        private RectTransform _rectTransform;
        private float _startPositionX;
        private float _startPositionY;
        private float _horizontalVelocity;
        private float _verticalVelocity = 2f;
        private readonly float _verticalAcceleration = -6.0f;

        public void Initialize(string text, float startPositionX, float startPositionY, float horizontalVelocity, Color textColor, float fontSize)
        {
            _startPositionX = startPositionX;
            _startPositionY = startPositionY;
            _horizontalVelocity = horizontalVelocity;
        
            scrollingFeedbackTM.text = text;
            scrollingFeedbackTM.fontSize = fontSize;
            scrollingFeedbackTM.color = textColor;
        }

        private void Start()
        {
            _time = Registry.gameSettings.scrollingFeedbackLifetime;
            _rectTransform = GetComponent<RectTransform>();
        
            Vector2 position = _rectTransform.anchoredPosition;

            position.x = _startPositionX;
            position.y = _startPositionY;

            _rectTransform.anchoredPosition = position;
        }
    
        private void Update()
        {
            Vector2 position = _rectTransform.anchoredPosition;

            position.x += _horizontalVelocity * Time.deltaTime;
            _verticalVelocity += _verticalAcceleration * Time.deltaTime;
            position.y += _verticalVelocity * Time.deltaTime;

            _rectTransform.anchoredPosition = position;

            _time -= Time.deltaTime;
            if (_time <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}