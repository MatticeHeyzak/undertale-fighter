using UndertaleBattle.Core.Interfaces;
using UndertaleBattle.Core.Models;

namespace UndertaleBattle.Core.Runtime;

/// <summary>
/// Runtime combat data: active target, active enemy attack, and projectiles.
/// </summary>
public sealed class CombatState
{
    public Enemy? CurrentEnemy { get; set; }
    
    public IAttackPattern? ActiveAttackPattern { get; private set; }

    public List<Bullet> Projectiles { get; } = new();
    
    public bool IsBattleOver { get; private set; }

    public void BeginAttack(IAttackPattern attackPattern)
    {
        ArgumentNullException.ThrowIfNull(attackPattern);
        
        Projectiles.Clear();
        ActiveAttackPattern = attackPattern;
    }

    public void EndAttack()
    {
        ActiveAttackPattern = null;
        Projectiles.Clear();
    }

    public void MarkBattleOver()
    {
        IsBattleOver = true;
        EndAttack();
    }

    public void RemoveInactiveProjectiles()
    {
        Projectiles.RemoveAll(projectile => !projectile.IsAlive);
    }
}