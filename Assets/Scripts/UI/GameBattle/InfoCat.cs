using TMPro;
using UnityEngine;

namespace UI.GameBattle
{
    public class InfoCat : MonoBehaviour
    {
        public GameObject graphicsParent;
        public TextMeshProUGUI nameCat;
        public TextMeshProUGUI infoCat;

        void OnEnable()
        {
            Registry.events.OnCatStacked += DisplayCatInfo;
            Registry.events.OnCatDestacked += RemoveCatInfo;
            graphicsParent = transform.Find("GraphicsParent").gameObject;
        }

        private void OnDisable()
        {
            Registry.events.OnCatStacked -= DisplayCatInfo;
            Registry.events.OnCatDestacked -= RemoveCatInfo;
        }

        private void DisplayCatInfo(string catTypeName, string catTypeInfo)
        {
            graphicsParent.SetActive(true);
            nameCat.text = catTypeName;
            infoCat.text = catTypeInfo;
        }

        private void RemoveCatInfo()
        {
            graphicsParent.SetActive(false);
        }
    }
}