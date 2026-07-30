using UndertaleBattle.Core.Input;
using UndertaleBattle.Core.Runtime;

namespace UndertaleBattle.Core.Systems;

public interface ISoulSystem
{
    void Update(
        SoulState soul,
        ArenaState arena,
        BattleInput input,
        float deltaTime);
}