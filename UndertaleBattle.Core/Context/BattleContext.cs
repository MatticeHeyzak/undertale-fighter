using System.Numerics;
using UndertaleBattle.Core.Context.StateData;
using UndertaleBattle.Core.Enums;
using UndertaleBattle.Core.Interfaces;
using UndertaleBattle.Core.Models;

namespace UndertaleBattle.Core.Context;

public class BattleContext
{
    public HeartSoul PlayerSoul { get; }
    public BattleStateIdentity CurrentState { get; internal set; }
    public bool BattleOver { get; set; }
    
    public MenuInput PendingMenuInput { get; set; }
    public Vector2 MovementInput { get; set; }
    
    public IArenaShape Arena { get; }
    public List<Bullet> Bullets { get; } = new();
    public List<Item> Inventory { get; } = new();
    public Enemy? CurrentEnemy { get; set; }
    public IAttackPattern? CurrentAttackPattern { get; set; }
    
    // Per-state scratch data.
    public MenuStateData Menu { get; } = new();
    public DialogueStateData Dialogue { get; } = new();
    public AttackQteStateData AttackQte { get; } = new();
    
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
        Dialogue.CurrentDialog = text;
        Dialogue.NextState = nextState;
        StateMachine.ChangeState(BattleStateIdentity.TextDialogue, this);
    }
}