using System;
using DG.Tweening;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectionUIItem : MonoBehaviour
{
    [Header("REFERENCES")] 
    public GameObject graphicsParent;
    public Sprite epicBackground, commonBackground;
    public Image backgroundImage, faceImage;
    public Image blackImage;
    public TextMeshProUGUI countTM;

    [Header("DEBUGGING")]
    public GameObject canvasParent;
    public bool isInDeck;
    
    public Action<bool> onItemDragBegin;
    public Action<bool> onItemDragEnd;
    public Action<Item> onItemSelected;
    
    [HideInInspector] public Item item;
    
    public void Initialize(Item item, Action<bool> onItemDragBegin, Action<bool> onItemDragEnd, Action<Item> onItemSelected, GameObject canvasParent, bool isInDeck)
    {
        this.item = item;
        this.onItemDragBegin = onItemDragBegin;
        this.onItemDragEnd = onItemDragEnd;
        this.onItemSelected = onItemSelected;
        this.canvasParent = canvasParent;
        this.isInDeck = isInDeck;
        
        this.item.onDataChanged += UpdateGraphics;
        graphicsParent.SetActive(true);
    }

    private void OnDestroy()
    {
        item.onDataChanged -= UpdateGraphics;
    }

    /// <summary>
    /// Updates graphics according to the item reference
    /// </summary>
    public void UpdateGraphics()
    {
        // updates information
        backgroundImage.sprite = Registry.entitiesConfig.cats[item.entityIndex].rarety == Rarety.EPIC ? epicBackground : commonBackground;
        faceImage.sprite = Registry.entitiesConfig.cats[item.entityIndex].sprite;
        countTM.text = $"{item.cats.Count}";
        
        // updates graphics visibility
        graphicsParent.SetActive(true);
        blackImage.DOFade(0, 0);
        
        // hides item ui, if it's empty
        if (item.cats.Count == 0)
        {
            blackImage.DOFade(0.5f, 0);
             if (isInDeck)
             {
                 Debug.Log("COLLECTION UI ITEM: item ui set as last sibling");
                 graphicsParent.SetActive(false);
                 transform.SetAsLastSibling();
             }
        }
    }

    /// <summary>
    /// Set graphics cat counter amount to the given parameter
    /// </summary>
    public void SetGraphicsCatAmount(int amount)
    {
        countTM.text = $"{amount}";
    }
    
    public void Press()
    {
        onItemSelected?.Invoke(item);
    }
}