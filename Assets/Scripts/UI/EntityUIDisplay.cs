using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EntityUIDisplay : MonoBehaviour
{
    [Header("REFERENCES")] 
    public BattlePawn battlePawn;
    public GameObject effectPrefab;
    public Transform layoutGroup;
    
    [Header("UI DISPLAY")] 
    public GameObject canvasGO;
    public Image healthFillImage;
    public TextMeshProUGUI healthTM;
    public TextMeshProUGUI armorTM;
    public GameObject armorParent;
    public GameObject AttackIntent;

    private List<GameObject> effectDisplays = new List<GameObject>();
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
            canvasGO.SetActive(true);

            entityRef.OnStatsUpdate += UpdateDisplay;
            entityRef.onIntentUpdate += UpdateDisplay;
            UpdateDisplay();
        }
        else
        {
            entityRef.OnStatsUpdate -= UpdateDisplay;

            canvasGO.SetActive(false);
            entityRef = null;
        }
    }

    private void UpdateDisplay()
    {
        // get if the entity is a cat or an enemy
        if (battlePawn.TryGetComponent(out Cat cat))
        {
            canvasGO.gameObject.SetActive(cat.state == CatState.OnBattle);
        }
        
        if (battlePawn.TryGetComponent(out Enemy enemy))
        {
            canvasGO.gameObject.SetActive(enemy.health > 0);
        }

        // exit if the canvas isn't active in the hierarchy
        if (!canvasGO.activeInHierarchy) return;
        
        // update ui displays
        healthFillImage.fillAmount = (float)entityRef.health / (float)entityRef.maxHealth;
        healthTM.text = $"{entityRef.health}/{entityRef.maxHealth}";

        // update armor display
        if (entityRef.armor > 0)
        {
            armorParent.SetActive(true);
            armorTM.text = entityRef.armor.ToString();
        }
        else
        {
            armorParent.SetActive(false);
        }
        
        // destroy all effects display
        int effectDisplayCount = effectDisplays.Count;
        for (int index = effectDisplayCount - 1; index >= 0; index--)
        {
            Destroy(effectDisplays[index]);
        }

        // instantiate all effects
        foreach (var effect in entityRef.effects)
        {
            var newEffectDisplay = Instantiate(effectPrefab, layoutGroup).GetComponent<EffectUIDiplay>();
            newEffectDisplay.UpdateDisplay(effect.type, effect.turnDuration);
            effectDisplays.Add(newEffectDisplay.gameObject);
        }

        // Update the Intent
        



    }
}