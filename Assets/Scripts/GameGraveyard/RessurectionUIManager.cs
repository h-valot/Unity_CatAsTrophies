using System;
using System.Collections.Generic;
using Data;
using Player;
using UnityEngine;

public class RessurectionUIManager : MonoBehaviour
{
    [Header("REFERENCES")]
    public GameObject graphicsParent;

    public Action onCatSelected;
    public int selectedCat;

    public List<Item> candidates = new List<Item>();
    
    public void Initialize()
    {
        Hide();
        onCatSelected += Select;
    }

    private void OnDisable()
    {
        onCatSelected -= Select;
    }

    private void GetAllDeadCats()
    {
        foreach (var item in DataManager.data.playerStorage.inGameDeck)
        {
            // continue, if the cat is alive
            if (!item.isDead) continue;
                
            candidates.Add(item);
        }
    }

    public void Select()
    {
        
    }
    
    public void Ressurect()
    {
        
    }

    public void Show() => graphicsParent.SetActive(true);
    public void Hide() => graphicsParent.SetActive(false);
}