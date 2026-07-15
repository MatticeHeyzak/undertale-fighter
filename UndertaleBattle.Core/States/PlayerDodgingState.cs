using System.Numerics;
using UndertaleBattle.Core.Context;
using UndertaleBattle.Core.Enums;
using UndertaleBattle.Core.Interfaces;

namespace UndertaleBattle.Core.States;

/// <summary>
/// Player moves the soul, bullets tick, collision is checked.
/// Input direction is injected via <see cref="BattleContext.MovementInput"/>.
/// </summary>
public sealed class PlayerDodgingState : IBattleState
{
    public BattleStateIdentity Identity => BattleStateIdentity.PlayerDodging;

    private readonly float _phaseDuration;
    private float _elapsed;

    public PlayerDodgingState(float phaseDuration = 6f)
    {
        _phaseDuration = phaseDuration;
    }

    public void Enter(BattleContext context)
    {
        _elapsed = 0f;
        context.Bullets.Clear();
    }

    public void Update(BattleContext context, float deltaTime)
    {
        _elapsed += deltaTime;

        MovePlayer(context, deltaTime);
        TickBullets(context, deltaTime);
        CheckCollisions(context);

        if (_elapsed >= _phaseDuration || context.PlayerSoul.IsDead)
            context.StateMachine.ChangeState(BattleStateIdentity.Menu, context);
    }

    public void Exit(BattleContext context)
    {
        context.Bullets.Clear();
    }

    private static void MovePlayer(BattleContext context, float deltaTime)
    {
        context.PlayerSoul.Move(context.MovementInput, deltaTime);
        context.PlayerSoul.ClampTo(context.Arena); // extension added to HeartSoul
        context.PlayerSoul.TickInvulnerability(deltaTime);
    }

    private static void TickBullets(BattleContext context, float deltaTime)
    {
        foreach (var bullet in context.Bullets)
            bullet.Update(deltaTime);

        context.Bullets.RemoveAll(b => !b.IsAlive || IsOutOfArena(b, context));
    }

    private static void CheckCollisions(BattleContext context)
    {
        var soul = context.PlayerSoul;
        foreach (var bullet in context.Bullets)
        {
            float dist = Vector2.Distance(soul.Position, bullet.Position);
            if (dist < bullet.Radius + soul.Radius)
            {
                soul.TakeDamage(bullet.Damage, invulnerabilitySeconds: 1.5f);
                bullet.IsAlive = false;
            }
        }
    }

    private static bool IsOutOfArena(Models.Bullet b, BattleContext ctx) =>
        b.Position.X < ctx.Arena.Left  - 20 ||
        b.Position.X > ctx.Arena.Right + 20 ||
        b.Position.Y < ctx.Arena.Top   - 20 ||
        b.Position.Y > ctx.Arena.Bottom + 20;
}