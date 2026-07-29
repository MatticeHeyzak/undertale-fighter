using System.Numerics;
using UndertaleBattle.Core.Context;
using UndertaleBattle.Core.Interfaces;
using UndertaleBattle.Core.Models;

namespace UndertaleBattle.Core.Patterns.Motion;

public class HomingMotion : IBulletMotion
{
    private readonly HeartSoul _target;
    private readonly float _turnRateRadiansPerSecond;

    public HomingMotion(HeartSoul target, float turnRateRadiansPerSecond = 3f)
    {
        _target = target;
        _turnRateRadiansPerSecond = turnRateRadiansPerSecond;
    }

    public void Update(Bullet bullet, float deltaTime)
    {
        var toTarget = _target.Position - bullet.Position;
        if (toTarget != Vector2.Zero)
        {
            var desired = Vector2.Normalize(toTarget) * bullet.Velocity.Length();
            float maxTurn = _turnRateRadiansPerSecond * deltaTime;
            bullet.Velocity = RotateTowards(bullet.Velocity, desired, maxTurn);
        }
        
        bullet.Position += bullet.Velocity * deltaTime;
    }

    private static Vector2 RotateTowards(Vector2 current, Vector2 desired, float maxRadians)
    {
        float currentAngle = MathF.Atan2(current.Y, current.X);
        float desiredAngle = MathF.Atan2(desired.Y, desired.X);
        float delta = MathF.Atan2(MathF.Sin(desiredAngle - currentAngle), MathF.Cos(desiredAngle - currentAngle));
        float clamped = Math.Clamp(delta, -maxRadians, maxRadians);
        float newAngle = currentAngle + clamped;
        return new Vector2(MathF.Cos(newAngle), MathF.Sin(newAngle)) * current.Length();
    }
}