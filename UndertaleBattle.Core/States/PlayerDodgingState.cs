using UndertaleBattle.Core.Enums;
using UndertaleBattle.Core.Input;
using UndertaleBattle.Core.Interfaces;
using UndertaleBattle.Core.Runtime;
using UndertaleBattle.Core.Systems;

namespace UndertaleBattle.Core.States;

/// <summary>
/// Coordinates the dodge phase. Reusable systems own movement, projectile,
/// collision, and cleanup behavior.
/// </summary>
public sealed class PlayerDodgingState : IBattleState
{
    private readonly float _phaseDuration;
    private readonly ISoulSystem _soulSystem;
    private readonly IProjectileSystem _projectileSystem;
    private readonly ICollisionSystem _collisionSystem;

    private float _elapsed;

    public BattleStateIdentity Identity => BattleStateIdentity.PlayerDodging;

    public PlayerDodgingState(
        ISoulSystem soulSystem,
        IProjectileSystem projectileSystem,
        ICollisionSystem collisionSystem,
        float phaseDuration = 6f)
    {
        if (phaseDuration <= 0f)
            throw new ArgumentOutOfRangeException(nameof(phaseDuration));

        _soulSystem = soulSystem ?? throw new ArgumentNullException(nameof(soulSystem));
        _projectileSystem = projectileSystem ?? throw new ArgumentNullException(nameof(projectileSystem));
        _collisionSystem = collisionSystem ?? throw new ArgumentNullException(nameof(collisionSystem));
        _phaseDuration = phaseDuration;
    }

    public BattleStateIdentity? Enter(BattleSession session)
    {
        _elapsed = 0f;

        // Do not clear projectiles here.
        // EnemyTurnState has just initialized the active attack and may already
        // have spawned projectiles.
        return null;
    }

    public BattleStateIdentity? Update(
        BattleSession session,
        BattleInput input,
        float deltaTime)
    {
        _elapsed += deltaTime;

        _soulSystem.Update(
            session.Player,
            session.Arena,
            input,
            deltaTime);

        session.Combat.ActiveAttackPattern?.Update(session, deltaTime);

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

        bool attackFinished =
            session.Combat.ActiveAttackPattern?.IsFinished ?? true;

        if (session.Player.IsDead)
            return BattleStateIdentity.Menu;

        if (_elapsed >= _phaseDuration && attackFinished)
            return BattleStateIdentity.Menu;

        return null;
    }

    public void Exit(BattleSession session)
    {
        session.Combat.EndAttack();
    }
}