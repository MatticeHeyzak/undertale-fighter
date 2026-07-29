using System.Numerics;
using UndertaleBattle.Core.Interfaces;
using UndertaleBattle.Core.Patterns.Motion;

namespace UndertaleBattle.Core.Models;

public sealed class Bullet
{
    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; }
    public float Radius { get; init; }
    public int Damage { get; init; }
    public bool IsAlive { get; set; } = true;

    private IBulletMotion Motion { get; init; } = new LinearMotion();

    public void Update(float deltaTime) => Motion.Update(this, deltaTime);
}