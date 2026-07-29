using System.Numerics;
using UndertaleBattle.Core.Context;
using UndertaleBattle.Core.Interfaces;
using UndertaleBattle.Core.Models;

namespace UndertaleBattle.Core.Patterns;

public class WavePattern : IAttackPattern
{
    private readonly int _waveCount;
    private readonly float _interval;
    private readonly float _speed;
    private readonly int _damage;

    private int _spawnedWaves;
    private float _elapsedSinceLastSpawn;

    public WavePattern(int waveCount = 6, float interval = 0.4f, float speed = 160f, int damage = 5)
    {
        _waveCount = waveCount;
        _interval = interval;
        _speed = speed;
        _damage = damage;
    }
    
    public bool IsFinished => _spawnedWaves >= _waveCount;
    
    public void Enter(BattleContext context) => _elapsedSinceLastSpawn = _interval;

    public void Update(BattleContext context, float deltaTime)
    {
        if (IsFinished) return;
        
        _elapsedSinceLastSpawn += deltaTime;
        if (_elapsedSinceLastSpawn < _interval) return;

        _elapsedSinceLastSpawn = 0f;
        context.Bullets.Add(new Bullet
        {
            Position = new Vector2(context.Arena.Center.X, context.Arena.Top - 10),
            Velocity = new Vector2(0, _speed),
            Radius = 6f,
            Damage = _damage
        });
        _spawnedWaves++;
    }
}