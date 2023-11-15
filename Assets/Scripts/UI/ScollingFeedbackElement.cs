using System;
using TMPro;
using UnityEngine;

public class ScrollingFeedbackElement : MonoBehaviour
{
    public TextMeshProUGUI ScrollingFeedbackTM;

    private float time;
    private RectTransform rectTransform;
    private float startPositionX;
    private float startPositionY;
    private float horizontalVelocity;
    private float verticalVelocity = 2f;
    private float verticalAcceleration = -6.0f;

    public void Initialize(string _text, float _startPositionX, float _startPositionY, float _horizontalVelocity, Color _textColor, float _fontSize)
    {
        ScrollingFeedbackTM.text = _text;
        ScrollingFeedbackTM.fontSize = _fontSize;
        ScrollingFeedbackTM.color = _textColor;
        startPositionX = _startPositionX;
        startPositionY = _startPositionY;
        horizontalVelocity = _horizontalVelocity;
    }

    private void Start()
    {
        time = Registry.gameSettings.scrollingFeedbackLifetime;
        rectTransform = GetComponent<RectTransform>();
        
        Vector2 position = rectTransform.anchoredPosition;

        position.x = startPositionX;
        position.y = startPositionY;

        rectTransform.anchoredPosition = position;
    }
    
    private void Update()
    {
        Vector2 position = rectTransform.anchoredPosition;

        position.x += horizontalVelocity * Time.deltaTime;
        verticalVelocity += verticalAcceleration * Time.deltaTime;
        position.y += verticalVelocity * Time.deltaTime;

        rectTransform.anchoredPosition = position;

        time -= Time.deltaTime;
        if (time <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}