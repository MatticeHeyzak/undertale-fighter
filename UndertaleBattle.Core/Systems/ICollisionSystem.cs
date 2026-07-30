using UndertaleBattle.Core.Runtime;

namespace UndertaleBattle.Core.Systems;

public interface ICollisionSystem
{
    void ResolvePlayerProjectileCollisions(SoulState player, CombatState combat);
}