using UndertaleBattle.Core.Enums;
using UndertaleBattle.Core.Input;
using UndertaleBattle.Core.Interfaces;
using UndertaleBattle.Core.Runtime;

namespace UndertaleBattle.Core.States;

/// <summary>
/// Spawns the enemy attack pattern, then transitions to <see cref="BattleStateIdentity.PlayerDodging"/>.
/// Pattern logic is delegated to <see cref="IAttackPattern"/> — scalable for future enemy variety.
/// </summary>
public sealed class EnemyTurnState : IBattleState
{
    private readonly IAttackPattern _pattern;

    public BattleStateIdentity Identity => BattleStateIdentity.EnemyTurn;

    public EnemyTurnState(IAttackPattern pattern)
    {
        _pattern = pattern ?? throw new ArgumentNullException(nameof(pattern));
    }

    public BattleStateIdentity? Enter(BattleSession session)
    {
        session.Combat.BeginAttack(_pattern);
        _pattern.Enter(session);

        return BattleStateIdentity.PlayerDodging;
    }

    public BattleStateIdentity? Update(
        BattleSession session,
        BattleInput input,
        float deltaTime)
    {
        return null;
    }

    public void Exit(BattleSession session)
    {
    }
}