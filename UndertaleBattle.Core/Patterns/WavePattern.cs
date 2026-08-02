using System.Numerics;
using UndertaleBattle.Core.Interfaces;
using UndertaleBattle.Core.Models;
using UndertaleBattle.Core.Runtime;

namespace UndertaleBattle.Core.Patterns;

public class WavePattern : IAttackPattern
{
    private readonly int _waveCount;
    private readonly float _interval;
    private readonly float _speed;
    private readonly int _damage;

    private int _spawnedWaves;
    private float _timeUntilNextSpawn;

    public WavePattern(
        int waveCount = 6,
        float interval = 0.4f,
        float speed = 160f,
        int damage = 5)
    {
        if (waveCount <= 0)
            throw new ArgumentOutOfRangeException(nameof(waveCount));

        if (interval <= 0f)
            throw new ArgumentOutOfRangeException(nameof(interval));

        if (speed <= 0f)
            throw new ArgumentOutOfRangeException(nameof(speed));

        if (damage <= 0)
            throw new ArgumentOutOfRangeException(nameof(damage));

        _waveCount = waveCount;
        _interval = interval;
        _speed = speed;
        _damage = damage;
    }

    public bool IsFinished => _spawnedWaves >= _waveCount;

    public void Enter(BattleSession session)
    {
        ArgumentNullException.ThrowIfNull(session);

        _spawnedWaves = 0;
        _timeUntilNextSpawn = 0f;
    }

    public void Update(BattleSession session, float deltaTime)
    {
        if (IsFinished)
            return;

        _timeUntilNextSpawn -= deltaTime;

        // A while loop prevents missed waves on a slow frame.
        while (_timeUntilNextSpawn <= 0f && !IsFinished)
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