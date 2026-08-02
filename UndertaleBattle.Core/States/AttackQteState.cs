using UndertaleBattle.Core.Enums;
using UndertaleBattle.Core.Input;
using UndertaleBattle.Core.Interfaces;
using UndertaleBattle.Core.Runtime;

namespace UndertaleBattle.Core.States;

public sealed class AttackQteState : IBattleState
{
    private const float FlashDuration = 2f;

    private readonly float _meterSpeed;
    private readonly int _minimumDamage;
    private readonly int _maximumDamage;
    private readonly float _perfectZoneHalfWidth;

    private bool _resolved;

    public BattleStateIdentity Identity => BattleStateIdentity.AttackQte;

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

    public BattleStateIdentity? Enter(BattleSession session)
    {
        session.Ui.AttackQte.Reset();
        _resolved = false;

        return null;
    }

    public BattleStateIdentity? Update(
        BattleSession session,
        BattleInput input,
        float deltaTime)
    {
        if (_resolved)
        {
            session.Ui.AttackQte.TickFlash(deltaTime);

            if (!session.Ui.AttackQte.IsFlashing)
            {
                return session.IsComplete
                    ? BattleStateIdentity.Menu
                    : BattleStateIdentity.EnemyTurn;
            }

            return null;
        }

        session.Ui.AttackQte.AdvanceMeter(_meterSpeed * deltaTime);

        if (input.ConfirmPressed)
        {
            ResolveHit(session);
            return null;
        }

        return session.Ui.AttackQte.MeterPosition >= 1f
            ? BattleStateIdentity.EnemyTurn
            : null;
    }

    public void Exit(BattleSession session)
    {
        session.Ui.AttackQte.Reset();
    }

    private void ResolveHit(BattleSession session)
    {
        var enemy = session.Combat.CurrentEnemy;

        if (enemy is null || enemy.IsDead)
        {
            _resolved = true;
            return;
        }

        float distanceFromCenter =
            MathF.Abs(session.Ui.AttackQte.MeterPosition - 0.5f);

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
            session.Complete(BattleOutcome.EnemyDefeated);

        session.Ui.AttackQte.StartFlash(FlashDuration);
        _resolved = true;
    }
}