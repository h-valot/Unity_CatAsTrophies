using TMPro;
using UnityEngine;

public class EffectUIDiplay : MonoBehaviour
{
    public TextMeshProUGUI effectNameTM, effectDurationTM;

    public void UpdateDisplay(EffectType _effect, int _effectDuration)
    {
        effectNameTM.text = _effect.ToString().Substring(0, 3);
        effectDurationTM.text = _effectDuration.ToString();
    }
}