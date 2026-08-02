using UndertaleBattle.Core.Interfaces;
using UndertaleBattle.Core.Models;

namespace UndertaleBattle.Core.Runtime;

/// <summary>
/// Mutable combat runtime for one battle. Projectile ownership remains here;
/// callers cannot modify the collection directly.
/// </summary>
public sealed class CombatState
{
    private readonly List<Bullet> _projectiles = new();

    public Enemy CurrentEnemy { get; }

    public IAttackPattern? ActiveAttackPattern { get; private set; }

    public IReadOnlyList<Bullet> Projectiles => _projectiles;

    public CombatState(Enemy currentEnemy)
    {
        CurrentEnemy = currentEnemy ??
                       throw new ArgumentNullException(nameof(currentEnemy));
    }

    public void BeginAttack(IAttackPattern attackPattern)
    {
        ArgumentNullException.ThrowIfNull(attackPattern);

        _projectiles.Clear();
        ActiveAttackPattern = attackPattern;
    }

    public void EndAttack()
    {
        ActiveAttackPattern = null;
        _projectiles.Clear();
    }

    public void SpawnProjectile(Bullet projectile)
    {
        ArgumentNullException.ThrowIfNull(projectile);
        _projectiles.Add(projectile);
    }

    internal void RemoveProjectiles(Predicate<Bullet> shouldRemove)
    {
        ArgumentNullException.ThrowIfNull(shouldRemove);
        _projectiles.RemoveAll(shouldRemove);
    }
}