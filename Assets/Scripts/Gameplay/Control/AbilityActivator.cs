using System.Threading.Tasks;
using UnityEngine;

public class AbilityActivator : MonoBehaviour
{
    public Cat catUsed;
    public float holdingTime;
    
    private bool isTouching;
    
    private void OnMouseDown()
    {
        // exit if cannot trigger ability
        if (!CanTriggerAbility()) return;
        
        isTouching = true;
        Timer(holdingTime);
    }

    private void OnMouseUp()
    {
        isTouching = false;
    }

    private async Task Timer(float _timerToWait)
    {
        await Task.Delay((int)(_timerToWait * 1000));
        
        if (isTouching &&
            TurnManager.Instance.actionCounter < 3)
        {
            catUsed.UseAbility();
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