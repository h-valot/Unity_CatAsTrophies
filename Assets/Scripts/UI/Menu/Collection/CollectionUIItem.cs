using Data;
using DG.Tweening;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectionUIItem : MonoBehaviour
{
    [Header("REFERENCES")] 
    public Image catImage;
    public Image blackImage;
    public TextMeshProUGUI countTM;

    private bool _isInDeck;
    private Item _item;

    public void Initialize(Item item, bool isInDeck)
    {
        _item = item;
        _isInDeck = isInDeck;
        _item.onCountChanged += UpdateGraphics;
    }

    private void OnDisable()
    {
        _item.onCountChanged -= UpdateGraphics;
    }

    public void UpdateGraphics()
    {
        catImage.sprite = Registry.entitiesConfig.cats[_item.entityIndex].sprite;
        countTM.text = $"{_item.Count}";

        if (_item.Count == 0) blackImage.DOFade(0.5f, 0);
        else blackImage.DOFade(0, 0);
    }
    
    public void Press()
    {
        // exit, if there is no more cats
        if (_item.Count == 0)
        {
            UpdateGraphics();
            return;
        }
        
        if (_isInDeck)
        {
            DataManager.data.playerStorage.Transfer(DataManager.data.playerStorage.deck, DataManager.data.playerStorage.collection, _item.entityIndex);
        }
        else
        {
            DataManager.data.playerStorage.Transfer(DataManager.data.playerStorage.collection, DataManager.data.playerStorage.deck, _item.entityIndex);
        }
        
        UpdateGraphics();
    }
}