using UndertaleBattle.Core.Runtime;

namespace UndertaleBattle.Core.Interfaces;

/// <summary>
/// Controls one active enemy attack execution.
///
/// An attack owns its spawning schedule and completion policy. It remains
/// presentation-free and may create hazards through the battle runtime.
/// </summary>
public interface IAttackPattern
{
    /// <summary>
    /// True when the player-dodging phase may safely end.
    /// </summary>
    bool IsComplete { get; }

    /// <summary>
    /// Runs once after the attack becomes active.
    /// </summary>
    void Enter(BattleSession session);

    /// <summary>
    /// Advances the attack's own timers and spawning behavior.
    /// </summary>
    void Update(BattleSession session, float deltaTime);
}