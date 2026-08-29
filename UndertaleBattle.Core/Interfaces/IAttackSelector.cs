using UndertaleBattle.Core.Runtime;

namespace UndertaleBattle.Core.Interfaces;

/// <summary>
/// Selects and creates a fresh attack execution for an enemy turn.
/// Implementations may later use health thresholds, attack history, a seeded
/// random source, or boss phase state.
/// </summary>
public interface IAttackSelector
{
    IAttackPattern CreateNextAttack(BattleSession session);
}