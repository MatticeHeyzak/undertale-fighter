using UndertaleBattle.Core.Context;
using UndertaleBattle.Core.Enums;
using UndertaleBattle.Core.Interfaces;

namespace UndertaleBattle.Core.States;

/// <summary>
/// Runs the player's attack timing minigame and applies damage to the active enemy.
/// </summary>
public sealed class AttackQteState : IBattleState
{
    public BattleStateIdentity Identity => BattleStateIdentity.AttackQte;

    private const float FlashDuration = 2f;

    private readonly float _meterSpeed;
    private readonly int _minimumDamage;
    private readonly int _maximumDamage;
    private readonly float _perfectZoneHalfWidth;

    private bool _resolved;

    public AttackQteState(
        float meterSpeed = 1f,
        int minimumDamage = 2,
        int maximumDamage = 15,
        float perfectZoneHalfWidth = 0.06f)
    {
        if (meterSpeed <= 0f)
            throw new ArgumentOutOfRangeException(nameof(meterSpeed));

        if (minimumDamage < 0)
            throw new ArgumentOutOfRangeException(nameof(minimumDamage));

        if (maximumDamage < minimumDamage)
            throw new ArgumentOutOfRangeException(nameof(maximumDamage));

        if (perfectZoneHalfWidth is <= 0f or > 0.5f)
            throw new ArgumentOutOfRangeException(nameof(perfectZoneHalfWidth));

        _meterSpeed = meterSpeed;
        _minimumDamage = minimumDamage;
        _maximumDamage = maximumDamage;
        _perfectZoneHalfWidth = perfectZoneHalfWidth;
    }

    public void Enter(BattleContext context)
    {
        context.AttackQte.Reset();
        context.ClearTransientInput();
        _resolved = false;
    }

    public void Update(BattleContext context, float deltaTime)
    {
        if (_resolved)
        {
            context.AttackQte.TickFlash(deltaTime);

            if (!context.AttackQte.IsResolving)
                AdvanceToEnemyTurn(context);

            return;
        }

        context.AttackQte.AdvanceMeter(_meterSpeed * deltaTime);

        if (context.PendingMenuInput == MenuInput.Confirm)
        {
            context.ClearTransientInput();
            ResolveHit(context);
            return;
        }

        if (context.AttackQte.MeterPosition >= 1f)
            AdvanceToEnemyTurn(context);
    }

    public void Exit(BattleContext context)
    {
    }

    private void ResolveHit(BattleContext context)
    {
        var enemy = context.CurrentEnemy;

        // An enemy could have been removed by another battle effect while
        // this state was active. Do not attempt to damage a null/dead target.
        if (enemy is null || enemy.IsDead)
        {
            AdvanceToEnemyTurn(context);
            return;
        }

        float distanceFromCenter =
            MathF.Abs(context.AttackQte.MeterPosition - 0.5f);

        float accuracy = Math.Clamp(
            1f - distanceFromCenter / 0.5f,
            0f,
            1f);

        int damage = distanceFromCenter <= _perfectZoneHalfWidth
            ? _maximumDamage
            : _minimumDamage +
              (int)((_maximumDamage - _minimumDamage) * accuracy);

        enemy.TakeDamage(damage);
        
        if (enemy.IsDead)
            context.BattleOver = true;

        context.AttackQte.StartFlash(FlashDuration);
        _resolved = true;
    }

    private static void AdvanceToEnemyTurn(BattleContext context)
    {
        context.AttackQte.Reset();

        BattleStateIdentity nextState = context.BattleOver
            ? BattleStateIdentity.Menu
            : BattleStateIdentity.EnemyTurn;

        context.StateMachine.ChangeState(nextState, context);
    }
}