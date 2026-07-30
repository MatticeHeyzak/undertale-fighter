using UndertaleBattle.Core.Enums;
using UndertaleBattle.Core.Runtime;

namespace UndertaleBattle.Interfaces;

public interface IStateRenderer
{
    BattleStateIdentity TargetState { get; }
    void Draw(BattleSession context);
}