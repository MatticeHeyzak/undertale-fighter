using System.Numerics;
using UndertaleBattle.Core.Interfaces;
using UndertaleBattle.Core.Models;
using UndertaleBattle.Core.Runtime;

namespace UndertaleBattle.Core.Patterns;

/// <summary>
/// Spawns vertical waves on a schedule and owns the duration of its dodge
/// phase.
/// </summary>
public sealed class WavePattern : IAttackPattern
{
    private readonly int _waveCount;
    private readonly float _interval;
    private readonly float _speed;
    private readonly int _damage;
    private readonly float _duration;

    private int _spawnedWaves;
    private float _timeUntilNextSpawn;
    private float _elapsed;

    public WavePattern(
        int waveCount = 6,
        float interval = 0.4f,
        float speed = 160f,
        int damage = 5,
        float duration = 6f)
    {
        if (waveCount <= 0)
            throw new ArgumentOutOfRangeException(nameof(waveCount));

        if (interval <= 0f)
            throw new ArgumentOutOfRangeException(nameof(interval));

        if (speed <= 0f)
            throw new ArgumentOutOfRangeException(nameof(speed));

        if (damage <= 0)
            throw new ArgumentOutOfRangeException(nameof(damage));

        if (duration <= 0f)
            throw new ArgumentOutOfRangeException(nameof(duration));

        _waveCount = waveCount;
        _interval = interval;
        _speed = speed;
        _damage = damage;
        _duration = duration;
    }

    public bool IsComplete =>
        _spawnedWaves >= _waveCount &&
        _elapsed >= _duration;

    public void Enter(BattleSession session)
    {
        ArgumentNullException.ThrowIfNull(session);

        _spawnedWaves = 0;
        _timeUntilNextSpawn = 0f;
        _elapsed = 0f;
    }

    public void Update(BattleSession session, float deltaTime)
    {
        ArgumentNullException.ThrowIfNull(session);

        if (deltaTime < 0f)
            throw new ArgumentOutOfRangeException(nameof(deltaTime));

        _elapsed += deltaTime;

        if (_spawnedWaves >= _waveCount)
            return;

        _timeUntilNextSpawn -= deltaTime;

        // Ensures waves are not missed if a future caller uses a larger step.
        while (_timeUntilNextSpawn <= 0f &&
               _spawnedWaves < _waveCount)
        {
            SpawnWave(session);

            _spawnedWaves++;
            _timeUntilNextSpawn += _interval;
        }
    }

    private void SpawnWave(BattleSession session)
    {
        var arena = session.Arena.Shape;

        session.Combat.SpawnProjectile(new Bullet
        {
            Position = new Vector2(arena.Center.X, arena.Top - 10f),
            Velocity = new Vector2(0f, _speed),
            Radius = 6f,
            Damage = _damage
        });
    }
}