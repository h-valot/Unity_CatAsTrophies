using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectionUIItem : MonoBehaviour
{
    [Header("REFERENCES")] 
    public Image image;
    public TextMeshProUGUI countTM;

    private bool _isInDeck;
    
    public void Initialize(bool isInDeck)
    {
        _isInDeck = isInDeck;
    }

    public void UpdateGraphics(Item item)
    {
        image.sprite = Registry.entitiesConfig.cats[item.entityIndex].sprite;
        countTM.text = $"{item.count}";
    }
    
    public void Press()
    {
        if (_isInDeck)
        {
            Debug.Log("COLLECTION UI ITEM: item was in the deck, going into the collection");
        }
        else
        {
            Debug.Log("COLLECTION UI ITEM: item was in the collection, going into the deck");
        }
    }
}
