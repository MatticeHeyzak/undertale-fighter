using UndertaleBattle.Core.Enums;
using UndertaleBattle.Core.Runtime;

namespace UndertaleBattle.Interfaces;

public interface IRaylibRenderer
{
    void Draw(BattleSession context, BattleStateIdentity currentState);
}