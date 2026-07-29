using UndertaleBattle.Core.Context;
using UndertaleBattle.Core.Enums;
using UndertaleBattle.Core.Interfaces;

namespace UndertaleBattle.Core.States;

public class BattleStateMachine : IBattleStateMachine
{
    private readonly Dictionary<BattleStateIdentity, IBattleState> _states = new();
    
    public IBattleState? CurrentState { get; private set; }

    public void RegisterState(IBattleState state)
    {
        ArgumentNullException.ThrowIfNull(state);

        if (!_states.TryAdd(state.Identity, state))
        {
            throw new InvalidOperationException(
                $"A state is already registered for '{state.Identity}'. " +
                "Register exactly one implementation for each battle-state identity.");
        }
    }

    public void ChangeState(BattleStateIdentity identity, BattleContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        if (!_states.TryGetValue(identity, out var nextState))
        {
            throw new InvalidOperationException(
                $"No state is registered for '{identity}'. " +
                "Register it before attempting to transition.");
        }

        if (ReferenceEquals(CurrentState, nextState))
            return;
        
        CurrentState?.Exit(context);
        
        // Prevent an input which initiated the transition from being interpreted by the newly entered state in the same or immediately following frame
        context.ClearTransientInput();
        
        CurrentState = nextState;
        CurrentState.Enter(context);
    }

    public void Update(BattleContext context, float deltaTime)
    {
        ArgumentNullException.ThrowIfNull(context);
        
        if (CurrentState is null)
        {
            throw new InvalidOperationException(
                "Update was called before a battle state was activated. " +
                "Call ChangeState first.");
        }

        if (deltaTime < 0f)
            throw new ArgumentOutOfRangeException(nameof(deltaTime));
        
        context.Arena.Update(deltaTime);
        CurrentState.Update(context, deltaTime);
    }
}