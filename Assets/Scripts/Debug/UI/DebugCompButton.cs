using TMPro;
using UnityEngine;

public class DebugCompButton : MonoBehaviour
{
    private int compoId;
    public TextMeshProUGUI textTM;

    public void Initialize(int _compoId)
    {
        compoId = _compoId;
        textTM.text = $"DEBUG: {Registry.entitiesConfig.compositions[compoId].compositionName}";
    }

    public void OnButtonPressed()
    {
        bool battlefieldEmpty = true;
        foreach (var battlePawn in BattlefieldManager.Instance.enemyBattlePawns)
        {
            if (battlePawn.entityIdLinked != "empty")
            {
                battlefieldEmpty = false;
                break;
            }
        }

        if (battlefieldEmpty)
        {
            EnemyGenerator.Instance.GenerateComposition(compoId);
        }
        else
        {
            Debug.Log("DEBUG COMP BUTTON: can not instantiate new enemies because there are still enemies alive");
        }
    }
}