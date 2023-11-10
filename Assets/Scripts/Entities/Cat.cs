using UnityEngine;

public class Cat : Entity
{
    [Header("BONES")] 
    public GameObject boneHead;
    public GameObject boneHand_R;
    public GameObject boneHand_L;
    
    [Header("DEBUGGING")]
    public int catType;
    public CatState state;
    public Ability ability;
    public bool isAbilityUsed;

    private Material material;
    private GameObject headAddonRef;
    private GameObject rightHandAddonRef;
    private GameObject leftHandAddonRef;

    public void Initialize(int _typeIndex)
    {
        base.Initialize();
        state = CatState.InDeck;
        catType = _typeIndex;
        
        // GAME STATS
        maxHealth = Registry.entitiesConfig.cats[catType].health;
        health = maxHealth;
        autoAttacks = Registry.entitiesConfig.cats[catType].autoAttack;
        ability = Registry.entitiesConfig.cats[catType].ability;
        
        // update game stat on ui displayer
        OnStatsUpdate?.Invoke();

        // GRAPHICS
        // Material update
        material = Registry.entitiesConfig.cats[catType].baseMaterial;
        skinnedMeshRenderer.sharedMaterial = material;
        // head addon
        headAddonRef = Instantiate(Registry.entitiesConfig.cats[catType].headAddon, boneHead.transform, true);
        headAddonRef.transform.localPosition = Vector3.zero;
        headAddonRef.transform.localRotation = Quaternion.identity;
        // right hand addon
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
        Registry.events.OnCatsUseAutoAttack += UseAutoAttack;
    }

    private void OnDisable()
    {
        Registry.events.OnNewPlayerTurn -= ResetAbility;
        Registry.events.OnNewPlayerTurn -= TriggerAllEffects;
        Registry.events.OnCatsUseAutoAttack -= UseAutoAttack;
    }

    private void ResetAbility() => isAbilityUsed = false;
    
    public bool CanMove() => state == CatState.InHand;

    /// <summary>
    /// Put the cat in the player's hand, reset rotation and scale and update the state
    /// </summary>
    public string PutInHand()
    {
        // set the cat's position at an available position in the player's hand
        transform.position = HandManager.Instance.GetAvailablePosition();
        
        // handle graphics tweaking
        graphicsParent.transform.eulerAngles = baseRotation;
        graphicsParent.transform.localScale = Vector3.one;
        graphicsParent.SetActive(true);
        gameObject.SetActive(true);
        blobShadowRenderer.enabled = false;

        // trigger animations
        animator.SetTrigger("IsInHand");

        state = CatState.InHand;
        return id;
    }
    
    /// <summary>
    /// Update cat's rotation, scale, state and use this ability
    /// </summary>
    public void PlaceOnBattlefield()
    {
        // handle graphics tweaking
        graphicsParent.transform.eulerAngles = battleRotation;
        graphicsParent.transform.localScale *= battleScale;
        if (rightHandAddonRef)
        {
            rightHandAddonRef.SetActive(true);
        }
        blobShadowRenderer.enabled = true;

        // trigger animations
        animator.SetTrigger("IsFighting");
        
        UseAbility();
        state = CatState.OnBattle;
        OnBattlefieldEntered?.Invoke();
    }
    
    public override void UpdateBattlePosition(BattlePosition _battlePosition)
    {
        base.UpdateBattlePosition(_battlePosition);
        
        // set the entity position to the corresponding battle pawn
        transform.position = BattlefieldManager.Instance.catBattlePawns[(int)battlePosition].transform.position;
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
    }
    
    /// <summary>
    /// Use auto attacks abilities
    /// </summary>
    public override void UseAutoAttack()
    {
        // exit if cat isn't on the battlefield
        if (state != CatState.OnBattle) return;
        
        base.UseAutoAttack();
    }
    
    /// <summary>
    /// Withdraw a cat from the battlefield to place it into the graveyard
    /// </summary>
    public void Withdraw()
    {
        // exit if the cat is in the graveyard
        if (state == CatState.InGraveyard) return;
        
        // handle graphics tweaking
        graphicsParent.SetActive(false);
        if (rightHandAddonRef)
        {
            rightHandAddonRef.SetActive(false);
        }
        
        DiscardManager.Instance.AddCat(id);
        state = CatState.Discarded;
    }
    
    /// <summary>
    /// Hide the cat's graphics and add a reference to it 
    /// </summary>
    public override void HandleDeath()
    {
        // handle graphics tweaking
        graphicsParent.SetActive(false);
        if (rightHandAddonRef)
        {
            rightHandAddonRef.SetActive(false);
        }
        
        BattlefieldManager.Instance.RemoveFromBattlePawn(id);
        
        state = CatState.InGraveyard;
        GraveyardManager.Instance.AddCat(id);
    }
}

public enum CatState
{
    InDeck = 0,
    InHand,
    OnBattle,
    Discarded,
    InGraveyard
}