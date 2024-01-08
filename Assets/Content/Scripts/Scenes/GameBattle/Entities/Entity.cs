using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class Entity : MonoBehaviour
{
    public string id;

    [Header("EXTERNAL REFERENCES")] 
    public GameSettings gameSettings;
    
    [Header("REFERENCES")]
    public GameObject graphicsParent;
    public Animator animator;
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public SkinnedMeshRenderer catEyesRenderer;
    public GameObject blobShadow;
    public SpriteRenderer blobShadowRenderer;

    [Header("GAMEPLAY")] 
    public BattlePosition battlePosition;
    public float health;
    public float maxHealth;
    public int armor;
    public List<Ability> autoAttacks = new List<Ability>();
    public List<Effect> effects = new List<Effect>();

    [Header("GRAPHICS TWEAKING")] 
    public Vector3 battleRotation;
    public Vector3 handRotation;
    public Vector3 dragRotation;
    public float battleScale;
    
    public Action onStatsUpdate;
    public Action onBattlefieldEntered;
    public Action<string, Color, bool> onStatusReceived; // text to display, color of the text, is an effect or not (change font size)
    public Action onIntentUpdate;
    public Action<Entity> onIntentReset;

    public int selectedAutoAttack;
    protected bool stopAsync;

    public bool isInFrontOfBackgroundFade = false; // used by TurnManager.cs

    public void Initialize()
    {
        id = Misc.IdManager.GetRandomId();
        stopAsync = false;
    }

    /// <summary>
    /// Select an auto attacks ability among the list of auto attacks abilities.
    /// </summary>
    public void SelectAutoAttack()
    {
        // exit if entity is stunned or slept
        if (HasEffect(EffectType.STUN) || HasEffect(EffectType.SLEEP)) return;
        
        selectedAutoAttack = UnityEngine.Random.Range(0, autoAttacks.Count);
        onIntentUpdate?.Invoke();
    }

    /// <summary>
    /// Returns all the target ids of every instruction of the selected ability.
    /// </summary>
    public List<string> GetAutoAttackTarget()
    {
        // exit if entity is stunned or slept
        return !HasEffect(EffectType.STUN) && !HasEffect(EffectType.SLEEP)
            ? autoAttacks[selectedAutoAttack].GetInvolvedTargetId(this)
            : new List<string>();
    }

    /// <summary>
    /// Use the auto attacks ability previously selected.
    /// Cannot be used if the entity is stunned or slept
    /// </summary>
    public virtual void UseAutoAttack()
    {
        // exit if entity is stunned or slept
        if (HasEffect(EffectType.STUN)) return;
        if (HasEffect(EffectType.SLEEP)) return;

        // exif if entity hasn't auto attacks
        if (autoAttacks.Count == 0) return;
        
        // use a random ability
        autoAttacks[selectedAutoAttack].Use(this);
        onIntentReset?.Invoke(this);

        // play Adapted Sound
    }
    
    public void UpdateHealth(int value)
    {
        // triggers function in scrollingFeedback.cs attached to each pawn to create a feedback text
        onStatusReceived?.Invoke(value.ToString().TrimStart('-'), gameSettings.colorTextDamage, false);

        if (!HasEffect(EffectType.STUN) && !HasEffect(EffectType.SLEEP))
        {
            animator.SetBool("IsActing", true);
            animator.SetTrigger("IsTakingDamage");
        }

        TimerToResetToIdleFighting(gameSettings.abilityAnimationDuration);

        // apply resistance, if it has effect
        if (HasEffect(EffectType.RESISTANCE))
        {
            // multiply the damage value by the resistance modifier
            value = Mathf.FloorToInt(value * gameSettings.damageResistanceModifier);
        }

        value += armor;
        if (value > 0)
        {
            DecreaseArmorTo(value);
            value = 0;
        }
        else
        {
            DecreaseArmorTo(0);
        }
        health += value;

        if (health <= 0)
        {
            health = 0;
            HandleDeath(); 
        }

        if (health > maxHealth)
        {
            health = maxHealth;
        }
        
        // triggers update display function in EntityUIDisplay.cs
        onStatsUpdate?.Invoke();
    }

    /// <summary>
    /// Apply the desired effect to this entity
    /// </summary>
    /// <param name="turnDuration">Number of turn this effect will last</param>
    public void ApplyEffect(EffectType effectType, int turnDuration)
    {
        // triggers function in scrollingFeedback.cs attached to each pawn to create a feedback text
        onStatusReceived?.Invoke(effectType.ToString(), gameSettings.colorTextEffect, true);

        // increments the effect if already exists
        foreach (Effect effect in effects)
        {
            if (effect.type == effectType)
            {
                effect.turnDuration += turnDuration;
                onStatsUpdate?.Invoke();
                return;
            }
        }
        
        // else, creates a new effect
        effects.Add(new Effect(effectType, turnDuration, id));

        // triggers update display function in EntityUIDisplay.cs
        onStatsUpdate?.Invoke();
    }
    
    /// <summary>
    /// Trigger all effects
    /// </summary>
    protected virtual void TriggerAllEffectsBeginTurn()
    {
        List<Effect> effectsToRemove = new List<Effect>();
        foreach (var effect in effects)
        {
            if ( effect.type == EffectType.DOT || effect.type == EffectType.HOT)
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


        if (!HasEffect(EffectType.STUN) && !HasEffect(EffectType.SLEEP))
        {
            animator.SetBool("IsSleeping", false);
        }

        // triggers update display function in EntityUIDisplay.cs
        onStatsUpdate?.Invoke();
    }

    protected virtual void TriggerAllEffectsEndTurn()
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


        if (!HasEffect(EffectType.STUN) && !HasEffect(EffectType.SLEEP))
        {
            animator.SetBool("IsSleeping", false);
        }

        // triggers update display function in EntityUIDisplay.cs
        onStatsUpdate?.Invoke();
    }

    public void ClearAllHarmfulEffects()
    {
        List<Effect> effectsToRemove = new List<Effect>();
        
        foreach (Effect effect in effects)
        {
            if (effect.isHarmful)
            {
                effectsToRemove.Add(effect);
            }
        }

        foreach (Effect effect in effectsToRemove)
        {
            effects.Remove(effect);
        }

        // triggers update display function in EntityUIDisplay.cs
        onStatsUpdate?.Invoke();
    }

    /// <summary>
    /// Browse all effects to check if this entity has the requested effect
    /// </summary>
    public bool HasEffect(EffectType effectType)
    {
        bool output = false;
        foreach (var effect in effects)
        {
            if (effect.type == effectType)
            {
                output = true;
                break;
            }
        }
        return output;
    }
    
    public virtual void HandleDeath()
    {
        stopAsync = true;
        // do nothing else in the parent
    }
    
    public void IncreaseArmor(int value)
    {
        int temporaryArmor = value;
        if (HasEffect(EffectType.BUFF_ARMOR))
        {
            temporaryArmor += gameSettings.buffArmorAmount;
        }
        if (HasEffect(EffectType.DEBUFF_ARMOR))
        {
            temporaryArmor -= gameSettings.debuffArmorAmout;
        }

        // triggers function in scrollingFeedback.cs attached to each pawn to create a feedback text
        onStatusReceived?.Invoke(value.ToString(), gameSettings.colorTextArmor, false);

        armor += temporaryArmor;

        // triggers update display function in EntityUIDisplay.cs
        onStatsUpdate?.Invoke();
    }
    
    // function used at the start of every entity's turn 
    public void ResetArmor()
    {
        armor = 0;
    }

    public void DecreaseArmorTo(int value)
    {
        armor = value;

        // triggers update display function in EntityUIDisplay.cs
        onStatsUpdate?.Invoke();
    }

    public void HealUpdate(int value)
    {
        int temporaryHeal = value;
        if (HasEffect(EffectType.ANTI_HEAL))
        {
            temporaryHeal -= gameSettings.antiHealAmout;
        }

        // triggers function in scrollingFeedback.cs attached to each pawn to create a feedback text
        onStatusReceived?.Invoke(value.ToString(), gameSettings.colorTextHeal, false);

        health += temporaryHeal;

        if (health > maxHealth)
        {
            health = maxHealth;
        }

        // triggers update display function in EntityUIDisplay.cs
        onStatsUpdate?.Invoke();
    }

    public void UpdateHealthPassArmor(int value)
    {
        if (HasEffect(EffectType.RESISTANCE))
        {
            // multiply the damage value by the resistance modifier
            value = Mathf.FloorToInt(value * gameSettings.damageResistanceModifier);
        }

        // triggers function in scrollingFeedback.cs attached to each pawn to create a feedback text
        onStatusReceived?.Invoke(value.ToString().TrimStart('-'), gameSettings.colorTextDamage, false);

        health += value;

        if (health <= 0)
        {
            health = 0;
            HandleDeath();
        }

        if (health > maxHealth)
        {
            health = maxHealth;
        }

        // triggers update display function in EntityUIDisplay.cs
        onStatsUpdate?.Invoke();
    }

    /// <summary>
    /// Update the entity position depending on the desired battle position
    /// </summary>
    /// <param name="battlePosition">Desired position</param>
    public virtual void UpdateBattlePosition(BattlePosition battlePosition)
    {
        this.battlePosition = battlePosition;
    }

    private async void TimerToResetToIdleFighting(float timerToWait)
    {
        await Task.Delay((int)(timerToWait * 1000));
        if (!stopAsync) animator.SetBool("IsActing", false);
    }
}

public enum BattlePosition
{
    FRONT = 0,
    MIDDLE,
    BACK
}