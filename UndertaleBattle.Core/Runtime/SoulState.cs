using System.Numerics;
using UndertaleBattle.Core.Interfaces;

namespace UndertaleBattle.Core.Runtime;

/// <summary>
/// Mutable runtime state of the player's soul for one battle.
/// </summary>
public sealed class SoulState
{
    public Vector2 Position { get; internal set; }
    
    public float Speed { get; }
    
    public float Radius { get; }
    
    public int Health { get; private set; }
    
    public int MaxHealth { get; }

    public bool IsDead => Health <= 0;
    
    public bool IsInvulnerable { get; private set; }
    
    public float InvulnerabilityTimer { get; private set; }

    public SoulState(
        int maxHealth,
        Vector2 startPosition,
        float speed = 200f,
        float radius = 8f)
    {
        if (maxHealth <= 0)
            throw new ArgumentOutOfRangeException(nameof(maxHealth));

        if (speed <= 0f)
            throw new ArgumentOutOfRangeException(nameof(speed));

        if (radius <= 0f)
            throw new ArgumentOutOfRangeException(nameof(radius));
        
        MaxHealth = maxHealth;
        Health = maxHealth;
        Position = startPosition;
        Speed = speed;
        Radius = radius;
    }

    public void Heal(int amount)
    {
        if (amount <= 0)
            return;
        
        Health = Math.Min(MaxHealth, Health + amount);
    }

    public bool TryTakeDamage(int amount, float invulnerabilitySeconds)
    {
        if (amount <= 0 || IsInvulnerable || IsDead)
            return false;

        Health = Math.Max(0, Health - amount);

        IsInvulnerable = true;
        InvulnerabilityTimer = Math.Max(0f, invulnerabilitySeconds);

        return true;
    }

    internal void TickInvulnerability(float deltaTime)
    {
        if (!IsInvulnerable)
            return;

        InvulnerabilityTimer = Math.Max(0f, InvulnerabilityTimer - deltaTime);

        if (InvulnerabilityTimer <= 0f)
            IsInvulnerable = false;
    }

    internal void ClampTo(IArenaShape arena)
    {
        Position = arena.Clamp(Position);
    }
}