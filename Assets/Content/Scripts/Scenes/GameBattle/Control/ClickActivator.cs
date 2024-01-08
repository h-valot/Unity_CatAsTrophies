using System.Threading.Tasks;
using UnityEngine;

public class ClickActivator : MonoBehaviour
{
    [Header("EXTERNAL REFERENCES")]
    public GameSettings gameSettings;
    
    [Header("REFERENCES")]
    public Cat catUsed;
    
    private bool isTouching;
    private float timeStartClick;

    private void OnMouseDown()
    {
        isTouching = true;
        
        if (catUsed.state != CatState.ON_BATTLE) return;
        
        timeStartClick = Time.time;
        Timer(gameSettings.holdingTimeToTriggerAbility, timeStartClick);
    }

    private void OnMouseUp()
    {
        isTouching = false;
        // the difference between a click and a drag is made inside DragAndDrop.cs
    }

    private async void Timer(float timerToWait, float timeStartClick)
    {
        await Task.Delay((int)(timerToWait * 1000));
        if (!isTouching || TurnManager.Instance.catAttackQueue.Count >= 3 || !CanTriggerAbility() || this.timeStartClick != timeStartClick) return;
        
        foreach (var battlePawn in BattlefieldManager.Instance.catBattlePawns)
        {
            if (Misc.IdManager.GetCatById(battlePawn.entityIdLinked) != catUsed) continue;
            
            battlePawn.orderInQueue = TurnManager.Instance.catAttackQueue.Count;
            break;
        }
        catUsed.AddCatAttackQueue();
    }

    /// <summary>
    /// Verify if the cat if on the battlefield
    /// </summary>
    private bool CanTriggerAbility()
    {
        return catUsed.state == CatState.ON_BATTLE &&
               catUsed.isAbilityUsed == false;
    }
}