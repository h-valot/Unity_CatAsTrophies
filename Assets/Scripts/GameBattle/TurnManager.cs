using NUnit.Framework;
using NUnit.Framework.Internal.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance;
    [Header("SETUP")]
    public Vector3 OffsetBattlePosition;
    public MeshRenderer backgroundFadeRenderer;

    [Header("DEBUGGING")]
    public TurnState state;
    public int turnCounter;
    public bool passTurn;
    public List<Entity> catAttackQueue; // Use to store reference of cats in for the attack phase

    private void Awake() => Instance = this;
    
    public void Initialize()
    {
        turnCounter = 0;
        passTurn = false;
    }
    
    public async void HandleTurnState()
    {
        switch (state)
        {
            case TurnState.PlayerTurn:
                
                turnCounter++;

                // clear the attack queue of all cat reference
                clearCatAttackQueue();
                passTurn = false;

                // discard the hand and fulfill it back
                // if there is no more cats in the deck, shuffle the graveyard into the deck
                // handle the case where no cats can be draw
                FulfillHand(5);

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
                
                // give the turn to the enemies
                state = TurnState.EnemyTurn;
                HandleTurnState();
                
                break;
            
            case TurnState.EnemyTurn:
                
                // new enemy turn and timer to allow time for animations
                Registry.events.OnNewEnemyTurn?.Invoke();
                await Task.Delay((int)Math.Round((Registry.gameSettings.delayBeforeAnimation + Registry.gameSettings.abilityAnimationDuration + Registry.gameSettings.delayAfterAnimation) * 1000));

                // enemies uses their attacks
                await HandleEnemiesAttacks();

                // start a new turn
                state = TurnState.PlayerTurn;
                HandleTurnState();
                
                break;
        }
    }

    private void FulfillHand(int _catCount)
    {
        for (int i = 0; i < _catCount; i++)
        {
            if (DeckManager.Instance.catsInDeck.Count > 0 ||
                DiscardManager.Instance.catsDiscarded.Count > 0)
            {
                HandManager.Instance.DrawCat(DeckManager.Instance.RemoveCat());
                HandManager.Instance.ArrangeHand();
            }
        }
    }

    private async Task HandlePlayerActions()
    {
        while (catAttackQueue.Count < 3 || passTurn)
        {
            await Task.Delay(100);
        }
        await Task.Delay((int)Math.Round(Registry.gameSettings.abilityAnimationDuration * 1000));
    }

    //For each battle pawn associated with a cat (the white circle on the battlefield), get the entityId and trigger the UseAutoAttack function
    private async Task HandleCatsAttacks()
    {
        //display the backgroud fade
        backgroundFadeRenderer.enabled = true;

        foreach (Entity _Entity in catAttackQueue)
        {
            //skip this cat if it no longer exist
            if (_Entity == null)
            {
                continue;
            }

            //offset the cat that will attack to be in front of the background fade
            _Entity.transform.position += OffsetBattlePosition;
            _Entity.isInFrontOfBackgroundFade = true;
            
            //select the autoattack that will be used by the entity
            _Entity.SelectAutoAttack();
            
            //Get every target ids of the selected ability and move them in front of the background fade
            List<string> involvedTargetIds = new List<string>();
            List<Entity> involvedTarget = new List<Entity>();
            involvedTargetIds = _Entity.GetAutoAttackTarget();
            foreach (String _targetId in involvedTargetIds)
            {
                involvedTarget.Add(Misc.GetEntityById(_targetId));
            }
            foreach (Entity _target in involvedTarget)
            {
                if (!_target.isInFrontOfBackgroundFade)
                {
                    _target.transform.position += OffsetBattlePosition;
                    _target.isInFrontOfBackgroundFade = true;
                }
            }

            //little delay to better understand the fight
            await Task.Delay((int)Math.Round(Registry.gameSettings.delayBeforeAnimation * 1000));

            //use the selected ability
            _Entity.UseAutoAttack();
            //wait for the end of ability animation + a delay
            await Task.Delay((int)Math.Round((Registry.gameSettings.abilityAnimationDuration + Registry.gameSettings.delayAfterAnimation) * 1000));
            
            //place the entity back behind the background fade
            _Entity.transform.position -= OffsetBattlePosition;
            _Entity.isInFrontOfBackgroundFade = false;
            foreach (Entity _target in involvedTarget)
            {
                if (_target.isInFrontOfBackgroundFade && _target != null)
                {
                    _target.transform.position -= OffsetBattlePosition;
                    _target.isInFrontOfBackgroundFade = false;
                }
            }
        }

        //disable background fade
        backgroundFadeRenderer.enabled = false;
    }

    private async Task HandleEnemiesAttacks()
    {
        //display the backgroud fade
        backgroundFadeRenderer.enabled = true;

        foreach (var battlePawn in BattlefieldManager.Instance.enemyBattlePawns)
        {
            Entity _Entity = Misc.GetEntityById(battlePawn.entityIdLinked);

            //skip this battlepawn if there is no entity on it
            if (_Entity == null)
            {
                continue; 
            }

            //offset the enemy that will attack to be in front of the background fade
            _Entity.transform.position += OffsetBattlePosition;
            _Entity.isInFrontOfBackgroundFade = true;

            //select the autoattack that will be used by the entity
            _Entity.SelectAutoAttack();

            //Get every target ids of the selected ability and move them in front of the background fade
            List<string> involvedTargetIds = new List<string>();
            List<Entity> involvedTarget = new List<Entity>();
            involvedTargetIds = _Entity.GetAutoAttackTarget();
            foreach (String _targetId in involvedTargetIds)
            {
                involvedTarget.Add(Misc.GetEntityById(_targetId));
            }
            foreach (Entity _target in involvedTarget)
            {
                if (!_target.isInFrontOfBackgroundFade)
                {
                    _target.transform.position += OffsetBattlePosition;
                    _target.isInFrontOfBackgroundFade = true;
                }
            }

            //little delay to better understand the fight
            await Task.Delay((int)Math.Round(Registry.gameSettings.delayBeforeAnimation * 1000));

            //use the selected ability
            _Entity.UseAutoAttack();
            //wait for the end of ability animation + a delay
            await Task.Delay((int)Math.Round((Registry.gameSettings.abilityAnimationDuration + Registry.gameSettings.delayAfterAnimation) * 1000));

            //place the entity back behind the background fade
            _Entity.transform.position -= OffsetBattlePosition;
            _Entity.isInFrontOfBackgroundFade = false;
            foreach (Entity _target in involvedTarget)
            {
                if (_target.isInFrontOfBackgroundFade)
                {
                    _target.transform.position -= OffsetBattlePosition;
                    _target.isInFrontOfBackgroundFade = false;
                }

            }
        }

        //disable background fade
        backgroundFadeRenderer.enabled = false;
    }

    //Triggered by cat.cs when cat are placed on battlefield or when reactivated
    public int addCatAttackQueue (Entity _CatReference)
    {
        catAttackQueue.Add(_CatReference);
        return (catAttackQueue.Count);
    }

    //Triggered at the start of the player turn
    public void clearCatAttackQueue()
    {
        catAttackQueue.Clear();
    }
}

public enum TurnState
{
    PlayerTurn = 0,
    EnemyTurn
}