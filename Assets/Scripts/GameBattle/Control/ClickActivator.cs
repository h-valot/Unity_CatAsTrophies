using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class ClickActivator : MonoBehaviour
{
    public Cat catUsed;
    
    private bool isTouching;
    private float timeStartClick;

    private void OnMouseDown()
    {
        isTouching = true;
        if (catUsed.state == CatState.OnBattle)
        {
            timeStartClick = Time.time;
            Timer(Registry.gameSettings.holdingTimeToTriggerAbility, timeStartClick);
        }
    }

    private void OnMouseUp()
    {
        isTouching = false;

        //the difference between a click and a drag is made inside DragAndDrop.cs
    }

    private async void Timer(float _timerToWait, float _timeStartClick)
    {
        await Task.Delay((int)(_timerToWait * 1000));
        if (isTouching && TurnManager.Instance.catAttackQueue.Count < 3 && CanTriggerAbility() && timeStartClick == _timeStartClick)
        {
            catUsed.AddCatAttackQueue();
        }
    }

    /// <summary>
    /// Verify if the cat if on the battlefield
    /// </summary>
    private bool CanTriggerAbility()
    {
        return catUsed.state == CatState.OnBattle &&
               catUsed.isAbilityUsed == false;
    }
}