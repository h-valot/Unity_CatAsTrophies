using UnityEngine;

public class Cat : Entity
{
    [Header("REFERENCES")]
    public GameObject graphicsParent;
    public Animator animator;

    [Header("BONES")] 
    public GameObject boneHead;
    public GameObject boneHand_R;
    public GameObject boneHand_L;

    [Header("GRAPHICS TWEAKING")] 
    public Vector3 battleRotation;
    public Vector3 baseRotation;
    public float battleScale;
    
    [Header("DEBUGGING")]
    public int catType;
    public CatState state;
    public Ability ability;
    public bool isAbilityUsed;

    private GameObject headAddonRef;
    private GameObject rightHandAddonRef;
    private GameObject leftHandAddonRef;

    public void Initialize(int _typeIndex)
    {
        base.Initialize();
        
        state = CatState.InDeck;
        catType = _typeIndex;
        id = Misc.GetRandomId();
        
        maxHealth = Registry.entitiesConfig.cats[catType].health;
        health = maxHealth;

        // GRAPHICS
        // Head
        headAddonRef = Instantiate(Registry.entitiesConfig.cats[catType].headAddon, boneHead.transform, true);
        headAddonRef.transform.localPosition = Vector3.zero;
        headAddonRef.transform.localRotation = Quaternion.identity;
        // Right Hand
        if (Registry.entitiesConfig.cats[catType].rightHandAddon)
        {
            rightHandAddonRef = Instantiate(Registry.entitiesConfig.cats[catType].rightHandAddon, boneHand_R.transform, true);
            rightHandAddonRef.transform.localPosition = Vector3.zero;
            rightHandAddonRef.transform.localRotation = Quaternion.identity;
            rightHandAddonRef.SetActive(false);
        }
    }

    private void OnEnable()
    {
        Registry.events.OnNewPlayerTurn += ResetAbility;
        Registry.events.OnNewPlayerTurn += TriggerAllEffects;
    }

    private void OnDisable()
    {
        Registry.events.OnNewPlayerTurn -= ResetAbility;
        Registry.events.OnNewPlayerTurn -= TriggerAllEffects;
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

        animator.SetTrigger("IsInHand");

        return id;
    }
    
    /// <summary>
    /// Place the cat onto the battlefield, update rotation, scale and the state
    /// </summary>
    public void PlaceOnBattlefield()
    {
        graphicsParent.transform.eulerAngles = battleRotation;
        graphicsParent.transform.localScale *= battleScale;
        if (rightHandAddonRef)
        {
            rightHandAddonRef.SetActive(true);
        }

        state = CatState.OnBattle;
        animator.SetTrigger("IsFighting");
        
        UseAbility();
    }

    /// <summary>
    /// Use the cat's custom ability
    /// </summary>
    public void UseAbility()
    {
        // count as a player action
        TurnManager.Instance.actionCounter++;

        ability.Use(this);
        isAbilityUsed = true;
        Debug.Log("CAT: ability used");
    }
    
    /// <summary>
    /// Use auto attacks abilities
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