using System.Diagnostics;
using System.Numerics;
using UndertaleBattle.Core.Models;

namespace UndertaleBattle.Core.Context;

public class HeartSoul
{
    public Vector2 Position { get; private set; }
    public float Speed { get; private set; } = 200f;
    
    public int Health { get; private set; }
    public int MaxHealth { get; private set; }

    /// <summary>True while the soul cannot take damage (e.g. right after being hit)</summary>
    public bool IsInvulnerable { get; private set; }
    
    /// <summary>Remaining invulnerability time</summary>
    public float InvulnerabilityTimer { get; private set; }
    
    public bool IsDead => Health <= 0;
    public float Radius { get; init; } = 8f;
    

    public HeartSoul(int maxHealth, Vector2 startPosition)
    {
        MaxHealth = maxHealth;
        Health = maxHealth;
        Position = startPosition;
    }

    public void Heal(int amount)
    {
        if (amount <= 0)
            return;
        
        Health = Math.Min(MaxHealth, Health + amount);
    }

    /// <summary>Moves the soul by a normalized direction, scaled by speed and deltaTime.</summary>
    public void Move(Vector2 direction, float deltaTime)
    {
        if (direction == Vector2.Zero)
            return;
        
        Position += Vector2.Normalize(direction) * Speed * deltaTime;
    }

    public void TakeDamage(int amount, float invulnerabilitySeconds)
    {
        if (IsInvulnerable || amount <= 0)
            return;
        
        Health = Math.Max(0, Health - amount);
        IsInvulnerable = true;
        InvulnerabilityTimer = invulnerabilitySeconds;
    }

    public void TickInvulnerability(float deltaTime)
    {
        if (!IsInvulnerable)
            return;

        InvulnerabilityTimer -= deltaTime;
        if (InvulnerabilityTimer <= 0f)
        {
            IsInvulnerable = false;
            InvulnerabilityTimer = 0f;
        }
    }
    
    public void ClampTo(BattleArena arena) => Position = arena.Clamp(Position);
}