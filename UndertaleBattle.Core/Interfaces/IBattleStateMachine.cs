using UndertaleBattle.Core.Enums;
using UndertaleBattle.Core.Input;
using UndertaleBattle.Core.Runtime;

namespace UndertaleBattle.Core.Interfaces;

public interface IBattleStateMachine
{
    IBattleState? CurrentState { get; }

    void RegisterState(IBattleState state);

    void ChangeState(BattleStateIdentity identity, BattleSession session);

    void Update(
        BattleSession session,
        BattleInput input,
        float deltaTime);
}