using UndertaleBattle.Core.Runtime;

namespace UndertaleBattle.Core.Systems;

public interface IProjectileSystem
{
    void Update(CombatState combat, ArenaState arena, float deltaTime);
    
    void RemoveExpired(CombatState combat, ArenaState arena);
}