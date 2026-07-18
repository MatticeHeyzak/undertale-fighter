using System.Numerics;
using UndertaleBattle.Core.Context;
using UndertaleBattle.Core.Interfaces;
using UndertaleBattle.Core.Models;

namespace UndertaleBattle.Core.Patterns;

public sealed class BarragePattern : IAttackPattern
{
    private readonly int _bulletCount;
    private readonly float _speed;
    private readonly int _damage;
    
    public BarragePattern(int bulletCount = 5, float speed = 180f, int damage = 4)
    {
        _bulletCount = bulletCount;
        _speed       = speed;
        _damage      = damage;
    }
    
    public void Spawn(BattleContext context)
    {
        float step = context.Arena.Height / (_bulletCount + 1);

        for (int i = 1; i <= _bulletCount; i++)
        {
            context.Bullets.Add(new Bullet
            {
                Position = new Vector2(context.Arena.Left - 10, context.Arena.Top + step * i),
                Velocity = new Vector2(_speed, 0),
                Radius   = 6f,
                Damage   = _damage
            });
        }
    }
}