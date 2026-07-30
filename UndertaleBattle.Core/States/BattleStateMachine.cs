using UndertaleBattle.Core.Enums;
using UndertaleBattle.Core.Input;
using UndertaleBattle.Core.Interfaces;
using UndertaleBattle.Core.Runtime;

namespace UndertaleBattle.Core.States;

/// <summary>
/// Controls high-level battle flow.
/// Gameplay systems are owned externally and used by states as dependencies.
/// </summary>
public sealed class BattleStateMachine : IBattleStateMachine
{
    private const int MaximumImmediateTransitions = 16;

    private readonly Dictionary<BattleStateIdentity, IBattleState> _states = new();

    public IBattleState? CurrentState { get; private set; }

    public void RegisterState(IBattleState state)
    {
        ArgumentNullException.ThrowIfNull(state);

        if (!_states.TryAdd(state.Identity, state))
        {
            throw new InvalidOperationException(
                $"A state is already registered for '{state.Identity}'.");
        }
    }

    public void ChangeState(
        BattleStateIdentity identity,
        BattleSession session)
    {
        ArgumentNullException.ThrowIfNull(session);

        TransitionTo(identity, session);
    }

    public void Update(
        BattleSession session,
        BattleInput input,
        float deltaTime)
    {
        ArgumentNullException.ThrowIfNull(session);

        if (deltaTime < 0f)
            throw new ArgumentOutOfRangeException(nameof(deltaTime));

        if (CurrentState is null)
        {
            throw new InvalidOperationException(
                "Update was called before a battle state was activated.");
        }

        BattleStateIdentity? nextState =
            CurrentState.Update(session, input, deltaTime);

        if (nextState.HasValue)
            TransitionTo(nextState.Value, session);
    }

    private void TransitionTo(
        BattleStateIdentity initialState,
        BattleSession session)
    {
        BattleStateIdentity? requestedState = initialState;
        int transitionCount = 0;

        while (requestedState.HasValue)
        {
            if (++transitionCount > MaximumImmediateTransitions)
            {
                throw new InvalidOperationException(
                    "Exceeded the maximum number of immediate state transitions. " +
                    "A state likely transitions in a loop from Enter().");
            }

            if (!_states.TryGetValue(requestedState.Value, out var nextState))
            {
                throw new InvalidOperationException(
                    $"No state is registered for '{requestedState.Value}'.");
            }

            if (ReferenceEquals(CurrentState, nextState))
                return;

            CurrentState?.Exit(session);

            CurrentState = nextState;
            requestedState = CurrentState.Enter(session);
        }
    }
}