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
    public CollectionUIModel itemModel;    
    public Sprite epicBackground, commonBackground;
    public RSE_CollectionDragBox rseCollectionDragBox;

    [Header("INFORMATION")] 
    public Image catBackgroundImage;
    public Image catFaceImage;
    public TextMeshProUGUI nameTM;
    public TextMeshProUGUI amountTM;
    public TextMeshProUGUI raretyTM;
    public TextMeshProUGUI hpTM;
    public TextMeshProUGUI abilityTM;

    [Header("DECK")] 
    public TextMeshProUGUI deckAmountTM;
    public Image deckHighlightImage;
    public List<CollectionUIItem> deckItems = new List<CollectionUIItem>();
    
    [Header("COLLECTION")] 
    public TextMeshProUGUI collectionAmountTM;
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
        InitializeDeckItems();
        InitializeCollectionItems();
        UpdateInformation(collectionItems[0].item);
        UpdateAmounts();
        canvasParent.SetActive(true);
    }

    public void Hide()
    {
        canvasParent.SetActive(false);
        ClearDeckItems();
        ClearCollectionItems();
    }

    private void InitializeDeckItems()
    {
        for (int index = 0; index < deckItems.Count; index++)
        {
            var newItem = index < DataManager.data.playerStorage.deck.Count ? DataManager.data.playerStorage.deck[index] : null;
            deckItems[index].SetItem(newItem);
            deckItems[index].Initialize(onItemDragBegin, onItemDragEnd, onItemSelected, itemModel, true);
        }
    }
    
    private void InitializeCollectionItems()
    {
        for (int index = 0; index < collectionItems.Count; index++)
        {
            var newItem = index < DataManager.data.playerStorage.collection.Count ? DataManager.data.playerStorage.collection[index] : null;
            collectionItems[index].SetItem(newItem);
            collectionItems[index].Initialize(onItemDragBegin, onItemDragEnd, onItemSelected, itemModel, false);
        }
    }

    private void ClearDeckItems()
    {
        int deckItemCount = deckItems.Count;
        for (var index = 0; index < deckItemCount; index++)
        {
            deckItems[index].SetItem(null);
        }
    }
    
    private void ClearCollectionItems()
    {
        int collectionItemCount = collectionItems.Count;
        for (var index = 0; index < collectionItemCount; index++)
        {
            collectionItems[index].SetItem(null);
        }
    }

    private void OnEnable()
    {
        rseCollectionDragBox.action += UpdateAll;
        onItemDragBegin += HighlightDragZones;
        onItemDragEnd += HideDragZones;
        onItemDragEnd += UpdatePositions;
        onItemDragEnd += UpdateAmounts;
        onItemSelected += UpdateInformation;
    }

    private void OnDisable()
    {
        rseCollectionDragBox.action -= UpdateAll;
        onItemDragBegin -= HighlightDragZones;
        onItemDragEnd -= HideDragZones;
        onItemDragEnd -= UpdatePositions;
        onItemDragEnd -= UpdateAmounts;
        onItemSelected -= UpdateInformation;
    }

    private void HighlightDragZones(bool isInDeck)
    {
        if (isInDeck)
        {
            collectionHighlightImage.DOFade(0.01f, 0);
            
            _collectionSequence = DOTween.Sequence();
            _collectionSequence.SetLoops(-1);
            _collectionSequence.Append(collectionHighlightImage.DOFade(0.85f, highlightCycleDuration / 2).SetEase(Ease.InOutQuad));
            _collectionSequence.Append(collectionHighlightImage.DOFade(0.01f, highlightCycleDuration / 2).SetEase(Ease.InOutQuad));
        }
        else
        {
            deckHighlightImage.DOFade(0.01f, 0);
        
            _deckSequence = DOTween.Sequence();
            _deckSequence.SetLoops(-1);
            _deckSequence.Append(deckHighlightImage.DOFade(0.85f, highlightCycleDuration / 2).SetEase(Ease.InOutQuad));
            _deckSequence.Append(deckHighlightImage.DOFade(0.01f, highlightCycleDuration / 2).SetEase(Ease.InOutQuad));
        }
    }

    private void HideDragZones(bool isInDeck)
    {
        if (isInDeck)
        {
            _collectionSequence.Kill();
            collectionHighlightImage.DOFade(0.01f, 0);
        }
        else
        {
            _deckSequence.Kill();
            deckHighlightImage.DOFade(0.01f, 0);
        }
    }

    /// <summary>
    /// Updates position of items ui in the deck to properly hide empty one
    /// </summary>
    private void UpdatePositions(bool isInDeck)
    {
        foreach (var item in deckItems)
            if (item.item == null) item.transform.SetAsLastSibling();
    }

    private void UpdateAmounts(bool isInDeck = false)
    {
        deckAmountTM.text = $"{DataManager.data.playerStorage.GetLenght(DataManager.data.playerStorage.deck)}/{Registry.playerConfig.deckMaxLengh}";
        collectionAmountTM.text = $"{DataManager.data.playerStorage.GetLenght(DataManager.data.playerStorage.collection)}";
    }

    private void UpdateAll(bool isInDeck, int entityIndex)
    {
        ClearDeckItems();
        InitializeDeckItems();
        
        ClearCollectionItems();
        InitializeCollectionItems();
        
        UpdateAmounts();
    }

    /// <summary>
    /// Updates information panel with the given item infos
    /// </summary>
    private void UpdateInformation(Item item)
    {
        if (item == null) return;
        
        catBackgroundImage.sprite = Registry.entitiesConfig.cats[item.entityIndex].rarety == Rarety.EPIC ? epicBackground : commonBackground;
        catFaceImage.sprite = Registry.entitiesConfig.cats[item.entityIndex].sprite;
        
        nameTM.text = Registry.entitiesConfig.cats[item.entityIndex].entityName;
        amountTM.text = $"Amount: {item.cats.Count}";
        raretyTM.text = $"{Registry.entitiesConfig.cats[item.entityIndex].rarety.ToString().ToLower()}";
        hpTM.text = $"{Registry.entitiesConfig.cats[item.entityIndex].health}";
        abilityTM.text = Registry.entitiesConfig.cats[item.entityIndex].abilityDescription;
    }
}