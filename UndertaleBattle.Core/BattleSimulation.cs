using UndertaleBattle.Core.Enums;
using UndertaleBattle.Core.Input;
using UndertaleBattle.Core.Interfaces;
using UndertaleBattle.Core.Runtime;
using UndertaleBattle.Core.Systems;

namespace UndertaleBattle.Core;

/// <summary>
/// Coordinates the simulation update order for one battle.
/// Rendering and platform input remain outside Core.
/// </summary>
public sealed class BattleSimulation
{
    private readonly IBattleStateMachine _stateMachine;
    private readonly IArenaSystem _arenaSystem;

    public BattleSession Session { get; }

    public BattleStateIdentity CurrentState =>
        _stateMachine.CurrentState?.Identity
        ?? throw new InvalidOperationException(
            "The battle simulation has not been started.");

    public BattleSimulation(
        BattleSession session,
        IBattleStateMachine stateMachine,
        IArenaSystem arenaSystem)
    {
        Session = session ?? throw new ArgumentNullException(nameof(session));
        _stateMachine = stateMachine ?? throw new ArgumentNullException(nameof(stateMachine));
        _arenaSystem = arenaSystem ?? throw new ArgumentNullException(nameof(arenaSystem));
    }

    public void Start(BattleStateIdentity initialState)
    {
        _stateMachine.ChangeState(initialState, Session);
    }

    public void Update(BattleInput input, float deltaTime)
    {
        if (deltaTime < 0f)
            throw new ArgumentOutOfRangeException(nameof(deltaTime));

        _arenaSystem.Update(Session.Arena, deltaTime);
        _stateMachine.Update(Session, input, deltaTime);
    }
}