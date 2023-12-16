using System;
using System.Collections.Generic;
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

    [Header("INFORMATION")]
    public TextMeshProUGUI catNameTM;
    public TextMeshProUGUI healthPointTM;
    public TextMeshProUGUI catAmountTM;
    public TextMeshProUGUI abilityTM;
    
    [Header("DECK")] public Transform deckContentTransform;
    public Image deckHighlightImage;
    public List<CollectionUIItem> deckItems = new List<CollectionUIItem>();
    
    [Header("COLLECTION")] public Transform collectionContentTransform;
    public Image collectionHighlightImage;
    public List<CollectionUIItem> collectionItems = new List<CollectionUIItem>();
    
    [Header("ANIMATION")] 
    public float highlightCycleDuration;
    
    public Action onItemDragBegin;
    public Action onItemDragEnd;
    public Action<Item> onItemSelected;

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
        onItemSelected += UpdateInformation;
    }

    private void OnDisable()
    {
        onItemDragBegin -= HighlightDragZones;
        onItemDragEnd -= HideDragZones;
        onItemSelected -= UpdateInformation;
    }

    private Sequence _deckSequence;
    private Sequence _collectionSequence;
    private void HighlightDragZones()
    {
        deckHighlightImage.DOFade(0, 0);
        collectionHighlightImage.DOFade(0, 0);
        
        _deckSequence = DOTween.Sequence();
        _deckSequence.SetLoops(-1);
        _deckSequence.Append(deckHighlightImage.DOFade(1, highlightCycleDuration / 2).SetEase(Ease.InOutQuad));
        _deckSequence.Append(deckHighlightImage.DOFade(0, highlightCycleDuration / 2).SetEase(Ease.InOutQuad));
        
        _collectionSequence = DOTween.Sequence();
        _collectionSequence.SetLoops(-1);
        _collectionSequence.Append(collectionHighlightImage.DOFade(1, highlightCycleDuration / 2).SetEase(Ease.InOutQuad));
        _collectionSequence.Append(collectionHighlightImage.DOFade(0, highlightCycleDuration / 2).SetEase(Ease.InOutQuad));
    }

    private void HideDragZones()
    {
        _deckSequence.Kill();
        _collectionSequence.Kill();
        
        deckHighlightImage.DOFade(1, 0);
        collectionHighlightImage.DOFade(0, 0);
    }

    private void UpdateInformation(Item item)
    {
        catNameTM.text = Registry.entitiesConfig.cats[item.entityIndex].entityName;
        healthPointTM.text = $"HP: {Registry.entitiesConfig.cats[item.entityIndex].health}";
        catAmountTM.text = $"Amount: {item.cats.Count}";
        abilityTM.text = Registry.entitiesConfig.cats[item.entityIndex].abilityDescription;
    }
}