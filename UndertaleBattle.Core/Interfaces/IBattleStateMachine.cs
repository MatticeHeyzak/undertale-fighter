using UndertaleBattle.Core.Context;
using UndertaleBattle.Core.Enums;

namespace UndertaleBattle.Core.Interfaces;

public interface IBattleStateMachine
{
    IBattleState? CurrentState { get; }
    
    /// <summary>
    /// Adds a state to the registry so it can later be switched to via <see cref="ChangeState"/>.
    /// </summary>
    void RegisterState(IBattleState state);
    
    /// <summary>
    /// Exits the current state (if any) and enters the state registered under <paramref cref="identity"/>.
    /// </summary>
    void ChangeState(BattleStateIdentity identity, BattleContext context);
    
    /// <summary>
    /// Forwards Update() to whichever state is currently active.
    /// </summary>
    void Update(BattleContext context, float deltaTime);
}