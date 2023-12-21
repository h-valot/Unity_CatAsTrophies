using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI.Battle
{
    public class DeckUI : MonoBehaviour
    {
        [Header("REFERENCES")]
        public Item itemPrefab;
        public Transform parent;
        public TextMeshProUGUI amount;
        
        [Header("DEBUGGING")]
        public List<Item> catCards;

        private void OnEnable()
        {
            InstantiateAllCats();
        }

        private void OnDisable()
        {
            DestroyAllCats();
        }

        private void InstantiateAllCats()
        {
            foreach (string catId in DeckManager.Instance.catsInDeck)
            {
                // instantiate a new display
                Item newCard = Instantiate(itemPrefab, parent);
                newCard.UpdateDisplay(catId);
                catCards.Add(newCard);
            }

            var catAmount = catCards.Count;
            if (catAmount <= 1) amount.text = $"{catAmount} cat in deck";
            else amount.text = $"{catAmount} cats in deck";
        }

        private void DestroyAllCats()
        {
            foreach (var catCard in catCards)
            {
                Destroy(catCard.gameObject);
            }
            catCards.Clear();
        }
    }
}