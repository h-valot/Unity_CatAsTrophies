using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI.Battle
{
    public class GraveyardUI : MonoBehaviour
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
            foreach (string catId in GraveyardManager.Instance.catsInGraveyard)
            {
                // instantiate a new display
                var newCard = Instantiate(itemPrefab, parent);
                newCard.UpdateDisplay(catId);
                catCards.Add(newCard);
            }

            var catAmount = catCards.Count;
            if (catAmount <= 1) amount.text = $"{catAmount} dead cat";
            else amount.text = $"{catAmount} dead cats";
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