using System.Collections.Generic;
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

    public void Initialize(int typeIndex)
    {
        base.Initialize();
        state = CatState.InDeck;
        catType = typeIndex;
        
        // setup entity stats
        maxHealth = Registry.entitiesConfig.cats[catType].health;
        health = maxHealth;
        autoAttacks = Registry.entitiesConfig.cats[catType].autoAttack;
        ability = Registry.entitiesConfig.cats[catType].ability;
        
        // update entity stats on the ui displayer
        OnStatsUpdate?.Invoke();

        // graphics scale update
        graphicsParent.transform.localScale *= battleScale;
        
        // graphics material update
        material = Registry.entitiesConfig.cats[catType].baseMaterial;
        skinnedMeshRenderer.sharedMaterial = material;
        
        // graphics instantiate addons
        headAddonRef = InstantiateAddon(Registry.entitiesConfig.cats[catType].headAddon, boneHead.transform);
        headAddonRef.SetActive(true);
        rightHandAddonRef = InstantiateAddon(Registry.entitiesConfig.cats[catType].rightHandAddon, boneHand_R.transform);
        leftHandAddonRef = InstantiateAddon(Registry.entitiesConfig.cats[catType].leftHandAddon, boneHand_L.transform);
    }

    /// <summary>
    /// Create a new addon and snap it to the desired bone
    /// </summary>
    /// <returns>The newly instantiated addon</returns>
    private GameObject InstantiateAddon(GameObject _addonToInstantiate, Transform _bone)
    {
        // exit if the addon doesn't exist
        if (!_addonToInstantiate) return null;
        
        var newAddon = Instantiate(_addonToInstantiate, _bone, true);
        newAddon.transform.localPosition = Vector3.zero;
        newAddon.transform.localRotation = Quaternion.identity;
        newAddon.transform.localScale *= battleScale;
        newAddon.SetActive(false);

        return newAddon;
    }
    
    private void OnEnable()
    {
        Registry.events.OnNewPlayerTurn += ResetAbility;
        Registry.events.OnNewPlayerTurn += ResetArmor;
        Registry.events.OnNewPlayerTurn += TriggerAllEffects;
        Registry.events.OnCatsUseAutoAttack += UseAutoAttack;
    }

    private void OnDisable()
    {
        Registry.events.OnNewPlayerTurn -= ResetAbility;
        Registry.events.OnNewPlayerTurn -= ResetArmor;
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
        // add armor on placed
        armor = Registry.entitiesConfig.cats[catType].armorAtStart;
        // update entity stats on the ui displayer
        OnStatsUpdate?.Invoke();
        
        // handle graphics tweaking
        graphicsParent.transform.eulerAngles = battleRotation;
        if (rightHandAddonRef) rightHandAddonRef.SetActive(true);
        if (leftHandAddonRef) leftHandAddonRef.SetActive(true);
        blobShadowRenderer.enabled = true;

        // trigger animations
        animator.SetTrigger("IsFighting");

        int AttackingOrder = TurnManager.Instance.addCatAttackQueue(this); //Add the cat to the attack queue in the turn manager and return it's order of attack
        isAbilityUsed = true;
        state = CatState.OnBattle;
        OnBattlefieldEntered?.Invoke();
        stopAsync = false;
    }

    protected override void TriggerAllEffects()
    {
        List<Effect> effectsToRemove = new List<Effect>();
        foreach (var effect in effects)
        {
            effect.Trigger();

            // if the effect expire, add it to a list of all effects to remove
            if (effect.turnDuration <= 0)
            {
                effectsToRemove.Add(effect);
            }
        }

        // removes all expired effects
        foreach (var effect in effectsToRemove)
        {
            effects.Remove(effect);
        }


        if (!HasEffect(EffectType.Stun) && !HasEffect(EffectType.Sleep) && state != CatState.InHand)
        {
            animator.SetTrigger("IsFighting");
        }

        //trigger update display function in EntityUIDisplay.cs
        OnStatsUpdate?.Invoke();
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
        ability.Use(this);
        isAbilityUsed = true;
    }
    
    public void AddCatAttackQueue()
    {
        int AttackingOrder = TurnManager.Instance.addCatAttackQueue(this); //Add the cat to the attack queue in the turn manager and return it's order of attack
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
    /// Withdraw a cat from the battlefield to place it into the discard pile
    /// </summary>
    public void Withdraw()
    {
        // exit if the cat is in the graveyard
        if (state == CatState.InGraveyard) return;
     
        // handle entity stats tweaking 
        armor = 0;
        
        // handle graphics tweaking
        graphicsParent.SetActive(false);
        if (rightHandAddonRef) rightHandAddonRef.SetActive(false);
        if (leftHandAddonRef) leftHandAddonRef.SetActive(false);

        stopAsync = true;

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

        base.HandleDeath();
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