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
        _states[state.Identity] = state;
    }

    public void ChangeState(BattleStateIdentity identity, BattleContext context)
    {
        if (!_states.TryGetValue(identity, out var nextState))
            throw new InvalidOperationException($"No state registered for '{identity}'. Did you forget to call RegisterState()?");

        CurrentState?.Exit(context);
        
        CurrentState = nextState;
        context.CurrentState = identity;
        
        CurrentState.Enter(context);
    }

    public void Update(BattleContext context, float deltaTime)
    {
        if (CurrentState is null)
            throw new InvalidOperationException("Update() called before any state was activated. Call ChangeState() first.");

        context.Arena.Update(deltaTime);
        CurrentState.Update(context, deltaTime);
    }
}