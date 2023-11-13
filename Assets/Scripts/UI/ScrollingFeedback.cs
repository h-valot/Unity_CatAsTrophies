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
    public GameObject effectPrefab;


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
        if (battlePawn.entityIdLinked != "")
        {
            entityRef = Misc.GetEntityById(battlePawn.entityIdLinked);
            
            entityRef.OnDamageRecieved += UpdateDisplay;
        }
        else
        {
            entityRef.OnDamageRecieved -= UpdateDisplay;

            entityRef = null;
        }
    }

    private void UpdateDisplay(string text)
    {
        Debug.Log($"ScrollingFeedBack: {text}");
    }
}