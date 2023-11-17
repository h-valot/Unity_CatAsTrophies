using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance;
    
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
                
                Registry.events.OnNewPlayerTurn?.Invoke();
                
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
                
                // new enemy turn
                Registry.events.OnNewEnemyTurn?.Invoke();
                
                // enemies uses their attacks
                await HandleEnemiesAttacks();

                // re-launched a new turn
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
        foreach (var Entity in catAttackQueue)
        {
            Entity.UseAutoAttack();
            await Task.Delay((int)Math.Round(Registry.gameSettings.abilityAnimationDuration * 1000));
        }
    }
    private async Task HandleEnemiesAttacks()
    {
        foreach (var battlePawn in BattlefieldManager.Instance.enemyBattlePawns)
        {
            Misc.GetEntityById(battlePawn.entityIdLinked).UseAutoAttack();
            await Task.Delay((int)Math.Round(Registry.gameSettings.abilityAnimationDuration * 1000));
        }
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