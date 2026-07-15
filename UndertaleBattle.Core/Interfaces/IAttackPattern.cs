using UndertaleBattle.Core.Context;

namespace UndertaleBattle.Core.Interfaces;

/// <summary>
/// Implement this to create a new enemy attack pattern.
/// Patterns only populate <see cref="BattleContext.Bullets"/> — no rendering.
/// </summary>
public interface IAttackPattern
{
    void Spawn(BattleContext context);
}