using System.Numerics;

namespace UndertaleBattle.Core.Models;

public sealed class Bullet
{
    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; }
    public float Radius { get; init; }
    public int Damage { get; init; }
    public bool IsAlive { get; set; } = true;

    public void Update(float deltaTime)
    {
        Position += Velocity * deltaTime;
    }
}