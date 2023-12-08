using UnityEngine;
using TMPro;

public class CurrenciesManager : MonoBehaviour
{
    public static CurrenciesManager instance;
    
    public int Tuna, Treats;
    public TextMeshProUGUI TunaTM, TreatsTM;

    public void Addtuna(int value)
    {
        Tuna += value;
        UpdateDisplay();
    }

    public void SpentTuna(int value)
    {
        Tuna -= value;
        UpdateDisplay();
    }

    public void AddTreats(int value)
    {
        Treats += value;
        UpdateDisplay();
    }

    public void SpentTreats(int value)
    {
        Treats -= value;
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        TunaTM.text = $"Tuna: {Tuna}";
        TreatsTM.text = $"Treats: {Treats}";
    }
}