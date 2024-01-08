using TMPro;
using UnityEngine;

public class DebugCompButton : MonoBehaviour
{
    [Header("EXTERNAL REFERENCES")]
    public EntitiesConfig entitiesConfig;
    
    [Header("REFERENCES")] 
    public TextMeshProUGUI textTM;
    
    private int compoId;

    public void Initialize(int _compoId)
    {
        compoId = _compoId;
        textTM.text = $"DEBUG: {entitiesConfig.compositions[compoId].compositionName}";
    }

    public void OnButtonPressed()
    {
        bool battlefieldEmpty = true;
        foreach (var battlePawn in BattlefieldManager.Instance.enemyBattlePawns)
        {
            if (battlePawn.entityIdLinked != "")
            {
                battlefieldEmpty = false;
                break;
            }
        }

        if (battlefieldEmpty)
        {
            EnemyGenerator.Instance.GenerateComposition(entitiesConfig.compositions[compoId]);
        }
        else
        {
            Debug.Log("DEBUG COMP BUTTON: can not instantiate new enemies because there are still enemies alive");
        }
    }
}