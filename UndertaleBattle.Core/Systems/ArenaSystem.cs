using UndertaleBattle.Core.Runtime;

namespace UndertaleBattle.Core.Systems;

/// <summary>
/// Advances arena animation/control independently of battle flow states.
/// </summary>
public class ArenaSystem : IArenaSystem
{
    public void Update(ArenaState arena, float deltaTime)
    {
        ArgumentNullException.ThrowIfNull(arena);
        
        if (deltaTime < 0f)
            throw new ArgumentOutOfRangeException(nameof(deltaTime));
        
        arena.Shape.Update(deltaTime);
    }
}