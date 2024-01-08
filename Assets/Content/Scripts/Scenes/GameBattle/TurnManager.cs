using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data;
using UI.GameBattle;
using UI.GameBattle.Menus;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance;

    [Header("REFERENCES")]
    public EndBattleManager endBattleManager;
    public DeckManager deckManager;
    public TurnMenu turnMenu;
    
    [Header("ENTITY ATTACKS SETUP")]
    public Vector3 offsetBattlePosition;
    public MeshRenderer backgroundFadeRenderer;

    [Header("DEBUGGING")]
    public TurnState state;
    public bool passTurn;
    public bool battleEnded;
    public List<Entity> catAttackQueue; // use to store reference of cats in for the attack phase

    private void Awake() => Instance = this;
    
    public void Initialize()
    {
        passTurn = false;
        battleEnded = false;
    }
    
    public async void HandleTurnState()
    {
        switch (state)
        {
            case TurnState.PLAYER_TURN:

                // clear the attack queue of all cat reference
                ClearCatAttackQueue();
                passTurn = false;

                // discard the hand and fulfill it back
                // if there is no more cats in the deck, shuffle the graveyard into the deck
                // handle the case where no cats can be draw
                FulfillHand(5);

                // display your turn title
                turnMenu.AnimateTitle("Your turn");

                // select autoatcks here to allow display of enemies intents sprites
                foreach (var battlePawn in BattlefieldManager.Instance.enemyBattlePawns)
                {
                    Entity enemyEntity = Misc.IdManager.GetEntityById(battlePawn.entityIdLinked);
                    
                    if (enemyEntity == null) continue;
                    if (enemyEntity.HasEffect(EffectType.STUN)) continue;
                    if (enemyEntity.HasEffect(EffectType.SLEEP)) continue;

                    enemyEntity.SelectAutoAttack();
                }

                // new car turn and timer to allow time for animations
                Registry.events.OnNewPlayerTurn?.Invoke();
                await Task.Delay((int)Math.Round((Registry.gameSettings.delayBeforeAnimation + Registry.gameSettings.abilityAnimationDuration + Registry.gameSettings.delayAfterAnimation) * 1000));

                // await for the player to perform three actions:
                // 1. place/replace cat on the battlefield to put it in the attack queue
                // 2. reactivate a cat that was already on the battlefield to put it on the attack queue
                await HandlePlayerActions();
                
                // discard the player's hand
                HandManager.Instance.DiscardHand();

                // cats use their attacks
                await HandleCatsAttacks();

                // handle effect end turn (stun, buff, etc.)
                Registry.events.OnEndPlayerTurn?.Invoke();

                // give the turn to the enemies
                state = TurnState.ENEMY_TURN;
                HandleTurnState();
                break;
            
            case TurnState.ENEMY_TURN:
                
                // new enemy turn and timer to allow time for animations
                Registry.events.OnNewEnemyTurn?.Invoke();
                await Task.Delay(Mathf.RoundToInt((Registry.gameSettings.delayBeforeAnimation + Registry.gameSettings.abilityAnimationDuration + Registry.gameSettings.delayAfterAnimation) * 1000));

                // enemies uses their attacks
                await HandleEnemiesAttacks();

                // handle effect end turn (stun, buff, etc.)
                Registry.events.OnEndEnemyTurn?.Invoke();

                // handle end battle, if all enemies are dead
                await HandleEndBattle();
                
                // start a new turn, if the battle isn't ended yet
                state = TurnState.PLAYER_TURN;
                if (!battleEnded) HandleTurnState();
                
                break;
        }
    }

    private void FulfillHand(int catCount)
    {
        for (int i = 0; i < catCount; i++)
        {
            if (deckManager.catsInDeck.Count > 0 ||
                DiscardManager.Instance.catsDiscarded.Count > 0)
            {
                HandManager.Instance.DrawCat(deckManager.RemoveCat());
                HandManager.Instance.ArrangeHand();
            }
        }
    }

    private async Task HandlePlayerActions()
    {
        while (catAttackQueue.Count < 3 && !passTurn && this)
        {
            //prevent soft lock when there is less than 3 cat in player deck
            if (HandManager.Instance.IsHandEmpty())
            {
                bool AllUsedBattlePawnAreLocked =true;
                int i = 0;
                foreach (var battlePawn in BattlefieldManager.Instance.catBattlePawns)
                {
                    if (!battlePawn.IsLocked() && battlePawn.entityIdLinked != "")
                    {
                        AllUsedBattlePawnAreLocked = false;
                    }
                    i++;
                }
                if (AllUsedBattlePawnAreLocked)
                {
                    passTurn = true;
                }
            }
            await Task.Delay(100);
        }
        await Task.Delay((int)Math.Round(Registry.gameSettings.abilityAnimationDuration * 1000));
    }

    /// <summary>
    /// For each battle pawn associated with a cat (the white circle on the battlefield),
    /// get the entityId and trigger the UseAutoAttack function
    /// </summary>
    private async Task HandleCatsAttacks()
    {
        //display the background fade
        backgroundFadeRenderer.enabled = true;

        foreach (Entity entity in catAttackQueue)
        {
            //skip this cat if it no longer exist
            if (entity == null)
            {
                continue;
            }

            //offset the cat that will attack to be in front of the background fade
            entity.transform.position += offsetBattlePosition;
            entity.isInFrontOfBackgroundFade = true;
            
            //select the autoattack that will be used by the entity
            entity.SelectAutoAttack();
            
            //Get every target ids of the selected ability and move them in front of the background fade
            List<string> involvedTargetIds = new List<string>();
            List<Entity> involvedTarget = new List<Entity>();
            involvedTargetIds = entity.GetAutoAttackTarget();
            foreach (String _targetId in involvedTargetIds)
            {
                involvedTarget.Add(Misc.IdManager.GetEntityById(_targetId));
            }
            foreach (Entity _target in involvedTarget)
            {
                if (!_target.isInFrontOfBackgroundFade)
                {
                    _target.transform.position += offsetBattlePosition;
                    _target.isInFrontOfBackgroundFade = true;
                }
            }

            //little delay to better understand the fight
            await Task.Delay((int)Math.Round(Registry.gameSettings.delayBeforeAnimation * 1000));

            //use the selected ability
            entity.UseAutoAttack();
            //wait for the end of ability animation + a delay
            await Task.Delay((int)Math.Round((Registry.gameSettings.abilityAnimationDuration + Registry.gameSettings.delayAfterAnimation) * 1000));
            
            //place the entity back behind the background fade
            entity.transform.position -= offsetBattlePosition;
            entity.isInFrontOfBackgroundFade = false;
            foreach (Entity _target in involvedTarget)
            {
                if (_target.isInFrontOfBackgroundFade && _target != null)
                {
                    _target.transform.position -= offsetBattlePosition;
                    _target.isInFrontOfBackgroundFade = false;
                }
            }
        }

        //disable background fade
        backgroundFadeRenderer.enabled = false;
    }

    private async Task HandleEnemiesAttacks()
    {
        //display the background fade
        backgroundFadeRenderer.enabled = true;
        
        // don't display the turn title if all enemies are dead
        if (!EnemyGenerator.Instance.allEnemiesDead) turnMenu.AnimateTitle("Enemy turn");

        foreach (var battlePawn in BattlefieldManager.Instance.enemyBattlePawns)
        {
            Entity _Entity = Misc.IdManager.GetEntityById(battlePawn.entityIdLinked);

            // skip this battlepawn if there is no entity on it
            if (_Entity == null) continue; 

            // offset the enemy that will attack to be in front of the background fade
            _Entity.transform.position += offsetBattlePosition;
            _Entity.isInFrontOfBackgroundFade = true;

            // select the autoattack that will be used by the entity
            // _Entity.SelectAutoAttack();

            // get every target ids of the selected ability and move them in front of the background fade
            List<string> involvedTargetIds = new List<string>();
            List<Entity> involvedTarget = new List<Entity>();
            involvedTargetIds = _Entity.GetAutoAttackTarget();
            foreach (String _targetId in involvedTargetIds)
            {
                involvedTarget.Add(Misc.IdManager.GetEntityById(_targetId));
            }
            foreach (Entity _target in involvedTarget)
            {
                if (!_target.isInFrontOfBackgroundFade)
                {
                    _target.transform.position += offsetBattlePosition;
                    _target.isInFrontOfBackgroundFade = true;
                }
            }

            // little delay to better understand the fight
            await Task.Delay((int)Math.Round(Registry.gameSettings.delayBeforeAnimation * 1000));

            // use the selected ability
            _Entity.UseAutoAttack();
            // wait for the end of ability animation + a delay
            await Task.Delay((int)Math.Round((Registry.gameSettings.abilityAnimationDuration + Registry.gameSettings.delayAfterAnimation) * 1000));

            // place the entity back behind the background fade
            _Entity.transform.position -= offsetBattlePosition;
            _Entity.isInFrontOfBackgroundFade = false;
            foreach (Entity _target in involvedTarget)
            {
                if (_target.isInFrontOfBackgroundFade)
                {
                    _target.transform.position -= offsetBattlePosition;
                    _target.isInFrontOfBackgroundFade = false;
                }

            }
        }

        // disable background fade
        backgroundFadeRenderer.enabled = false;
    }

    /// <summary>
    /// Triggered by cat.cs when cat are placed on battlefield or when reactivated
    /// </summary>
    public int AddCatAttackQueue(Entity catReference)
    {
        catAttackQueue.Add(catReference);
        return (catAttackQueue.Count);
    }
    
    private void ClearCatAttackQueue() => catAttackQueue.Clear();
    
    private async Task HandleEndBattle()
    {
        // exit, if the debug mode is enabled
        if (Registry.gameSettings.gameBattleDebugMode) return;
        
        if (EnemyGenerator.Instance.allEnemiesDead)
        {
            battleEnded = true;
            DataManager.data.playerStorage.SynchronizeCatData(CatManager.Instance.cats);
            DataManager.data.endBattleStatus = EndBattleStatus.VICTORY;
            await endBattleManager.AnimateEndBattle();
        }
        
        if (CatManager.Instance.allCatsDead)
        {
            battleEnded = true;
            DataManager.data.endBattleStatus = EndBattleStatus.DEFEATED;
            await endBattleManager.AnimateEndTitle();
            SceneManager.LoadScene("mainmenu");
        }
    }
}

public enum TurnState
{
    PLAYER_TURN = 0,
    ENEMY_TURN
}