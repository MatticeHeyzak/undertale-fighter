using UndertaleBattle.Core.Runtime;

namespace UndertaleBattle.Core.Systems;

public interface IArenaSystem
{
    void Update(ArenaState arena, float deltaTime);
}