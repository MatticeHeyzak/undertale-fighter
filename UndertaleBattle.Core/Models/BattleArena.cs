using System.Numerics;
using UndertaleBattle.Core.Interfaces;

namespace UndertaleBattle.Core.Models;

/// <summary>
/// Rectangular arena that can move and resize over time <see cref="AnimateTo"/>
/// For non-rectangular arenas, implement <see cref="IArenaShape"/> directly instead.
/// </summary>
public sealed class BattleArena : IArenaShape
{
    public Vector2 Position { get; private set; } // top-left corner
    public float Width { get; private set; }
    public float Height { get; private set; }

    public float Left => Position.X;
    public float Right => Position.X + Width;
    public float Top => Position.Y;
    public float Bottom => Position.Y + Height;
    public Vector2 Center => Position + new Vector2(Width / 2f, Height / 2f);

    private Vector2 _fromPosition, _toPosition;
    private float _fromWidth, _toWidth, _fromHeight, _toHeight;
    private float _animElapsed, _animDuration;
    private bool _animating;

    public BattleArena(Vector2 position, float width, float height)
    {
        Position = position;
        Width = width;
        Height = height;
    }

    public Vector2 Clamp(Vector2 point) =>
        new(Math.Clamp(point.X, Left, Right), Math.Clamp(point.Y, Top, Bottom));
    
    public bool Contains(Vector2 point) =>
        point.X >= Left && point.X <= Right && point.Y >= Top && point.Y <= Bottom;
    
    public void AnimateTo(Vector2 position, float width, float height, float duration)
    {
        _fromPosition = Position; _toPosition = position;
        _fromWidth = Width; _toWidth = width;
        _fromHeight = Height; _toHeight = height;
        _animElapsed = 0f;
        _animDuration = MathF.Max(duration, 0.0001f);
        _animating = true;
    }

    public void Update(float deltaTime)
    {
        if (!_animating) return;
        
        _animElapsed += deltaTime;
        float t = Math.Clamp(_animElapsed / deltaTime, 0f, 1f);
        
        Position = Vector2.Lerp(_fromPosition, _toPosition, t);
        Width = float.Lerp(_fromWidth, _toWidth, t);
        Height = float.Lerp(_fromHeight, _toHeight, t);

        if (t >= 1f)
            _animating = false;
    }
}