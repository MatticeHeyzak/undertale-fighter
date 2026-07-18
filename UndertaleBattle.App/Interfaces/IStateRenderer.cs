using UndertaleBattle.Core.Context;
using UndertaleBattle.Core.Enums;

namespace UndertaleBattle.Interfaces;

public interface IStateRenderer
{
    BattleStateIdentity TargetState { get; }
    void Draw(BattleContext context);
}