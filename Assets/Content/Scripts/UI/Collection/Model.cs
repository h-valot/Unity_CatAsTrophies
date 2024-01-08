using UnityEngine;
using UnityEngine.UI;

namespace UI.Collection
{
    public class Model : MonoBehaviour
    {
        [Header("REFERENCES")] 
        public GameObject graphicsParent;
        public Sprite epicBackground, commonBackground;
        public Image backgroundImage, faceImage;

        public void UpdateGraphics(Data.Player.Item item)
        {
            if (item == null)
            {
                Debug.LogError("COLLECTION UI MODEL: the given item is null");
                return;
            }
     
            // updates images
            backgroundImage.sprite = Registry.entitiesConfig.cats[item.entityIndex].rarety == Rarety.EPIC ? epicBackground : commonBackground;
            backgroundImage.color = Color.white;
            faceImage.sprite = Registry.entitiesConfig.cats[item.entityIndex].sprite;
            faceImage.color = Color.white;
        }

        public void Show() => graphicsParent.SetActive(true);
        public void Hide() => graphicsParent.SetActive(false);
    }
}