using System.Numerics;

namespace UndertaleBattle.Core.Interfaces;

/// <summary>
/// Defines the playable bounds for the current battle phase.
/// </summary>
public interface IArenaShape
{
    Vector2 Center { get; }
    float Left { get; }
    float Right { get; }
    float Top { get; }
    float Bottom { get; }
    float Width => Right - Left;
    float Height => Bottom - Top;

    Vector2 Clamp(Vector2 point);
    bool Contains(Vector2 point);

    void AnimateTo(Vector2 position, float width, float height, float duration);
    void Update(float deltaTime);
}