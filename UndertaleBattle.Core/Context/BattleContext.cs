using System.Numerics;
using UndertaleBattle.Core.Enums;
using UndertaleBattle.Core.Interfaces;
using UndertaleBattle.Core.Models;

namespace UndertaleBattle.Core.Context;

public class BattleContext
{
    public HeartSoul PlayerSoul { get; }
    
    public BattleStateIdentity CurrentState { get; internal set; }
    
    public int SelectedMenuIndex { get; set; }
    
    public bool BattleOver { get; set; }

    public string CurrentDialog { get; set; } = string.Empty;
    
    /// <summary>
    /// State to transition to once the current dialogue is fully read and confirmed.
    /// Set this right before switching to <see cref="BattleStateIdentity.TextDialogue"/>.
    /// </summary>
    public BattleStateIdentity DialogueNextState { get; set; }

    public MenuInput PendingMenuInput { get; set; }
    public int VisibleDialogCharCount { get; set; }
    public Vector2 MovementInput { get; set; }

    public BattleArena Arena { get; }
    public List<Bullet> Bullets { get; } = new();
    public List<Item> Inventory { get; } = new();
    public Enemy? CurrentEnemy { get; set; }
    
    public float AttackMeterPosition { get; set; }
    
    public float AttackFlashTimer { get; set; }
    
    public IBattleStateMachine StateMachine { get; }

    public BattleContext(IBattleStateMachine stateMachine, HeartSoul playerSoul, BattleArena arena)
    {
        StateMachine = stateMachine;
        PlayerSoul = playerSoul;
        Arena = arena;
    }

    /// <summary>
    /// Convenience helper: shows a line of dialogue, then routes to <paramref name="nextState"/> on confirm.
    /// </summary>
    public void ShowDialogue(string text, BattleStateIdentity nextState)
    {
        CurrentDialog = text;
        DialogueNextState = nextState;
        StateMachine.ChangeState(BattleStateIdentity.TextDialogue, this);
    }
}