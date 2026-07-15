using System.Numerics;

namespace UndertaleBattle.Core.Models;

public sealed class BattleArena
{
    public Vector2 Position { get; } // top-left corner
    public float Width { get; }
    public float Height { get; }

    public float Left => Position.X;
    public float Right => Position.X + Width;
    public float Top => Position.Y;
    public float Bottom => Position.Y + Height;

    public BattleArena(Vector2 position, float width, float height)
    {
        Position = position;
        Width = width;
        Height = height;
    }
    
    public Vector2 Clamp(Vector2 point) =>
        new(Math.Clamp(point.X, Left, Right), Math.Clamp(point.Y, Top, Bottom));
}