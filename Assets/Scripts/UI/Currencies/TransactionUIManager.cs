using Data;
using TMPro;
using UnityEngine;

public class TransactionUIManager : MonoBehaviour
{
    [Header("REFERENCES")] 
    public GameObject graphicsParent;

    [Header("TEXTS")] 
    public TextMeshProUGUI titleTM;
    public TextMeshProUGUI flavorTM;
    public TextMeshProUGUI confirmationTM;

    private Currency _currency;
    private int _cost;
    
    public void UpdateGraphics(string name, int cost, Currency currency)
    {
        _currency = currency;
        _cost = cost;
        titleTM.text = $"{name}";

        var canBuy = currency == Currency.TUNA
            ? DataManager.data.playerStorage.tuna >= cost
            : DataManager.data.playerStorage.treats >= cost;
        
        var costDisplay = currency == Currency.TUNA
            ? $"{DataManager.data.playerStorage.tuna} TUNA"
            : $"{DataManager.data.playerStorage.treats} TREATS";
        
        flavorTM.text = canBuy ? $"New balance: {costDisplay}" : "Not enough";

        confirmationTM.text = costDisplay;
    }
    
    public void Buy()
    {
        switch (_currency)
        {
            case Currency.TUNA:
                DataManager.data.playerStorage.tuna -= _cost;
                break;
            case Currency.TREATS:
                DataManager.data.playerStorage.treats -= _cost;
                break;
        }
    }
    
    public void Show() => graphicsParent.SetActive(true);
    public void Hide() => graphicsParent.SetActive(false);
}

public enum Currency
{
    TUNA = 0,
    TREATS
}