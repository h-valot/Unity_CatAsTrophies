using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraveyardUI : MonoBehaviour
{
    public GameObject cardCatPrefab;
    public VerticalLayoutGroup verticalLayoutGroup;
    public List<CatCardDisplay> catCards;

    private void OnEnable()
    {
        UpdateAllDisplays();
    }
    
    private void UpdateAllDisplays()
    {
        // if there are less ui cat cards than cats in the graveyard
        // then instantiate new ui cat cards 
        if (catCards.Count < GraveyardManager.Instance.catsInGraveyard.Count)
        {
            InstantiateAllCats();
            return;
        }
        
        // update all ui cat cards 
        for (int i = 0; i < catCards.Count; i++)
        {
            // if there are less cats in the graveyard than ui cat cards
            // then hide and remove ui cat cards from the list
            if (i > GraveyardManager.Instance.catsInGraveyard.Count)
            {
                Destroy(catCards[i]);
                catCards.Remove(catCards[i]);
            }
            else
            {
                catCards[i].Show();
                catCards[i].UpdateDisplay();
            }
        }
    }

    private void InstantiateAllCats()
    {
        for (int i = 0; i < GraveyardManager.Instance.catsInGraveyard.Count; i++)
        {
            if (i < catCards.Count)
            {
                // update display that already exists
                catCards[i].Show();
                catCards[i].UpdateDisplay();
            }
            else
            {
                // instantiate a new display
                var newCard = Instantiate(cardCatPrefab, verticalLayoutGroup.transform).GetComponent<CatCardDisplay>();
                newCard.Initialize(GraveyardManager.Instance.catsInGraveyard[i]);
                newCard.UpdateDisplay();
                catCards.Add(newCard);
            }
        }
    }
}