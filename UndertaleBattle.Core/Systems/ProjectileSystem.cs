using UndertaleBattle.Core.Runtime;

namespace UndertaleBattle.Core.Systems;

/// <summary>
/// Advances projectiles and removes those marked dead or safely outside arena bounds.
/// </summary>
public sealed class ProjectileSystem : IProjectileSystem
{
    private const float DespawnPadding = 20f;
    
    public void Update(
        CombatState combat,
        ArenaState arena,
        float deltaTime)
    {
        ArgumentNullException.ThrowIfNull(combat);
        ArgumentNullException.ThrowIfNull(arena);

        if (deltaTime < 0f)
            throw new ArgumentOutOfRangeException(nameof(deltaTime));

        foreach (var projectile in combat.Projectiles)
        {
            if (projectile.IsAlive)
                projectile.Update(deltaTime);
        }
    }

    public void RemoveExpired(CombatState combat, ArenaState arena)
    {
        ArgumentNullException.ThrowIfNull(combat);
        ArgumentNullException.ThrowIfNull(arena);

        combat.Projectiles.RemoveAll(projectile =>
            !projectile.IsAlive ||
            IsOutsideDespawnBounds(projectile, arena));
    }

    private static bool IsOutsideDespawnBounds(
        Models.Bullet projectile,
        ArenaState arena)
    {
        var shape = arena.Shape;

        return projectile.Position.X < shape.Left - DespawnPadding ||
               projectile.Position.X > shape.Right + DespawnPadding ||
               projectile.Position.Y < shape.Top - DespawnPadding ||
               projectile.Position.Y > shape.Bottom + DespawnPadding;
    }
}