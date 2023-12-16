using System;
using Data;
using DG.Tweening;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectionUIItem : MonoBehaviour
{
    [Header("REFERENCES")] 
    public GameObject graphicsParent;
    public Image catImage;
    public Image blackImage;
    public TextMeshProUGUI countTM;

    [Header("DEBUGGING")]
    public GameObject canvasParent;
    public bool isInDeck;
    
    public Action onItemDragBegin;
    public Action onItemDragEnd;
    public Action<Item> onItemSelected;
    
    [HideInInspector] public Item item;
    
    public void Initialize(Item item, Action onItemDragBegin, Action onItemDragEnd, Action<Item> onItemSelected, GameObject canvasParent, bool isInDeck)
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

    public void UpdateGraphics()
    {
        graphicsParent.SetActive(true);
        catImage.sprite = Registry.entitiesConfig.cats[item.entityIndex].sprite;
        countTM.text = $"{item.cats.Count}";
        
        if (item.cats.Count == 0)
        {
            blackImage.DOFade(0.5f, 0);
             if (isInDeck)
             {
                 graphicsParent.SetActive(false);
                 transform.SetAsLastSibling();
             }
        }
        else
        {
            blackImage.DOFade(0, 0);
        }
    }

    public void Drag()
    {
        countTM.text = "1";
    }

    public void Undrag()
    {
        countTM.text = $"{item.cats.Count}";
    }
    
    public void Press()
    {
        onItemSelected?.Invoke(item);
    }
}