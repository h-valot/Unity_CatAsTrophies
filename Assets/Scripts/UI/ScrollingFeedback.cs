using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class ScollingFeedback : MonoBehaviour
{
    [Header("REFERENCES")] 
    public BattlePawn battlePawn;
    public ScrollingFeedbackElement scrollingFeedbackElementPrefab;


    private Entity entityRef;
    
    public void OnEnable()
    {
        battlePawn.OnEntityUpdated += UpdateEntityRef;
    }
    
    public void OnDisable()
    {
        battlePawn.OnEntityUpdated -= UpdateEntityRef;
    }

    private void UpdateEntityRef()
    {
        if (battlePawn.entityIdLinked == "")
        {
            entityRef.OnStatusRecieved -= CreateScrollingFeedbackElement;
            entityRef = null;
        }
        else if(entityRef == null)
        {
            entityRef = Misc.GetEntityById(battlePawn.entityIdLinked);
            entityRef.OnStatusRecieved += CreateScrollingFeedbackElement;
        }
        else
        {
            entityRef.OnStatusRecieved -= CreateScrollingFeedbackElement;
            entityRef = Misc.GetEntityById(battlePawn.entityIdLinked);
            entityRef.OnStatusRecieved += CreateScrollingFeedbackElement;
        }
    }

    private void CreateScrollingFeedbackElement (string _text, Color _textColor, bool _isEffect)
    {
        float _startPositionX = UnityEngine.Random.Range(-0.4f, 0.4f);
        float _startPositionY = UnityEngine.Random.Range(-0.3f, 0.5f);
        float _horizontalVelocity = UnityEngine.Random.Range(0.2f, 1.0f) * (UnityEngine.Random.Range(0, 2) * 2 - 1); //random between -1 and -0.2 or 0.2 and 1

        float _fontSize = Registry.gameSettings.defaultFontSize;
        if (_isEffect)
        {
            _fontSize = Registry.gameSettings.effectFontSize;
        }

        var newEffectDisplay = Instantiate(scrollingFeedbackElementPrefab, gameObject.transform);
        newEffectDisplay.Initialize(_text, _startPositionX, _startPositionY, _horizontalVelocity, _textColor, _fontSize);
    }
}