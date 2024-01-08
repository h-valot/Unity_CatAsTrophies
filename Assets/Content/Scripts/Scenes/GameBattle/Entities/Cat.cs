using System.Collections.Generic;
using Data;
using UnityEngine;

public class Cat : Entity
{
    [Header("MESH RENDERER")]
    public SkinnedMeshRenderer CatMeshRenderer;
    public SkinnedMeshRenderer CatEyesMeshRenderer;

    [Header("BONES")] 
    public GameObject boneHead;
    public GameObject boneHand_R;
    public GameObject boneHand_L;
    
    [Header("DEBUGGING")]
    public int catType;
    public CatState state;
    public Ability ability;
    public bool isAbilityUsed;

    private Material catSkinMateriel_inst;
    private Texture catSkinTexture;
    private Material catEyesMaterial_inst;
    private Texture catEyesTexture;
    private GameObject headAddonRef;
    private GameObject rightHandAddonRef;
    private GameObject leftHandAddonRef;
    private float blobShadowPositionY;
    public bool startedTurnOnBattlefield;

    public void Initialize(int typeIndex, float currentHealth)
    {
        base.Initialize();
        state = CatState.IN_DECK;
        startedTurnOnBattlefield = false;
        catType = typeIndex;
        
        // setup entity stats
        maxHealth = Registry.entitiesConfig.cats[catType].health;
        health = currentHealth;
        autoAttacks = Registry.entitiesConfig.cats[catType].autoAttack;
        ability = Registry.entitiesConfig.cats[catType].ability;
        
        // update entity stats on the ui displayer
        onStatsUpdate?.Invoke();

        // graphics scale update
        graphicsParent.transform.localScale *= battleScale;

        // graphics material update
        catSkinTexture = Registry.entitiesConfig.cats[catType].catSkinTexture;
        catSkinMateriel_inst = CatMeshRenderer.material;
        catSkinMateriel_inst.SetTexture("_MainTex", catSkinTexture);
        catEyesTexture = Registry.entitiesConfig.cats[catType].catEyesTexture;
        catEyesMaterial_inst = CatEyesMeshRenderer.material;
        catEyesMaterial_inst.SetTexture("_MainTex", catEyesTexture);

        // graphics instantiate addons
        headAddonRef = InstantiateAddon(Registry.entitiesConfig.cats[catType].headAddon, boneHead.transform);
        if(headAddonRef) headAddonRef.SetActive(true);
        rightHandAddonRef = InstantiateAddon(Registry.entitiesConfig.cats[catType].rightHandAddon, boneHand_R.transform);
        leftHandAddonRef = InstantiateAddon(Registry.entitiesConfig.cats[catType].leftHandAddon, boneHand_L.transform);
        blobShadowPositionY = blobShadow.transform.localPosition.y;
        blobShadowRenderer.enabled = false;
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
        Registry.events.OnNewPlayerTurn += TriggerAllEffectsBeginTurn;
        Registry.events.OnEndPlayerTurn += TriggerAllEffectsEndTurn;
        Registry.events.OnEndPlayerTurn += ResetAbility;
        Registry.events.OnCatsUseAutoAttack += UseAutoAttack;
    }

    private void OnDisable()
    {
        Registry.events.OnNewPlayerTurn -= ResetAbility;
        Registry.events.OnNewPlayerTurn -= ResetArmor;
        Registry.events.OnNewPlayerTurn -= TriggerAllEffectsBeginTurn;
        Registry.events.OnEndPlayerTurn -= TriggerAllEffectsEndTurn;
        Registry.events.OnEndPlayerTurn -= ResetAbility;
        Registry.events.OnCatsUseAutoAttack -= UseAutoAttack;
    }

    private void ResetAbility() => isAbilityUsed = false;
    
    public bool CanMove() => state == CatState.IN_HAND;

    /// <summary>
    /// Put the cat in the player's hand, reset rotation and scale and update the state
    /// </summary>
    public string PutInHand()
    {
        // set the cat's position at an available position in the player's hand
        transform.position = HandManager.Instance.AddToHand(id);

        // handle graphics tweaking
        graphicsParent.transform.eulerAngles = handRotation;
        graphicsParent.SetActive(true);
        gameObject.SetActive(true);
        if (rightHandAddonRef) rightHandAddonRef.SetActive(false);
        if (leftHandAddonRef) leftHandAddonRef.SetActive(false);
        blobShadowRenderer.enabled = false; 
        blobShadow.transform.localPosition = new Vector3(blobShadow.transform.localPosition.x, blobShadowPositionY, blobShadow.transform.localPosition.z);

        // trigger animations
        animator.SetBool("IsInHand", true);
        animator.SetBool("IsFalling", false);
        animator.SetBool("IsOnBattlefield", false);

        startedTurnOnBattlefield = false;
        state = CatState.IN_HAND;
        return id;
    }

    /// <summary>
    /// Update cat's rotation, scale, state for drag state
    /// </summary>
    public void OnDrag()
    {
        graphicsParent.transform.eulerAngles = dragRotation;
        animator.SetBool("IsFalling", true);
        animator.SetBool("IsInHand", false);

        blobShadowRenderer.enabled = true;
        blobShadow.transform.localPosition = new Vector3(blobShadow.transform.localPosition.x, blobShadowPositionY + Registry.gameSettings.verticalOffsetBlobShadow, blobShadow.transform.localPosition.z);
    }

    /// <summary>
    /// Update cat's rotation, scale, state for battle state
    /// </summary>
    public void PlaceOnBattlefield()
    {
        // add armor on placed
        armor = Registry.entitiesConfig.cats[catType].armorAtStart;
        // update entity stats on the ui displayer
        onStatsUpdate?.Invoke();
        
        // handle graphics tweaking
        graphicsParent.transform.eulerAngles = battleRotation;
        if (rightHandAddonRef) rightHandAddonRef.SetActive(true);
        if (leftHandAddonRef) leftHandAddonRef.SetActive(true);
        blobShadowRenderer.enabled = true;
        blobShadow.transform.localPosition = new Vector3(blobShadow.transform.localPosition.x, blobShadowPositionY, blobShadow.transform.localPosition.z);

        // trigger animations
        animator.SetBool("IsOnBattlefield", true);
        animator.SetBool("IsFalling", false);
        
        state = CatState.ON_BATTLE;
        onBattlefieldEntered?.Invoke();
        stopAsync = false;
    }

    protected override void TriggerAllEffectsBeginTurn()
    {
        if (state == CatState.ON_BATTLE)
        {
            startedTurnOnBattlefield = true;
        }


        List<Effect> effectsToRemove = new List<Effect>();
        foreach (var effect in effects)
        {
            if (effect.type == EffectType.DOT || effect.type == EffectType.HOT)
            {
                effect.Trigger();

                // if the effect expire, add it to a list of all effects to remove
                if (effect.turnDuration <= 0)
                {
                    effectsToRemove.Add(effect);
                }
            }
        }

        // removes all expired effects
        foreach (var effect in effectsToRemove)
        {
            effects.Remove(effect);
        }


        if (!HasEffect(EffectType.STUN) && !HasEffect(EffectType.SLEEP) && state != CatState.IN_HAND)
        {
            animator.SetBool("IsActing", false);
        }

        // trigger update display function in EntityUIDisplay.cs
        onStatsUpdate?.Invoke();
    }

    protected override void TriggerAllEffectsEndTurn()
    {
        List<Effect> effectsToRemove = new List<Effect>();
        foreach (var effect in effects)
        {
            if (effect.type != EffectType.DOT && effect.type != EffectType.HOT)
            {
                effect.Trigger();

                // if the effect expire, add it to a list of all effects to remove
                if (effect.turnDuration <= 0)
                {
                    effectsToRemove.Add(effect);
                }
            }
        }

        // removes all expired effects
        foreach (var effect in effectsToRemove)
        {
            effects.Remove(effect);
        }


        if (!HasEffect(EffectType.STUN) && !HasEffect(EffectType.SLEEP) && state != CatState.IN_HAND)
        {
            animator.SetBool("IsActing", false);
        }

        // trigger update display function in EntityUIDisplay.cs
        onStatsUpdate?.Invoke();
    }

    public override void UpdateBattlePosition(BattlePosition battlePosition)
    {
        base.UpdateBattlePosition(battlePosition);
        
        // set the entity position to the corresponding battle pawn
        transform.position = BattlefieldManager.Instance.catBattlePawns[(int)base.battlePosition].transform.position;
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
        // add the cat to the attack queue in the turn manager and return it's order of attack
        int attackingOrder = TurnManager.Instance.AddCatAttackQueue(this);
        isAbilityUsed = true;
    }

    /// <summary>
    /// Use auto attacks abilities
    /// </summary>
    public override void UseAutoAttack()
    {
        // exit if cat isn't on the battlefield
        if (state != CatState.ON_BATTLE) return;
        
        base.UseAutoAttack();
    }
    
    /// <summary>
    /// Withdraw a cat from the battlefield to place it into the discard pile
    /// </summary>
    public void Withdraw()
    {
        // exit if the cat is in the graveyard
        if (state == CatState.IN_GRAVEYARD) return;
     
        // handle entity stats tweaking 
        armor = 0;
        effects.Clear();
        
        // handle graphics tweaking
        graphicsParent.SetActive(false);
        if (rightHandAddonRef) rightHandAddonRef.SetActive(false);
        if (leftHandAddonRef) leftHandAddonRef.SetActive(false);

        stopAsync = true;

        DiscardManager.Instance.AddCat(id);
        state = CatState.DISCARDED;
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
        
        state = CatState.IN_GRAVEYARD;
        GraveyardManager.Instance.AddCat(id);

        CatManager.Instance.deadCatAmount++;
        
        base.HandleDeath();
    }
}

public enum CatState
{
    IN_DECK = 0,
    IN_HAND,
    ON_BATTLE,
    DISCARDED,
    IN_GRAVEYARD
}