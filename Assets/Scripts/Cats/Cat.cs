using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Cat : MonoBehaviour
{
    [Header("REFERENCES")]
    public GameObject graphicsParent;

    [Header("BONES")] 
    public GameObject bone;
    
    [Header("GRAPHICS TWEAKING")] 
    public Vector3 battleRotation;
    public Vector3 baseRotation;
    public float battleScale;
    
    [Header("DEBUGGING")]
    public string id;
    public int typeIndex;
    public CatState state;
    public float health;
    public bool isAbilityUsed;

    public void Initialize(int _typeIndex)
    {
        state = CatState.InDeck;
        typeIndex = _typeIndex;
        id = Misc.GetRandomId();

        health = Registry.catConfig.cats[typeIndex].health;
    }

    private void OnEnable()
    {
        Registry.events.OnNewPlayerTurn += ResetAbility;
    }

    private void OnDisable()
    {
        Registry.events.OnNewPlayerTurn -= ResetAbility;
    }

    private void ResetAbility() => isAbilityUsed = false;
    
    public bool CanMove() => state == CatState.InHand;

    /// <summary>
    /// Put the cat in the player's hand, reset rotation and scale and update the state
    /// </summary>
    public string PutInHand()
    {
        transform.position = HandManager.Instance.GetAvailablePosition();
        
        graphicsParent.transform.eulerAngles = baseRotation;
        graphicsParent.transform.localScale = Vector3.one;
        graphicsParent.SetActive(true);
        gameObject.SetActive(true);
        
        state = CatState.InHand;
        return id;
    }
    
    /// <summary>
    /// Place the cat onto the battlefield, update rotation, scale and the state
    /// </summary>
    public void PlaceOnBattlefield()
    {
        graphicsParent.transform.eulerAngles = battleRotation;
        graphicsParent.transform.localScale *= battleScale;
        
        state = CatState.OnBattle;
        UseAbility();
    }

    /// <summary>
    /// Use the cat's custom ability
    /// </summary>
    public void UseAbility()
    {
        // count as a player action
        TurnManager.Instance.actionCounter++;

        isAbilityUsed = true;
    }
    
    /// <summary>
    /// Deal a fix amout of damage to an enemy
    /// </summary>
    public void UseAutoAttack()
    {
        // do nothing for the moment
    }
    
    /// <summary>
    /// Withdraw a cat from the battlefield to place it into the graveyard
    /// </summary>
    public void Withdraw()
    {
        GraveyardManager.Instance.AddCat(id);
        graphicsParent.SetActive(false);
        
        state = CatState.InGraveyard;
    }
}

public enum CatState
{
    InDeck = 0,
    InHand,
    OnBattle,
    InGraveyard
}