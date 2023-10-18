using UnityEngine;

public class Cat : MonoBehaviour
{
    public GameObject graphicsParent;
    
    [Header("DEBUGGING")]
    public string id;
    public int typeIndex;
    public CatState state;
    public float health;

    public void Initialize(int _typeIndex)
    {
        state = CatState.InDeck;
        typeIndex = _typeIndex;
        id = Misc.GetRandomId();

        health = Registry.catConfig.cats[typeIndex].health;
    }
    
    public bool CanMove() => state == CatState.InHand;

    public void Place()
    {
        // place the cat onto the battlefield
        state = CatState.OnBattle;
        UseAbility();
    }

    public void UseAbility()
    {
        // use the cat's custom ability
        TurnManager.Instance.actionCounter++;
    }
    
    public void UseAutoAttack()
    {
        // deal fix amout of damage to an enemy 
    }
    
    /// <summary>
    /// Withdraw a cat from the battlefield to place it into the graveyard
    /// </summary>
    public void Withdraw()
    {
        GraveyardManager.Instance.AddCat(this);
        state = CatState.InGraveyard;
        
        // finally repool the cat and desactivating it
        graphicsParent.SetActive(false);
    }
}

public enum CatState
{
    InDeck = 0,
    InHand,
    OnBattle,
    InGraveyard
}