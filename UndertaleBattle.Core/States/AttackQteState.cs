using UndertaleBattle.Core.Context;
using UndertaleBattle.Core.Enums;
using UndertaleBattle.Core.Interfaces;

namespace UndertaleBattle.Core.States;

public sealed class AttackQteState :  IBattleState
{
    public BattleStateIdentity Identity => BattleStateIdentity.AttackQte;

    private const float FlashDuration = 2f;

    private readonly float _meterSpeed;
    private readonly int _minDamage;
    private readonly int _maxDamage;
    private readonly float _perfectZoneHalfWidth;

    private bool _resolved;

    public AttackQteState(
        float meterSpeed = 1f,
        int minDamage = 2,
        int maxDamage = 15,
        float perfectZoneHalfWidth = 0.06f)
    {
        _meterSpeed = meterSpeed;
        _minDamage = minDamage;
        _maxDamage = maxDamage;
        _perfectZoneHalfWidth = perfectZoneHalfWidth;
    }

    public void Enter(BattleContext context)
    {
        context.AttackQte.MeterPosition = 0f;
        context.AttackQte.FlashTimer = 0f;
        _resolved = false;
    }

    public void Update(BattleContext context, float deltaTime)
    {
        var qte = context.AttackQte;

        if (_resolved)
        {
            qte.FlashTimer -= deltaTime;
            if (qte.FlashTimer <= 0f)
                Advance(context);
            return;
        }

        AdvanceMeter(context, deltaTime);

        if (context.PendingMenuInput == MenuInput.Confirm)
        {
            context.PendingMenuInput = MenuInput.None;
            ResolveHit(context);
        }
        else if (qte.MeterPosition >= 0.99f)
            Advance(context);
    }
    
    public void Exit(BattleContext context) { }
    
    private void AdvanceMeter(BattleContext context, float deltaTime) =>
        context.AttackQte.MeterPosition += _meterSpeed * deltaTime;

    private void ResolveHit(BattleContext context)
    {
        float distanceFromCenter = Math.Abs(context.AttackQte.MeterPosition - 0.5f);
        float accuracy = Math.Clamp(1f - distanceFromCenter / 0.5f, 0f, 1f);

        int damage = distanceFromCenter <= _perfectZoneHalfWidth
            ? _maxDamage
            : _minDamage + (int)((_maxDamage - _minDamage) * accuracy);

        context.CurrentEnemy?.TakeDamage(damage);
        context.AttackQte.FlashTimer = FlashDuration;
        _resolved = true;
    }

    private static void Advance(BattleContext context)
    {
        context.AttackQte.FlashTimer = 0;
        context.StateMachine.ChangeState(BattleStateIdentity.EnemyTurn, context);
    }
}