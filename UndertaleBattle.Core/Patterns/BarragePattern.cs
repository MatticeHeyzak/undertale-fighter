using System.Numerics;
using UndertaleBattle.Core.Interfaces;
using UndertaleBattle.Core.Models;
using UndertaleBattle.Core.Runtime;

namespace UndertaleBattle.Core.Patterns;

/// <summary>
/// Spawns one horizontal barrage, then keeps the dodge phase active for the
/// configured duration.
/// </summary>
public sealed class BarragePattern : IAttackPattern
{
    private readonly int _bulletCount;
    private readonly float _speed;
    private readonly int _damage;
    private readonly float _duration;

    private float _elapsed;
    private bool _spawned;

    public BarragePattern(
        int bulletCount = 5,
        float speed = 180f,
        int damage = 4,
        float duration = 6f)
    {
        if (bulletCount <= 0)
            throw new ArgumentOutOfRangeException(nameof(bulletCount));

        if (speed <= 0f)
            throw new ArgumentOutOfRangeException(nameof(speed));

        if (damage <= 0)
            throw new ArgumentOutOfRangeException(nameof(damage));

        if (duration <= 0f)
            throw new ArgumentOutOfRangeException(nameof(duration));

        _bulletCount = bulletCount;
        _speed = speed;
        _damage = damage;
        _duration = duration;
    }

    public bool IsComplete => _spawned && _elapsed >= _duration;

    public void Enter(BattleSession session)
    {
        ArgumentNullException.ThrowIfNull(session);

        _elapsed = 0f;
        _spawned = false;

        Spawn(session);
        _spawned = true;
    }

    public void Update(BattleSession session, float deltaTime)
    {
        ArgumentNullException.ThrowIfNull(session);

        if (deltaTime < 0f)
            throw new ArgumentOutOfRangeException(nameof(deltaTime));

        _elapsed += deltaTime;
    }

    private void Spawn(BattleSession session)
    {
        var arena = session.Arena.Shape;
        float step = arena.Height / (_bulletCount + 1);

        for (int index = 1; index <= _bulletCount; index++)
        {
            session.Combat.SpawnProjectile(new Bullet
            {
                Position = new Vector2(
                    arena.Left - 10f,
                    arena.Top + step * index),
                Velocity = new Vector2(_speed, 0f),
                Radius = 6f,
                Damage = _damage
            });
        }
    }
}