using UndertaleBattle.Core.Enums;
using UndertaleBattle.Core.Input;
using UndertaleBattle.Core.Interfaces;
using UndertaleBattle.Core.Runtime;
using UndertaleBattle.Core.Systems;

namespace UndertaleBattle.Core.States;

/// <summary>
/// Coordinates the dodge phase. Reusable systems own movement, projectile,
/// collision, and cleanup behavior. The active attack owns its own completion
/// policy and signals when this phase may end.
/// </summary>
public sealed class PlayerDodgingState : IBattleState
{
    private readonly ISoulSystem _soulSystem;
    private readonly IProjectileSystem _projectileSystem;
    private readonly ICollisionSystem _collisionSystem;

    public BattleStateIdentity Identity => BattleStateIdentity.PlayerDodging;

    public PlayerDodgingState(
        ISoulSystem soulSystem,
        IProjectileSystem projectileSystem,
        ICollisionSystem collisionSystem)
    {
        _soulSystem = soulSystem ??
                      throw new ArgumentNullException(nameof(soulSystem));
        _projectileSystem = projectileSystem ??
                            throw new ArgumentNullException(nameof(projectileSystem));
        _collisionSystem = collisionSystem ??
                           throw new ArgumentNullException(nameof(collisionSystem));
    }
    
    public BattleStateIdentity? Enter(BattleSession session)
    {
        ArgumentNullException.ThrowIfNull(session);

        if (session.Combat.ActiveAttackPattern is null)
        {
            throw new InvalidOperationException(
                "Player dodging requires an active attack pattern.");
        }

        // Do not clear projectiles here. EnemyTurnState has already initialized
        // the active attack, which may have spawned projectiles in Enter().
        return null;
    }

    public BattleStateIdentity? Update(
        BattleSession session,
        BattleInput input,
        float deltaTime)
    {
        ArgumentNullException.ThrowIfNull(session);

        if (deltaTime < 0f)
            throw new ArgumentOutOfRangeException(nameof(deltaTime));

        IAttackPattern attack = session.Combat.ActiveAttackPattern
                                ?? throw new InvalidOperationException(
                                    "Player dodging was updated without an active attack pattern.");

        _soulSystem.Update(
            session.Player,
            session.Arena,
            input,
            deltaTime);

        attack.Update(session, deltaTime);

        _projectileSystem.Update(
            session.Combat,
            session.Arena,
            deltaTime);

        _collisionSystem.ResolvePlayerProjectileCollisions(
            session.Player,
            session.Combat);

        _projectileSystem.RemoveExpired(
            session.Combat,
            session.Arena);

        if (session.Player.IsDead)
        {
            session.Complete(BattleOutcome.PlayerDefeated);
            return BattleStateIdentity.Menu;
        }

        return attack.IsComplete
            ? BattleStateIdentity.Menu
            : null;
    }

    public void Exit(BattleSession session)
    {
        ArgumentNullException.ThrowIfNull(session);
        session.Combat.EndAttack();
    }
}