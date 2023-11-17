using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrenciesManager : MonoBehaviour
{
    public static CurrenciesManager instance;
    public int Tuna = 0;
    public int Treats = 0;
    public TMP_Text TunaText;
    public TMP_Text TreatsText;

    public void Addtuna(int value)
    {
        Tuna += value;
    }

    public void SpentTuna(int value)
    {
        Tuna -= value;
    }

    public void AddTreats(int value)
    {
        Treats += value;
    }

    public void SpentTreats(int value)
    {
        Treats -= value;
    }

    public void Update()
    {
        TunaText.text = "Tuna : " + Tuna.ToString();
        TreatsText.text = "Treats : " + Treats.ToString();
    }
}
