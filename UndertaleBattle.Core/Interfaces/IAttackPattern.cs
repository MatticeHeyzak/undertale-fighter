using UndertaleBattle.Core.Runtime;

namespace UndertaleBattle.Core.Interfaces;

/// <summary>
/// Controls projectile spawning for one active enemy attack.
/// Patterns remain presentation-free.
/// </summary>
public interface IAttackPattern
{
    bool IsFinished { get; }

    void Enter(BattleSession session);

    void Update(BattleSession session, float deltaTime);
}