using TMPro;
using UnityEngine;

namespace UI.GameBattle
{
    public class EffectItem : MonoBehaviour
    {
        public TextMeshProUGUI effectNameTM, effectDurationTM;

        public void UpdateDisplay(EffectType effect, int effectDuration)
        {
            effectNameTM.text = effect.ToString().Substring(0, 3);
            effectDurationTM.text = effectDuration.ToString();
        }
    }
}