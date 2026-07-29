using UndertaleBattle.Core.Context;
using UndertaleBattle.Core.Enums;
using UndertaleBattle.Core.Interfaces;

namespace UndertaleBattle.Core.States;

/// <summary>
/// Spawns the enemy attack pattern, then transitions to <see cref="BattleStateIdentity.PlayerDodging"/>.
/// Pattern logic is delegated to <see cref="IAttackPattern"/> — scalable for future enemy variety.
/// </summary>
public sealed class EnemyTurnState : IBattleState
{
    public BattleStateIdentity Identity => BattleStateIdentity.EnemyTurn;

    private readonly IAttackPattern _pattern;

    public EnemyTurnState(IAttackPattern pattern) => _pattern = pattern;

    public void Enter(BattleContext context)
    {
        _pattern.Enter(context);
        context.CurrentAttackPattern = _pattern;
        context.StateMachine.ChangeState(BattleStateIdentity.PlayerDodging, context);
    }

    public void Update(BattleContext context, float deltaTime) { }
    public void Exit(BattleContext context) { }
}