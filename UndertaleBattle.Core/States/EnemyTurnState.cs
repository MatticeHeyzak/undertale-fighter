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
    private readonly IAttackSelector _attackSelector;

    public BattleStateIdentity Identity => BattleStateIdentity.EnemyTurn;

    public EnemyTurnState(IAttackSelector attackSelector)
    {
        _attackSelector = attackSelector ??
                          throw new ArgumentNullException(nameof(attackSelector));
    }

    public BattleStateIdentity? Enter(BattleSession session)
    {
        ArgumentNullException.ThrowIfNull(session);

        IAttackPattern attack = _attackSelector.CreateNextAttack(session);

        session.Combat.BeginAttack(attack);
        attack.Enter(session);

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