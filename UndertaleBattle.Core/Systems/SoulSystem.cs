using System.Numerics;
using UndertaleBattle.Core.Input;
using UndertaleBattle.Core.Runtime;

namespace UndertaleBattle.Core.Systems;

/// <summary>
/// Updates standard free-movement soul behaviour.
/// </summary>
public class SoulSystem : ISoulSystem
{
    public void Update(
        SoulState soul,
        ArenaState arena,
        BattleInput input,
        float deltaTime)
    {
        ArgumentNullException.ThrowIfNull(soul);
        ArgumentNullException.ThrowIfNull(arena);
        
        if (deltaTime < 0f)
            throw new ArgumentOutOfRangeException(nameof(deltaTime));
        
        
    }

    private static void Move(
        SoulState soul,
        Vector2 direction,
        float deltaTime)
    {
        if (direction == Vector2.Zero)
            return;
        
        soul.Position += Vector2.Normalize(direction) * soul.Speed * deltaTime;
    }
}