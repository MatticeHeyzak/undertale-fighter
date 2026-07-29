using System.Numerics;
using UndertaleBattle.Core.Context.StateData;
using UndertaleBattle.Core.Enums;
using UndertaleBattle.Core.Interfaces;
using UndertaleBattle.Core.Models;

namespace UndertaleBattle.Core.Context;

/// <summary>
/// Mutable runtime state for one active battle.
///
/// This class owns battle-wide state only. State-specific transient data belongs
/// in the corresponding object under <see cref="Menu"/>, <see cref="Dialogue"/>,
/// or <see cref="AttackQte"/>.
/// </summary>
public sealed class BattleContext
{
    public HeartSoul PlayerSoul { get; }

    public IArenaShape Arena { get; }

    public Enemy? CurrentEnemy { get; set; }

    public List<Bullet> Bullets { get; } = new();

    public List<Item> Inventory { get; } = new();

    /// <summary>
    /// The pattern currently controlling the enemy's attack phase.
    /// It is assigned by <see cref="States.EnemyTurnState"/> and updated by
    /// <see cref="States.PlayerDodgingState"/>.
    /// </summary>
    public IAttackPattern? CurrentAttackPattern { get; set; }

    public bool BattleOver { get; set; }

    /// <summary>
    /// Continuous directional input for the current frame.
    /// Written by the application layer; consumed by gameplay states.
    /// </summary>
    public Vector2 MovementInput { get; set; }

    /// <summary>
    /// One-shot menu/action input for the current frame.
    /// A state that handles it must reset it to <see cref="MenuInput.None"/>.
    /// </summary>
    public MenuInput PendingMenuInput { get; set; }

    /// <summary>
    /// Per-state data owned by <see cref="States.MenuState"/> and its renderer.
    /// </summary>
    public MenuStateData Menu { get; } = new();

    /// <summary>
    /// Per-state data owned by <see cref="States.TextDialogueState"/> and its renderer.
    /// </summary>
    public DialogueStateData Dialogue { get; } = new();

    /// <summary>
    /// Per-state data owned by <see cref="States.AttackQteState"/> and its renderer.
    /// </summary>
    public AttackQteStateData AttackQte { get; } = new();

    /// <summary>
    /// The single authority for battle-state transitions and the current state.
    /// </summary>
    public IBattleStateMachine StateMachine { get; }

    /// <summary>
    /// Convenience read-only view for renderers and diagnostics.
    /// The state machine is the single source of truth.
    /// </summary>
    public BattleStateIdentity CurrentState =>
        StateMachine.CurrentState?.Identity
        ?? throw new InvalidOperationException(
            "The current battle state was requested before a state was activated.");

    public BattleContext(
        IBattleStateMachine stateMachine,
        HeartSoul playerSoul,
        IArenaShape arena)
    {
        StateMachine = stateMachine ?? throw new ArgumentNullException(nameof(stateMachine));
        PlayerSoul = playerSoul ?? throw new ArgumentNullException(nameof(playerSoul));
        Arena = arena ?? throw new ArgumentNullException(nameof(arena));
    }

    /// <summary>
    /// Sets up a dialogue line and transitions into the reusable dialogue state.
    /// </summary>
    public void ShowDialogue(string text, BattleStateIdentity nextState)
    {
        ArgumentNullException.ThrowIfNull(text);

        Dialogue.Begin(text, nextState);
        StateMachine.ChangeState(BattleStateIdentity.TextDialogue, this);
    }

    /// <summary>
    /// Clears transient input so an input from one state cannot unintentionally
    /// be consumed immediately by the next state.
    /// </summary>
    public void ClearTransientInput()
    {
        PendingMenuInput = MenuInput.None;
    }
}