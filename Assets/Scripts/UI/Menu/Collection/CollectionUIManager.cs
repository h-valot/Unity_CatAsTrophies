using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using DG.Tweening;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectionUIManager : MonoBehaviour
{
    [Header("REFERENCES")] 
    public GameObject canvasParent;
    public CollectionUIItem itemPrefab;
    public Sprite epicBackground, commonBackground;

    [Header("INFORMATION")] 
    public Image catBackgroundImage;
    public Image catFaceImage;
    public TextMeshProUGUI nameTM;
    public TextMeshProUGUI amountTM;
    public TextMeshProUGUI raretyTM;
    public TextMeshProUGUI hpTM;
    public TextMeshProUGUI abilityTM;
    
    [Header("DECK")] public Transform deckContentTransform;
    public Image deckHighlightImage;
    public List<CollectionUIItem> deckItems = new List<CollectionUIItem>();
    
    [Header("COLLECTION")] public Transform collectionContentTransform;
    public Image collectionHighlightImage;
    public List<CollectionUIItem> collectionItems = new List<CollectionUIItem>();
    
    [Header("ANIMATION")] 
    public float highlightCycleDuration;
    
    public Action<bool> onItemDragBegin;
    public Action<bool> onItemDragEnd;
    public Action<Item> onItemSelected;
    private Sequence _deckSequence;
    private Sequence _collectionSequence;

    public void Show()
    {
        GenerateItems();
        UpdateInformation(collectionItems[0].item);
        canvasParent.SetActive(true);
    }

    public void Hide()
    {
        canvasParent.SetActive(false);
        ClearItems();
    }

    private void GenerateItems()
    {
        foreach (var item in DataManager.data.playerStorage.deck)
        {
            var newItem = Instantiate(itemPrefab, deckContentTransform);
            newItem.Initialize(item, onItemDragBegin, onItemDragEnd, onItemSelected, canvasParent, true);
            newItem.UpdateGraphics();
            deckItems.Add(newItem);
        }
        
        foreach (var item in DataManager.data.playerStorage.collection)
        {
            var newItem = Instantiate(itemPrefab, collectionContentTransform);
            newItem.Initialize(item, onItemDragBegin, onItemDragEnd, onItemSelected, canvasParent, false);
            newItem.UpdateGraphics();
            collectionItems.Add(newItem);
        }
    }

    private void ClearItems()
    {
        int deckItemCount = deckItems.Count;
        for (var index = 0; index < deckItemCount; index++)
        {
            Destroy(deckItems[index].gameObject);
        }

        deckItems.Clear();
        
        int collectionItemCount = collectionItems.Count;
        for (var index = 0; index < collectionItemCount; index++)
        {
            Destroy(collectionItems[index].gameObject);
        }

        collectionItems.Clear();
    }

    private void OnEnable()
    {
        onItemDragBegin += HighlightDragZones;
        onItemDragEnd += HideDragZones;
        onItemDragEnd += UpdatePositions;
        onItemSelected += UpdateInformation;
    }

    private void OnDisable()
    {
        onItemDragBegin -= HighlightDragZones;
        onItemDragEnd -= HideDragZones;
        onItemDragEnd -= UpdatePositions;
        onItemSelected -= UpdateInformation;
    }

    private void HighlightDragZones(bool isInDeck)
    {
        if (isInDeck)
        {
            collectionHighlightImage.DOFade(0, 0);
            
            _collectionSequence = DOTween.Sequence();
            _collectionSequence.SetLoops(-1);
            _collectionSequence.Append(collectionHighlightImage.DOFade(1, highlightCycleDuration / 2).SetEase(Ease.InOutQuad));
            _collectionSequence.Append(collectionHighlightImage.DOFade(0, highlightCycleDuration / 2).SetEase(Ease.InOutQuad));
        }
        else
        {
            deckHighlightImage.DOFade(0, 0);
        
            _deckSequence = DOTween.Sequence();
            _deckSequence.SetLoops(-1);
            _deckSequence.Append(deckHighlightImage.DOFade(1, highlightCycleDuration / 2).SetEase(Ease.InOutQuad));
            _deckSequence.Append(deckHighlightImage.DOFade(0, highlightCycleDuration / 2).SetEase(Ease.InOutQuad));
        }
    }

    private void HideDragZones(bool isInDeck)
    {
        if (isInDeck)
        {
            _collectionSequence.Kill();
            collectionHighlightImage.DOFade(0, 0);
        }
        else
        {
            _deckSequence.Kill();
            deckHighlightImage.DOFade(1, 0);
        }
    }

    /// <summary>
    /// Updates position of items ui in the deck to properly hide empty one
    /// </summary>
    private void UpdatePositions(bool isInDeck)
    {
        // foreach (var item in deckItems.Where(item => item.graphicsParent.activeInHierarchy == false))
        //     item.transform.SetAsLastSibling();
    }

    /// <summary>
    /// Updates information panel with the given item infos
    /// </summary>
    private void UpdateInformation(Item item)
    {
        catBackgroundImage.sprite = Registry.entitiesConfig.cats[item.entityIndex].rarety == Rarety.EPIC ? epicBackground : commonBackground;
        catFaceImage.sprite = Registry.entitiesConfig.cats[item.entityIndex].sprite;
        
        nameTM.text = Registry.entitiesConfig.cats[item.entityIndex].entityName;
        amountTM.text = $"Amount: {item.cats.Count}";
        raretyTM.text = $"{Registry.entitiesConfig.cats[item.entityIndex].rarety.ToString().ToLower()}";
        hpTM.text = $"{Registry.entitiesConfig.cats[item.entityIndex].health}";
        abilityTM.text = Registry.entitiesConfig.cats[item.entityIndex].abilityDescription;
    }
}