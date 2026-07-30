using System.Numerics;
using UndertaleBattle.Core.Runtime;

namespace UndertaleBattle.Core.Systems;

/// <summary>
/// Resolves player/projectile circle collisions.
/// todo: make collision rules pluggable
/// </summary>
public class CollisionSystem : ICollisionSystem
{
    private const float PlayerHitInvulnerabilitySeconds = 1.5f;

    public void ResolvePlayerProjectileCollisions(SoulState player, CombatState combat)
    {
        ArgumentNullException.ThrowIfNull(player);
        ArgumentNullException.ThrowIfNull(combat);

        foreach (var projectile in combat.Projectiles)
        {
            if (!projectile.IsAlive)
                continue;

            float collistionDistance = player.Radius + projectile.Radius;
            
            float distanceSquared = Vector2.DistanceSquared(player.Position, projectile.Position);

            if (distanceSquared > collistionDistance * collistionDistance)
                continue;

            bool damaged = player.TryTakeDamage(
                projectile.Damage,
                PlayerHitInvulnerabilitySeconds);
            
            if (damaged && player.IsDead)
                return;
        }
    }
}