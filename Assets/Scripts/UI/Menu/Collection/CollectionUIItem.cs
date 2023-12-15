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

    [HideInInspector] public Item item;
    private bool _isInDeck;
    private int _count;
    
    public void Initialize(Item item, bool isInDeck)
    {
        this.item = item;
        _isInDeck = isInDeck;
        this.item.onChanged += UpdateGraphics;
    }

    private void OnDisable()
    {
        item.onChanged -= UpdateGraphics;
    }

    public void UpdateGraphics()
    {
        catImage.sprite = Registry.entitiesConfig.cats[item.entityIndex].sprite;
        
        _count = _isInDeck 
            ? DataManager.data.playerStorage.GetCount(item.entityIndex, DataManager.data.playerStorage.deck) 
            : DataManager.data.playerStorage.GetCount(item.entityIndex, DataManager.data.playerStorage.collection);
        
        countTM.text = $"{_count}";

        if (_count == 0) blackImage.DOFade(0.5f, 0);
        else blackImage.DOFade(0, 0);
    }
    
    public void Press()
    {
        // exit, if there is no more cats
        if (_count == 0)
        {
            UpdateGraphics();
            return;
        }
        
        if (_isInDeck)
        {
            DataManager.data.playerStorage.Transfer(DataManager.data.playerStorage.deck, DataManager.data.playerStorage.collection, item.entityIndex);
        }
        else
        {
            DataManager.data.playerStorage.Transfer(DataManager.data.playerStorage.collection, DataManager.data.playerStorage.deck, item.entityIndex);
        }
        
        UpdateGraphics();
    }
}