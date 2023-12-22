using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Collection
{
    public class Item : MonoBehaviour
    {
        [Header("REFERENCES")] 
        public TextMeshProUGUI countTM;
        public Sprite epicBackground, commonBackground;
        public Image backgroundImage, faceImage;
        public Image blackImage;

        [Header("EMPTY SETTINGS")] 
        public GameObject countParent;
        public GameObject buttonParent;
        public Color emptyBackgroundColor, emptyFaceColor;
        public Sprite emptyBackgroundSprite, emptyFaceSprite;
    
        [Header("SETTINGS")]
        public bool isInDeck;
    
        [Header("DEBUGGING")]
        public Data.Player.Item item;
        public Model itemModel;
    
        public Action<bool> onItemDragBegin;
        public Action<bool> onItemDragEnd;
        public Action<Data.Player.Item> onItemSelected;
    
        public void Initialize(Action<bool> onItemDragBegin, Action<bool> onItemDragEnd, Action<Data.Player.Item> onItemSelected, Model itemModel, bool isInDeck)
        {
            this.onItemDragBegin = onItemDragBegin;
            this.onItemDragEnd = onItemDragEnd;
            this.onItemSelected = onItemSelected;
            this.itemModel = itemModel;
            this.isInDeck = isInDeck;
        }

        private void OnDisable()
        {
            if (item == null) return;
        
            item.onDataChanged -= UpdateGraphics;
        }

        public void SetItem(Data.Player.Item item)
        {
            // unsubscribe from the former event, if there was one
            if (this.item != null) this.item.onDataChanged -= UpdateGraphics;
        
            this.item = item;
            UpdateGraphics();
        
            // subscribe to the new event
            if (this.item != null) this.item.onDataChanged += UpdateGraphics;
        }

        private void UpdateGraphics()
        {
            if (item != null && isInDeck && item.cats.Count <= 0) HideCat();
            else if (item == null) HideCat();
            else ShowCat();
        }

        private void ShowCat()
        {
            buttonParent.SetActive(true);
        
            // updates information
            countParent.SetActive(true);
            countTM.text = $"{item.cats.Count}";
        
            // updates images
            backgroundImage.sprite = Registry.entitiesConfig.cats[item.entityIndex].rarety == Rarety.EPIC ? epicBackground : commonBackground;
            backgroundImage.color = Color.white;
            faceImage.sprite = Registry.entitiesConfig.cats[item.entityIndex].sprite;
            faceImage.color = Color.white;
        
            // updates graphics visibility
            blackImage.DOFade(0, 0);
        
            // hides item ui, if it's empty
            if (item.cats.Count == 0)
            {
                if (isInDeck) transform.SetAsLastSibling();
                blackImage.DOFade(0.5f, 0);
            }
        }

        private void HideCat()
        {
            buttonParent.SetActive(false);
        
            // updates information
            countParent.SetActive(false);
        
            // updates images
            backgroundImage.sprite = emptyBackgroundSprite;
            backgroundImage.color = emptyBackgroundColor;
            faceImage.sprite = emptyFaceSprite;
            faceImage.color = emptyFaceColor;
        }

        /// <summary>
        /// Set graphics cat counter amount to the given parameter
        /// </summary>
        public int UpdateGraphicsAmount(int modifier)
        {
            if (item == null)
            {
                countTM.text = "0";
                return -1;
            }
        
            int finalAmount = item.cats.Count + modifier;
            if (finalAmount < 0) finalAmount = 0;
            countTM.text = $"{finalAmount}";
            return item.cats.Count + modifier;
        }
    
        public void Press()
        {
            onItemSelected?.Invoke(item);
        }
    }
}