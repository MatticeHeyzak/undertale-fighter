using UndertaleBattle.Core.Enums;
using UndertaleBattle.Core.Input;
using UndertaleBattle.Core.Runtime;

namespace UndertaleBattle.Core.Interfaces;

/// <summary>
/// A high-level battle-flow state.
///
/// States coordinate flow and delegate recurring mechanics to systems.
/// They do not own the battle session's input or a state machine reference.
/// </summary>
public interface IBattleState
{
    BattleStateIdentity Identity { get; }

    /// <summary>
    /// Runs once when this state becomes active.
    /// Return a state identity for an immediate flow transition, otherwise null.
    /// </summary>
    BattleStateIdentity? Enter(BattleSession session);

    /// <summary>
    /// Updates this state once. Return a state identity to request a transition.
    /// </summary>
    BattleStateIdentity? Update(
        BattleSession session,
        BattleInput input,
        float deltaTime);

    /// <summary>
    /// Runs once before another state becomes active.
    /// </summary>
    void Exit(BattleSession session);
}