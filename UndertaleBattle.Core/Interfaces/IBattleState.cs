using UndertaleBattle.Core.Context;
using UndertaleBattle.Core.Enums;

namespace UndertaleBattle.Core.Interfaces;

public interface IBattleState
{
    BattleStateIdentity Identity { get; }
    
    /// <summary>
    /// Called exactly once, the frame this state becomes active.
    /// </summary>
    void Enter(BattleContext context);
    
    void Update(BattleContext context, float deltaTime);
    
    /// <summary>
    /// Called exactly once, the frame this state stops being active.
    /// </summary>
    void Exit(BattleContext context);
}